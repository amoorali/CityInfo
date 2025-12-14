namespace CityInfo.Infrastructure.Repositories.Contracts
{
    public interface ICityRepository
    {
        Task<bool> SaveChangesAsync();
    }
}
