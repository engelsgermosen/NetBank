using NetBank.Core.Application.Dtos.Email;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
