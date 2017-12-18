using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Data.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        private CategoryRepository() { }

        public static CategoryRepository NewInstance()
        {
            return new CategoryRepository();
        }
    }
}
