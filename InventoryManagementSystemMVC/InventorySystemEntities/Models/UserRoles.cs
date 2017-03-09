using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventorySystemEntities.Models
{
    public class Roles
    {
        [Key]
        public long RoleId {get; set;}
        [Required]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }
    }
}