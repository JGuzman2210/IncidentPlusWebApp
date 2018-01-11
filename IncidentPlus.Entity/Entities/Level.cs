using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    public class Level : BaseEntity
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int ProjectID { get; set; }

        [Column("DefaultLevel")]
        public bool IsDefault { get; set; }

        #region RelationShip
        [ForeignKey("ProjectID")]
        public Project Project { get; set; }
        #endregion
    }
}
