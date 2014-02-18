using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;
using System.Data.Entity;
using System.Web.Security;
using Imzist.Data;
using Imzist.Logic;
using Imzist.Web.Helpers;
using Imzist.Web.Models;
using System.Web;


namespace Imzist.Web.Controllers
{
    public class ItemController : Controller
    {
        private bool IsPoster(Guid userId)
        {
            if (Membership.GetUser(User.Identity.Name) == null)
            {
                //not logged in so obv not the poster
                return false;
            }

           
            //check if the current logged in user is the poster
            return userId == (Guid) Membership.GetUser(User.Identity.Name).ProviderUserKey;

        }
        private Item ImageProcesser(Item item, IEnumerable<Image> imagesDb)
        {
            int counter = imagesDb.Count();
            foreach (string file in Request.Files)
            {
                var imageFile = Request.Files[file];
                if (imageFile == null)
                {
                    continue;
                }
                if (counter > 5) //limit of 5 images per item
                {
                    break;
                }
                if (item.Images.Any(image => image.Name == imageFile.FileName))
                {
                    //don't need to process existing image
                    break;
                }
                var imageBytes = new byte[imageFile.InputStream.Length];
                imageFile.InputStream.Read(imageBytes, 0, imageBytes.Length);
                var imageStream = new MemoryStream(imageBytes);

                if (ImageHelper.IsValidImage(imageStream))
                {
                    string newFileName = ImageHelper.RenameImageFile(imageFile.FileName);
                    Image img = new Image { Name = newFileName };
                    imageFile.SaveAs(Path.Combine(Server.MapPath("~/content/images/Full"), newFileName));
                    ImageHelper.ThumbnailMaker(imageStream,
                                               Path.Combine(Server.MapPath("~/Content/Images/Thumbnail"), newFileName),
                                               50);
                    item.Images.Add(img);
                }

                imageFile.InputStream.Dispose();
                imageStream.Dispose();
                counter++;
            }
            return item;
        }
        public ActionResult Index(int id)
        {
            using (var dbcontext = new ImzistEntities())
            {
                IEnumerable<Item> items = dbcontext.Items.Include(it => it.Images);
                Item item = items.FirstOrDefault(i => i.Id == id);
                bool isPoster = IsPoster(item.UserId);
                
                return View(new ItemViewModel() {Item = item, IsPoster = isPoster});
            }
        }

        [Authorize]
        public ActionResult Add()
        {
            var model = new ItemListingViewModel();
            model.UserId = (Guid) Membership.GetUser(User.Identity.Name).ProviderUserKey;
            model.Item = new Item(); //this feels like a hack - instead of checking for null in the partial
            using (var dbContext = new ImzistEntities())
            {
                model.Categories = dbContext.Categories.ToList();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(Item item, int expirationDays)
        {
            item.LocationId = LocationResolver.GetLocation().Id;
            item.PostedDate = DateTime.Now;
            item.ExpirationDate = item.PostedDate.AddDays(expirationDays);
            item.UserId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
            item = ImageProcesser(item,new List<Image>());
            using (var dbContext = new ImzistEntities())
            {
                dbContext.Items.Add(item);
                dbContext.SaveChanges();
                Emailer.SendEmail(User.Identity.Name, "Imzist Listing",
                                  String.Format(
                                      "Thank you for listing a {0} with us!\nYour listing will expire on {1}.",
                                      item.Title, item.ExpirationDate));
                var newId = item.Id;
                return RedirectToAction("Index", new {location = LocationResolver.GetLocation().Name, id = newId});
            }
        }

        [HttpPost]
        public ActionResult SendEmail(int itemId, string replyToAddress, string message)
        {
            
            using (var dbContext = new ImzistEntities())
            {
                var item = dbContext.Items.First(i => i.Id == itemId);
                var email = Membership.GetUser(item.UserId).UserName;
                Emailer.SendEmail(email, string.Format("Regarding your {0} listing on Imzist.com", item.Title),
                                  message, replyToAddress);
            }

            return Json(new {status = true});
        }

        [Authorize]
        public ActionResult Edit(int itemId)
        {
            using (var dbContext = new ImzistEntities())
            {
                var model = new ItemListingViewModel();
                var item = dbContext.Items.Include(item1 => item1.Images).FirstOrDefault(i => i.Id == itemId);
                if (IsPoster(item.UserId))
                {
                    model.UserId = item.UserId;
                    model.Categories = dbContext.Categories.ToList();
                    model.Item = item;
                    return View(model);
                }

                return RedirectToAction("Index",  new { location = LocationResolver.GetLocation().Name, id = itemId });
            }
            
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Item item, IEnumerable<int> deletedImages, int expirationDays)
        {
                
                using (var dbContext = new ImzistEntities())
                {
                    var itemDb = dbContext.Items.First(i => i.Id == item.Id);
                    
                    if (IsPoster(itemDb.UserId))
                    {
                        
                        item = ImageProcesser(item, itemDb.Images);
                        
                        itemDb.ExpirationDate = itemDb.PostedDate.AddDays(expirationDays);
                        foreach (Image img in item.Images)
                        {
                            itemDb.Images.Add(img);
                        }
                        itemDb.Title = item.Title;
                        itemDb.Description = item.Description;
                        itemDb.CategoryId = item.CategoryId;

                        if (deletedImages != null)
                        {
                            foreach (int deletedImageId in deletedImages)
                            {
                                Image deleteImg = itemDb.Images.FirstOrDefault(image => image.Id == deletedImageId);
                                dbContext.Images.Remove(deleteImg);
                                itemDb.Images.Remove(deleteImg);
                            }
                        }

                        dbContext.SaveChanges();
                        Emailer.SendEmail(User.Identity.Name, "Imzist Listing Updated",
                                          String.Format(
                                              "Thank you for updating your {0} listing with us!\nYour listing will expire on {1}.",
                                              item.Title, item.ExpirationDate));
                    }
                }
                //@todo return message that item was updated....

            

            return RedirectToAction("Index", new { location = LocationResolver.GetLocation().Name, id = item.Id });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbContext = new ImzistEntities())
            {
                var item = dbContext.Items.First(i => i.Id == id);
                if (IsPoster(item.UserId))
                {
                    
                    foreach (Image img in item.Images)
                    {
                        dbContext.Images.Remove(img);
                    }
                    dbContext.Items.Remove(item);
                    dbContext.SaveChanges();
                }
            }
            
            return RedirectToAction("GetPosts", "Account");
        }
    }
}