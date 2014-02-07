using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imzist.Data;

namespace Imzist.Logic
{
    public class ImzistDb
    {
        public IEnumerable<Item> Flagged()
        {
            using (var db = new ImzistEntities())
            {
                return db.Items.Where(i => i.Flagged == true);
            }
        } 

        public IEnumerable<Item> UserItems(Guid id)
        {
            using (var db = new ImzistEntities())
            {
                return db.Items.Where(i => i.UserId == id);
            }
        }
 
        public IEnumerable<Item> LocationBasedItems(HttpContextBase context)
        {
            using (var db = new ImzistEntities())
            {
                string location = context.Session["location"].ToString();
//                var items = db.Items.Where(i => i.Location.Name == location).ToList();
                return db.Images.Select(i => i.Item).Distinct().Where(i => i.Location.Name == location).ToList();
            }
        }

        public IEnumerable<Category> Categories()
        {
            using (var db = new ImzistEntities())
            {
                return db.Categories.ToList();
            }
        } 
        public Category Category(int id)
        {
            using (var db = new ImzistEntities())
            {
                return db.Categories.FirstOrDefault(c => c.Id == id);
            }
        }

        public IEnumerable<Location> Locations()
        {
            using (var db = new ImzistEntities())
            {
                return db.Locations;
            }
        } 
        public Location Location(int id)
        {
            using (var db = new ImzistEntities())
            {
                return db.Locations.FirstOrDefault(l => l.Id == id);
            }
        }
    }
}
