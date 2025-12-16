namespace CityInfo.Application.Services.Contracts
{
    public interface IMailService
    {
        public void Send(string subject, string message);
    }
}
