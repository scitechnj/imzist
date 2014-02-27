using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imzist.Data;

namespace Imzist.Web.Models
{
    public class ItemViewModel
    {
        public Item Item { get; set; }
        public bool IsPoster { get; set; }
    }

}