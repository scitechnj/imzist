using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;
using Imzist.Logic;
using Imzist.Web.Helpers;
using Imzist.Web.Models;

namespace Imzist.Web.Controllers
{
    public class ListingsController : Controller
    {
        private ImzistDb _imzistDb = new ImzistDb();

        public ActionResult Index(string category = null)
        {
            ListingsViewModel lvm = new ListingsViewModel();
            lvm.Categories = _imzistDb.Categories();

            if (category == null)
            {
                lvm.Items = _imzistDb.LocationBasedItems(HttpContext);
            }
            else
            {
                lvm.Items = _imzistDb.CategoryBasedItems(category, HttpContext);
            }
            
            return View(lvm);
        }
    }
}
