namespace CityInfo.Application.Services.Contracts
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string? fields);
    }
}