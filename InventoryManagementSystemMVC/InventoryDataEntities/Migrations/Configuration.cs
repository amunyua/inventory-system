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
            //create crud actions
            context.CrudActions.AddOrUpdate(a=>a.ActionCode, new CRUDAction{ ActionCode = "ADD", Description = "Can Add an entry"});
            context.CrudActions.AddOrUpdate(a=>a.ActionCode, new CRUDAction{ ActionCode = "EDIT", Description = "Can Edit an entry"});
            context.CrudActions.AddOrUpdate(a=>a.ActionCode, new CRUDAction{ ActionCode = "DELETE", Description = "Can Delete an entry"});
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

            AssignRole(context, roleId,analyticalDashboard.Id,dashboardId);

            //############################################### User Settings #############################################################
            //parent menu for system settings
            var userSettings = new Menu { Status = true, MenuName = "User Settings", Controller = "#", Action = "#", Icon = "fa fa-user text-yellow", Sequence = 8 };
            context.Menus.AddOrUpdate(
                m => m.MenuName, userSettings
                );
            var userSettingsId = userSettings.Id;
            //////////////// children menus
            var roles = new Menu { Status = true, MenuName = "Manage Roles", Controller = "Roles", Action = "Index", Icon = "#", Sequence = 1, ParentMenu = userSettingsId };

            context.Menus.AddOrUpdate(m => m.MenuName, roles);

            AssignRole(context, roleId, roles.Id, userSettingsId);


            //############################################### System Settings #############################################################
            //parent menu for system settings
            var systemSettings = new Menu { Status = true, MenuName = "System Settings", Controller = "#", Action = "#", Icon = "fa fa-gears text-yellow", Sequence = 9 };
            context.Menus.AddOrUpdate(
                m => m.MenuName, systemSettings
                );
            var systemSettingsId = systemSettings.Id;
            //////////////////// children menus for system settings
            var manageMenu = new Menu { Status = true, MenuName = "Manage Menu", Controller = "Menus", Action = "Index", Icon = "#", Sequence = 1, ParentMenu = systemSettingsId };
            
            context.Menus.AddOrUpdate(m => m.MenuName, manageMenu);

            AssignRole(context, roleId, manageMenu.Id, systemSettingsId);

        }

       
        
        
        
        
        
        
        
        
        
        public void AssignRole(InventoryDataEntities.Models.IMSDataEntities context,long roleId, long menuId, long parentId)
        {
            context.UserRoleAllocations.AddOrUpdate(
                x =>  x.MenuId, new UserRoleAllocation { MenuId = menuId, RoleId = roleId, ParentId = parentId}
                );
        }
    }
}
