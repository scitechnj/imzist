using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;
using System.Data.Entity;
using Imzist.Web.Models;

namespace Imzist.Web.Controllers
{
    public class ItemController : Controller
    {
        [HttpGet]
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
    }
}
