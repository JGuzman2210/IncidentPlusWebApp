using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    public class Category : BaseEntity
    {
        [Required,StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public Project Project { get; set; }
    }
}
