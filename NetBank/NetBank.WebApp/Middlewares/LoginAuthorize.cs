using Microsoft.AspNetCore.Mvc.Filters;
using NetBank.Core.Application.Enums;
using NetBank.Core.Application.Helpers;
using NetBank.WebApp.Controllers;

namespace NetBank.WebApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSessionHelper validateUserSessionHelper;

        public LoginAuthorize(ValidateUserSessionHelper validateUserSessionHelper)
        {
            this.validateUserSessionHelper = validateUserSessionHelper;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var usuario = validateUserSessionHelper.HasUser();

           if(usuario != null)
           {
                 var controller = (UserController)context.Controller;
                switch(usuario.Rol)
                {
                    case (int)Roles.Admin:
                        context.Result = controller.RedirectToAction("Index", "Admin");
                        break;
                    case (int)Roles.Client:
                        context.Result = controller.RedirectToAction("Index", "Home");
                        break;
                }  
           }
           else
           {
                await next();
           }
        }
    }
}
