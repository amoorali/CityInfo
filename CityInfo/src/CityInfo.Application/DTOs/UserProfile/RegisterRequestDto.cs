namespace CityInfo.Application.DTOs.UserProfile
{
    public record RegisterRequestDto(
        string UserName,
        string Password,
        string FirstName,
        string LastName,
        string City
        );
}
