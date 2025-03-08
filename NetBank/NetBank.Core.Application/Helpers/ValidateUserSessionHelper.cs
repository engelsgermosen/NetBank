using Microsoft.AspNetCore.Http;
using NetBank.Core.Application.Dtos.Account;

namespace NetBank.Core.Application.Helpers
{
    public class ValidateUserSessionHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ValidateUserSessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor; 
        }
        public bool HasUser()
        {
            var user = httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            return user != null;
        }
    }
}
