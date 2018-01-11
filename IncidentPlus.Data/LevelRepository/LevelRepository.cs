using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Data.LevelRepository
{
    public class LevelRepository : GenericRepository<Level>
    {
        private LevelRepository() { }

        public static LevelRepository NewInstance()
        {
            return new LevelRepository();
        }

        public bool ExitDefaultLevel(int projectId)
        {
            using(var db = new ContextDB.IncidencPlusDBContext())
            {
                return db.Levels.Any(_ => _.ProjectID == projectId && _.IsDefault == true);
            }
        }
    }
}
