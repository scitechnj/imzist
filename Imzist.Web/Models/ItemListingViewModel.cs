using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Imzist.Data;

namespace Imzist.Web.Models
{
    public class ItemListingViewModel
    {
        public Guid UserId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SelectListItem> ExpirationOptions { get; set; }
        public Item Item { get; set; }
    }
}