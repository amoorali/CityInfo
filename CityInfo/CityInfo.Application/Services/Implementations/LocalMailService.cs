using CityInfo.Application.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace CityInfo.Application.Services.Implementations
{
    public class LocalMailService : IMailService
    {

        #region [ Fields ]
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;
        #endregion

        #region [ Constructure ]
        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }
        #endregion

        #region [ Methods ]
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
        #endregion
    }
}
