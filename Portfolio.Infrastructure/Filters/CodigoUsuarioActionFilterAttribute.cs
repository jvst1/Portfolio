using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Infrastructure.Filters
{
    public class CodigoUsuarioActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Guid? usuario = null;
            string clientId = null;
            var contextController = (ControllerBase)context.Controller;

            if (!contextController.ControllerContext.ActionDescriptor.EndpointMetadata.Contains(new AllowAnonymousAttribute()))
            {
                var userId = contextController.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    clientId = contextController.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                }
                else
                {
                    usuario = Guid.Parse(userId);
                }
            }

            var helper = context.HttpContext.RequestServices.GetService<CodigoUsuarioHelper>();
            helper.CodigoUsuario = usuario;
            helper.ClientId = clientId;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}