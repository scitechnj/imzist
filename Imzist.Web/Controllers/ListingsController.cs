﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Web.Helpers;

namespace Imzist.Web.Controllers
{
    public class ListingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}