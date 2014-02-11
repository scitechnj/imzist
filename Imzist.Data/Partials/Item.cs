using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imzist.Data
{
    public partial class Item
    {
        public bool IsExpired
        {
            get { return this.ExpirationDate < DateTime.Now.Date; }
        }
    }
}
