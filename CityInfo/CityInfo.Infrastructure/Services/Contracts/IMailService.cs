namespace CityInfo.Infrastructure.Services.Contracts
{
    public interface IMailService
    {
        public void Send(string subject, string message);
    }
}
