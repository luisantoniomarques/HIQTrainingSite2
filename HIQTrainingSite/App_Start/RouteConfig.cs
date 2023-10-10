using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HIQTrainingSite {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "CreateCertification",
            //    url: "Certification/CreateFullCertification/{id}",
            //    defaults: new { controller = "Certification", action = "CreateFullCertification", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "CreateCourse",
            //    url: "Course/Create/{teacherId}",
            //    defaults: new { controller = "Course", action = "Create", teacherId = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "InscriptionDefault",
            //    url: "Inscription/{action}/{courseId}",
            //    defaults: new { controller = "Inscription", action = "Index", courseId = UrlParameter.Optional }
            //);

            if (HttpContext.Current.User == null)
            {
                routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new
                  {
                      controller = "Account",
                      action = "Login",
                      id = UrlParameter.Optional
                  }
                );
            }
            else
            {
                routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new
                  {
                      controller = "Home",
                      action = "Index2",
                      id = UrlParameter.Optional
                  }
                );
            }


        }
    }
}
