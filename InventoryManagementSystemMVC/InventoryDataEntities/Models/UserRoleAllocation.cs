using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataEntities.Models
{
    class UserRoleAllocation
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Role_id")]
        public long RoleId { get; set; }
        [ForeignKey("Menus")]
        public long MenuId { get; set; }
        [Required]
        public virtual Menu Menus { get; set; }
        [Required]
        public virtual Roles Role_id { get; set; }
    }
}
