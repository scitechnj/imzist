using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imzist.Data;

namespace Imzist.Web.Helpers
{
    public static class LocationResolver
    {
        public static Location GetLocation()
        {
            var location = HttpContext.Current.Session["location"].ToString();
            using (var db = new ImzistEntities())
            {
                return db.Locations.FirstOrDefault(l => l.Name == location);
            }
        }
    }
}