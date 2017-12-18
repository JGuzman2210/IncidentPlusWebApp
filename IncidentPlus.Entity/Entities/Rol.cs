using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    [Table(name:"Roles")]
    public class Rol : BaseEntity
    {

        [Required,StringLength(30)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

    }
}
