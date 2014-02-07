using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Logic;
using Imzist.Web.Helpers;
using Imzist.Web.Models;

namespace Imzist.Web.Controllers
{
    public class ListingsController : Controller
    {
        private ImzistDb _imzistDb = new ImzistDb();

        public ActionResult Index()
        {
            ListingsViewModel lvm = new ListingsViewModel();
            lvm.Categories = _imzistDb.Categories();
            lvm.Items = _imzistDb.LocationBasedItems(HttpContext);
            return View(lvm);
        }

    }
}
