﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;

namespace AwareswebApp
{
    public class kmlRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string filename = requestContext.RouteData.Values["filename"] as string;

            if (string.IsNullOrEmpty(filename))
            {
                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.StatusCode = 404;
                requestContext.HttpContext.Response.End();
            }
            else
            {
                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.ContentType = GetContentType(requestContext.HttpContext.Request.Url.ToString());

                // find physical path to image here.  
                string filepath = requestContext.HttpContext.Server.MapPath("SE_LaJulia.kml");

                requestContext.HttpContext.Response.WriteFile(filepath);
                requestContext.HttpContext.Response.End();
            }
            return null;
        }

        private static string GetContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".kml": return "kml";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }
    
    }
}

