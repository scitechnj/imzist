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
        //
        // GET: /Item/
        public ActionResult Index(int id)
        {
            using (var dbcontext = new ImzistEntities())
            {
                IEnumerable<Item> items = dbcontext.Items.Include(it => it.Images);
                Item item = items.FirstOrDefault(i => i.Id == id);
                return View(new ItemViewModel() {Item = item});
            }
        }

        public ActionResult Add()
        {
            var model = new AddListingViewModel();
            model.UserId = Guid.NewGuid(); //(Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey

            using (var dbContext = new ImzistEntities())
            {
                model.Categories = dbContext.Categories.ToList();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Item item, int expirationDays)
        {
            item.Location = LocationResolver.GetLocation();
            item.UserId = Guid.Parse("88923831-E379-4FE3-8EDD-2342CF7727B5");
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
                                      "Thank you for listing and item with us!\nYour listing will expire on {0}.",
                                      item.ExpirationDate));
                var newId = item.Id;
                return RedirectToAction("Index", new {location = LocationResolver.GetLocation().Name, id = newId});
            }
        }
    }
}