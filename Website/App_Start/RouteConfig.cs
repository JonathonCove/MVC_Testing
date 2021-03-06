﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Scarecrow
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //User
            routes.MapRoute(null, "signup", new { controller="User", action="Signup" });
            routes.MapRoute(null, "login", new { controller="User", action="Login" });
            routes.MapRoute(null, "verify-email/{hash}", new { controller="User", action= "VerfyEmail" });

            //routes.MapRoute(null, "user/{action}", new { controller="User" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
