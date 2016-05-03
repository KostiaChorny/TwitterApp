using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TwitterApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "TweetsDelete",
               url: "Tweets/Delete/{id}",
               defaults: new { controller = "Tweets", action = "delete" }
           );

            routes.MapRoute(
                name: "TweetsPull",
                url: "Tweets/Pull/{username}",
                defaults: new { controller = "Tweets", action = "Pull" }
            );

            routes.MapRoute(
                name: "TweetsByUsername",
                url: "Tweets/{username}/{page}",
                defaults: new { controller = "Tweets", action = "Index", page=UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
