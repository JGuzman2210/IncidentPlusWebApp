using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Data.RolRepository
{
    public class RolRepository : GenericRepository<Rol>
    {
        private RolRepository(){}

        public static RolRepository NewInstance()
        {
            return new RolRepository();
        }

    }
}
