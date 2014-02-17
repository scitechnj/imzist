using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imzist.Data;

namespace Imzist.Web.Models
{
    public class EditListingViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SelectListItem> ExpirationOptions { get; set; }
    }
}