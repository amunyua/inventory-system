using System.Diagnostics;
using InventoryDataEntities.Models;
using Microsoft.AspNet.Identity;


namespace InventoryDataEntities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InventoryDataEntities.Models.IMSDataEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InventoryDataEntities.Models.IMSDataEntities context)
        {
            //create the admins user role
            context.Roles.AddOrUpdate(
                r => r.RoleName, new Models.Roles { RoleName = "SystemAdmin", Description = "Super admin"}
                );

            //get the id of the system admin role
            var roleId = context.Roles.FirstOrDefault(r => r.RoleName == "SystemAdmin").RoleId;

            //get the default user and give them system admins role
            var admin = context.Users.FirstOrDefault();
            if (admin != null)
            {
                context.UserRoles.AddOrUpdate(
                    x => x.UserId, new UserRoles { UserId = admin.Id, RoleId = roleId}
                    );
            }
            
            

            //parent menu for dashboard
            var dashboard = new Menu { Status = true, MenuName = "Dashboard", Controller = "#", Action = "#", Icon = "fa fa-dashboard text-yellow", Sequence = 1 };

            context.Menus.AddOrUpdate(
                m => m.MenuName, dashboard
                );
            var dashboardId = dashboard.Id;
            //analytical dashboard
            var analyticalDashboard = new Menu
            {
                Status = true,
                MenuName = "Analitical Dashboard",
                ParentMenu = dashboardId,
                Controller = "Dashboard",
                Action = "Index",
                Icon = "",
                Sequence = 1
            };
            context.Menus.AddOrUpdate(m => m.MenuName,analyticalDashboard);

            AssignRole(context, roleId,analyticalDashboard.Id);
        }

        public void AssignRole(InventoryDataEntities.Models.IMSDataEntities context,long roleId, long menuId)
        {
            context.UserRoleAllocations.AddOrUpdate(
                x => new { x.RoleId, x.MenuId }, new UserRoleAllocation { MenuId = menuId, RoleId = roleId }
                );
        }
    }
}
