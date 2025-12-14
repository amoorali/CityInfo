namespace CityInfo.Infrastructure.Repositories.Contracts
{
    public interface ICityInfoRepository
    {
        Task<bool> SaveChangesAsync();
    }
}
