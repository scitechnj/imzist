using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imzist.Data;
using System.Data.Entity;

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
        public IEnumerable<Item> LocationBasedItems(int locationId)
        {
            using (var db = new ImzistEntities())
            {
                //                var items = db.Items.Where(i => i.Location.Name == location).ToList();
                var items = db.Items.Include(i => i.Images).Include(i => i.Location);

                return items.Where(i => i.Location.Id == locationId).OrderByDescending(i => i.PostedDate).ToList();
            }
        }
        public IEnumerable<Item> CategoryBasedItems(string categoryName, int locationId)
        {
            using (var db = new ImzistEntities())
            {
                var items = db.Items.Include(i => i.Images).Include(i => i.Location);
                return items.Where(i => i.Category.Name == categoryName && i.Location.Id == locationId).OrderByDescending(i => i.PostedDate).ToList();
            }
        }
        public IEnumerable<Category> Categories()
        {
            using (var db = new ImzistEntities())
            {
                return db.Categories.OrderBy(c => c.Name).ToList();
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
