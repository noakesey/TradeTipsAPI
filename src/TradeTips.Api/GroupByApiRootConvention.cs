using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace TradeTips.Api
{
    public class GroupByApiRootConvention : IControllerModelConvention
    {
        /// <summary>
        /// Define the group name used by Swagger on every controller (for Swashbuckle?)
        /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md
        /// </summary>
        /// <param name="controller"></param>
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.Attributes.OfType<RouteAttribute>().FirstOrDefault();
            var apiVersion = controllerNamespace?.Template?.Split('/')?.First()?.ToLower() ?? "default";

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}