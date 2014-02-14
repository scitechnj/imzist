using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Imzist.Web.Controllers
{
    public class ItemController : Controller
    {
        public ActionResult Index(int id)
        {
            using (var dbcontext = new ImzistEntities())
            {
                IEnumerable<Item> items = dbcontext.Items.Include(it => it.Images);
                Item item = items.FirstOrDefault(i => i.Id == id);
                bool IsPoster;
                if(Membership.GetUser(User.Identity.Name) == null)
                {
                    //not logged in so obv not the poster
                    IsPoster = false;
                }
                
                else
                {
                    //check if the current logged in user is the poster
                    IsPoster = item.UserId == (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                }
                return View(new ItemViewModel() {Item = item, IsPoster = IsPoster});
            }
        }

        [Authorize]
        public ActionResult Add()
        {
            var model = new AddListingViewModel();
            model.UserId = (Guid) Membership.GetUser(User.Identity.Name).ProviderUserKey;

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
            item.UserId = item.UserId;
            item.PostedDate = DateTime.Now;
            item.ExpirationDate = item.PostedDate.AddDays(expirationDays);
            foreach (string file in Request.Files)
            {
                var imageFile = Request.Files[file];
                if (imageFile == null)
                {
                    continue;
                }

                var imageBytes = new byte[imageFile.InputStream.Length];
                imageFile.InputStream.Read(imageBytes, 0, imageBytes.Length);
                var imageStream = new MemoryStream(imageBytes);

                if (ImageHelper.IsValidImage(imageStream))
                {
                    string newFileName = ImageHelper.RenameImageFile(imageFile.FileName);
                    Image img = new Image {Name = newFileName};
                    imageFile.SaveAs(Path.Combine(Server.MapPath("~/content/images/Full"), newFileName));
                    ImageHelper.ThumbnailMaker(imageStream,
                                               Path.Combine(Server.MapPath("~/Content/Images/Thumbnail"), newFileName),
                                               50);
                    item.Images.Add(img);
                }

                imageFile.InputStream.Dispose();
                imageStream.Dispose();
            }


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
    }
}