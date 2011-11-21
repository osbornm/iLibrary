using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iLibrary {

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //**********************************
            // Tv Show Routes 
            //**********************************
            routes.MapRoute(
                "TvShowsSeason",
                "TvShows",
                new { controller = "TvShows", action = "Index" }
            );

            routes.MapRoute(
               "TvShowsByShow",
               "TvShows/{ShowName}",
               new { controller = "TvShows", action = "Show" }
            );

            routes.MapRoute(
                "TvShowsBySeason",
                "TvShows/{ShowName}/{Season}",
                new { controller = "TvShows", action = "Season" }
            );

            routes.MapRoute(
                "TvShowsByEpisode",
                "TvShows/{ShowName}/{Season}/{Episode}",
                new { controller = "TvShows", action = "Episode" }
            );

            //**********************************
            // Movie Routes 
            //**********************************
            routes.MapRoute(
                "MoviesList",
                "Movies",
                new { controller = "Movies", action = "Index" }
            );

            routes.MapRoute(
                "Movie",
                "Movies/{name}",
                new { controller = "Movies", action = "Movie" }
            );

            routes.MapRoute(
               "refresh",
               "refresh",
               new { controller = "Home", action = "RefreshAllData" }
           );

            //**********************************
            // default Routes 
            //**********************************
            routes.MapRoute(
                "default",
                "{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            ValueProviderFactories.Factories.Add(new Microsoft.Web.Mvc.CookieValueProviderFactory());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}