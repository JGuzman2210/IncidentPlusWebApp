using IncidentPlus.Entity.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Entity.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Update { get; set; }

        [Required]
        public StateEntity State { get; set; }
    }
}
