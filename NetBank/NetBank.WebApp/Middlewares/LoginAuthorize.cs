using Microsoft.AspNetCore.Mvc.Filters;
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
           if(validateUserSessionHelper.HasUser())
            {
                var controller = (UserController)context.Controller;
                context.Result = controller.RedirectToAction("Index","Home");
            }
           else
            {
                await next();
            }
        }
    }
}
