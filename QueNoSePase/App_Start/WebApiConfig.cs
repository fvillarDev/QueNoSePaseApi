﻿using System.Web.Http;

namespace QueNoSePase.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.EnableCors();

            log4net.Config.XmlConfigurator.Configure();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
