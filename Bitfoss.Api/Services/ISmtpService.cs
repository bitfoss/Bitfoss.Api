using System.Threading.Tasks;
using Bitfoss.Api.Models;

namespace Bitfoss.Api.Services
{
    public interface ISmtpService
    {
        Task SendEmailAsync(Email email);
    }
}
