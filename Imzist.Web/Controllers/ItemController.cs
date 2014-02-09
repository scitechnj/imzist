using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;
using System.Data.Entity;
using Imzist.Web.Models;
using System.Web.Security;
using Imzist.Data;
using Imzist.Web.Helpers;
using Imzist.Web.Models;

namespace Imzist.Web.Controllers
{
    public class ItemController : Controller
    {
        public ActionResult Index(int id)
        {
            ItemViewModel ivm = new ItemViewModel();
            using (var dbcontext = new ImzistEntities())
            {
                IEnumerable<Item> items = dbcontext.Items.Include(it=>it.Images);
                ivm.Item = items.FirstOrDefault(i => i.Id == id);
                return View(ivm);
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
            item.PostedDate = DateTime.Now;
            item.ExpirationDate = item.PostedDate.AddDays(expirationDays);
            foreach (HttpPostedFileBase imageFile in Request.Files)
            {
                Image img = new Image();
                //thumbnail maker
                //saving to disk
                //renaming to guid with .extension
                //any validation

//                img.Name = imageFile.FileName
                item.Images.Add(img);
            }
            
           
            using (var dbContext = new ImzistEntities())
            {
                dbContext.Items.Add(item);
                dbContext.SaveChanges();
            }
            return Json(item);
        }

    }
}
