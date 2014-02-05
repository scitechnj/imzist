using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Imzist.Web
{
    public class LocationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var location = filterContext.RouteData.Values["location"].ToString();
            if (filterContext.HttpContext.Session != null) filterContext.HttpContext.Session["locaton"] = location;
        }
    }
}