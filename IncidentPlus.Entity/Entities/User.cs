using IncidentPlus.Entity.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    [Table(name: "Users")]
    public class User : BaseEntity
    {
        #region Properties

        [Required,StringLength(60)]
        public string Name { get; set; }

        [Required, StringLength(60),DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required, StringLength(60),Index(IsUnique =true)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
        #endregion

        #region RelationShip
        public int RolID { get; set; }

        [ForeignKey("RolID")]
        public Rol Rol { get; set; }
        #endregion

    }
}
