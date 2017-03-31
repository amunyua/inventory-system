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
//                context.UserRoles.AddOrUpdate(
//                    x => x.UserId, new UserRoles { UserId = admin.Id, RoleId = roleId}
//                    );
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

            AssignRole(roleId,analyticalDashboard.Id,dashboardId);

           
            
            
            
            //############################################### User Settings #############################################################
            //parent menu for user settings
            var userSettings = new Menu { Status = true, MenuName = "User Settings", Controller = "#", Action = "#", Icon = "fa fa-user text-yellow", Sequence = 8 };
            context.Menus.AddOrUpdate(
                m => m.MenuName, userSettings
                );
            var userSettingsId = userSettings.Id;
            //////////////// children menus
            var roles = new Menu { Status = true, MenuName = "Manage Roles", Controller = "Roles", Action = "Index", Icon = "#", Sequence = 1, ParentMenu = userSettingsId };

            context.Menus.AddOrUpdate(m => m.MenuName, roles);

            AssignRole(roleId, roles.Id, userSettingsId);
            //manage users
            var manageUsers = new Menu { Status = true, MenuName = "Manage Users", Controller = "Manage", Action = "Index", Icon = "#", Sequence = 2, ParentMenu = userSettingsId };
            context.Menus.AddOrUpdate(m => m.MenuName, manageUsers);
            AssignRole( roleId, manageUsers.Id, userSettingsId);


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

            AssignRole(roleId, manageMenu.Id, systemSettingsId);
            //            ChildMenu(context, "General Settings", "GeneralSettings", "Index", 2, systemSettingsId, roleId);



            //##################### Inventory Module###############
            //parent
            var suppliersParent = ParentMenu(context,"Suppliers", 2);

            //manage suppliers
            ChildMenu(context, "Manage Suppliers", "Suppliers", "Index", 2, suppliersParent, roleId);

            //################## Proucts module

            var productsParent = ParentMenu(context, "Products", 3);
            //children
            ChildMenu(context, "Manage Uom", "UOM", "Index", 2, productsParent, roleId);
            //manage products categories
            ChildMenu(context, "Manage Product Categories", "ProductCategories", "Index", 1, productsParent, roleId);
            ChildMenu(context, "Manage Products", "Products", "Index", 1, productsParent, roleId);

            //################## Warehouses
            var warehouseParent = ParentMenu(context, "Warehouses", 4);
            //warehouse types
            ChildMenu(context, "Manage Warehouse Types", "WarehouseTypes", "Index", 1, warehouseParent, roleId);
            ChildMenu(context, "Manage Warehouses", "Warehouses", "Index", 2, warehouseParent, roleId);

            //parent menu for sock management
            var stockParent = ParentMenu(context, "Stock Management", 5);
            ChildMenu(context, "Manage Stock", "Stock", "Index", 1, stockParent, roleId);
            ChildMenu(context, "Stock Transaction", "Stock", "Create", 2, stockParent, roleId);
            ChildMenu(context, "Transactions", "Stock", "Transactions", 3, stockParent, roleId);


        }



        public long ParentMenu(InventoryDataEntities.Models.IMSDataEntities context,string menuName, int sequence)
        {
//            var context = new IMSDataEntities();
            var parentMenu = new Menu { Status = true, MenuName = menuName, Controller = "#", Action = "#", Icon = "fa fa-gears text-yellow", Sequence = sequence };
            context.Menus.AddOrUpdate(
                m => m.MenuName, parentMenu
                );
            return parentMenu.Id;
        }

        public void ChildMenu(InventoryDataEntities.Models.IMSDataEntities context, string childMenuName, string controller, string action, int sequence, long parentId, long roleId)
        {
//            var context = new IMSDataEntities();
            var childMenu = new Menu { Status = true, MenuName = childMenuName, Controller = controller, Action = action, Icon = "#", Sequence = sequence, ParentMenu = parentId };

            context.Menus.AddOrUpdate(m => m.MenuName, childMenu);

            AssignRole(roleId, childMenu.Id, parentId);
        }
        
        
        
        
        
        
        
        public void AssignRole(long roleId, long menuId, long parentId)
        {
            var context = new IMSDataEntities();

            var count = context.UserRoleAllocations.Where(x => x.RoleId == roleId && x.MenuId == menuId);
            if (!count.Any())
            {
                context.UserRoleAllocations.Add(
                    
                    new UserRoleAllocation
                    {
                        MenuId = menuId,
                        RoleId = roleId,
                        ParentId = parentId,
                        CrudActions = "{1,2,3}"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
