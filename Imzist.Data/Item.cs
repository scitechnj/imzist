//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Imzist.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public Item()
        {
            this.Images = new HashSet<Image>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Flagged { get; set; }
        public System.DateTime PostedDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public System.Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual Location Location { get; set; }
    }
}
