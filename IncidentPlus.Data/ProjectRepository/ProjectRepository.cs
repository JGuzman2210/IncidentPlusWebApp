using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace IncidentPlus.Data.ProjectRepository
{
    public class ProjectRepository : GenericRepository<Project>
    {
        private ProjectRepository(){}


        public static ProjectRepository NewInstance()
        {
            return new ProjectRepository();
        }

        public List<Category> GetCategoriesByProjectID(int id)
        {
            using(var db = new ContextDB.IncidencPlusDBContext())
            {
                var categories = db.Categories.Where(c => c.ProjectID == id).ToList();
                return categories;
            }
        }

        public List<Level> GetLevelsByProjectID(int id)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                var levels = db.Levels.Where(c => c.ProjectID == id).ToList();
                return levels;
            }
        }
    }
}
