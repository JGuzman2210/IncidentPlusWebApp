using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    public class Project : BaseEntity
    {
        [Required,StringLength(50),Index(IsUnique =true)]
        public String Name { get; set; }

        [StringLength(100)]
        public String Description { get; set; }

    }
}
