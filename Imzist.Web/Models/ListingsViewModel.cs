﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imzist.Data;

namespace Imzist.Web.Models
{
    public class ListingsViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Dictionary<DateTime, Item> Items { get; set; } 
    }
}