using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace InventoryManagementSystemMVC.Controllers
{
    public  class FlagController : Controller
    {

        public FlagController()
        {
            Status();
        }


        public  bool Status()
        {
            var userId = User.Identity.GetUserId<int>();
            Debug.WriteLine("true");
            return true;
        }
    }
}