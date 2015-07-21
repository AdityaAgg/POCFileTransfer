using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace POCFileTransfer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "UploadApi",
                routeTemplate: "api/file/upload",
                defaults:  new { controller = "FileUploadAPI", action="Upload", id = RouteParameter.Optional}
            );
            config.Routes.MapHttpRoute(
                name: "DownloadApi",
                routeTemplate: "api/tree/download",
                defaults: new { controller = "TreeDownloadAPI", action="PostFileTree", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
