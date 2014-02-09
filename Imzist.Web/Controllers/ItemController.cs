using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;
using System.Data.Entity;

namespace Imzist.Web.Controllers
{
    public class ItemController : Controller
    {
        //
        // GET: /Item/
        [HttpGet]
        public ActionResult Index(int id)
        {
            using (var dbcontext = new ImzistEntities())
            {
                IEnumerable<Item> items = dbcontext.Items.Include(it=>it.Images);
                Item item = items.FirstOrDefault(i => i.Id == id);
                return View(item);
            }
            
            
        }

    }
}
