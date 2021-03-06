﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imzist.Data;

namespace Imzist.Web.Helpers
{
    public static class LocationResolver
    {
        private static IEnumerable<Location> _locations;
        public static Location GetLocation()
        {
            if(_locations == null)
            {
                using (var db = new ImzistEntities())
                {
                    _locations = db.Locations.ToList();
                }
            }
            var location = HttpContext.Current.Session["location"].ToString();
            return _locations.FirstOrDefault(l => l.Name.ToLowerInvariant() == location.ToLowerInvariant());
        }
    }
}
