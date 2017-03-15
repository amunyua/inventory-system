using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventorySystemEntities.Models
{
    public class IMSDataEntities : DbContext
    {
        public DbSet<Roles> Roles { get; set; }
    }
}