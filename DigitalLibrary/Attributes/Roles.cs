using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DigitalLibrary.Attributes
{
    
        public class Role : ActionFilterAttribute
        {
            public List<string> type { get; set; }
            public Role(string type)
            {
                this.type = new List<string>();
                this.type.Add(type);
            }
            public Role(string[] type)
            {
                this.type = type.ToList();
            }
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var controller = (Controller)filterContext.Controller;
                Users user = controller.Session["User"] as Users;
                if (user == null || !this.type.Contains(user.Role))
                {
                    //if (!this.type.Contains(user.Role))
                    //{
                        controller.Session.Clear();

                    var route = new RouteValueDictionary {
                        { "action", "Login" },
                        { "controller", "Account"  }
                    };
                    filterContext.Result = new RedirectToRouteResult(route);
                        //controller.RedirectToAction("Loing", "Account");
                    //}
                }
            }
        }
    
}