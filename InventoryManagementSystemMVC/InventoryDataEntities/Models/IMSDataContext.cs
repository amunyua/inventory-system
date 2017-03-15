using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataEntities.Models
{
    class IMSDataEntities : DbContext
    {
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserRoleAllocation> UserRoleAllocations { get; set; }
        public DbSet<User> Users { get; set; }

    }

   
}
