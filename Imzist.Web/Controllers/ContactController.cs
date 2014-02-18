using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Logic;

namespace Imzist.Web.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Email(string message)
        {
            Emailer.SendEmail("freeImzist@gmail.com", "Contact from " + User.Identity.Name, "From " + User.Identity.Name + ":\n\n" + message);

            return Json(new {status = true});
        }
    }
}
