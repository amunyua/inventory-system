using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataEntities.Models
{
   public class UserRoles
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Roles")]
        public long RoleId { get; set; }

        public long UserId { get; set; }
        [Required]
        public virtual Roles Roles { get; set; }
    }
}
