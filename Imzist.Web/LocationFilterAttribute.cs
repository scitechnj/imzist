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
            if (filterContext.RouteData.Values["location"] != null)
            {
                var location = filterContext.RouteData.Values["location"].ToString();
                filterContext.HttpContext.Session["location"] = location;
                base.OnActionExecuting(filterContext);   
            }
        }
    }
}