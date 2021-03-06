﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AwareswebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



           // routes.MapRoute(
           //    name: "ObtenerRutas",
           //    url: "{controller}/{action}/{usrCor}",
           //     defaults: new { controller = "Rutas", action = "getRoutesPerUser" }
           //);

            routes.MapRoute(
                name: "RecepcionConsumo",
                url: "{controller}/{action}/{userNameColaborador}/{lecturaConsumo}/{tipoConsumo}/{Fecha}",
                 defaults: new { controller = "Consumos", action = "Rec" }
            );
            routes.MapRoute(
                name: "CreacionColab",
                url: "{controller}/{action}/{usuario}/{email}/{password}"
            );
            // Eliminar ruta
            //routes.MapRoute(
            //    name: "CreacionColabUsuario",
            //    url: "{controller}/{action}/{usuario}/{email}/{password}",
            //      defaults: new { controller = "Account", action = "RegisterCustom" }
            //);
            routes.MapRoute(
                name: "Creacionreportes",
                url: "{controller}/{action}/{numReporteUsr}/{userName}/{situacion}/{longitud}/{latitud}/{sector}"
      );
            routes.MapRoute(
                 name: "ValidacionColaborador",
                 url: "{controller}/{action}/{username}/{password}",
                 defaults: new { controller = "Colaboradores", action = "validate" }
       );   
        }
    }
}
