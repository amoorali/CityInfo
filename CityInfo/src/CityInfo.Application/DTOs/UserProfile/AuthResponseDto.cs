namespace CityInfo.Application.DTOs.UserProfile
{
    public record AuthResponseDto(
        string UserName,
        string Password,
        string FirstName,
        string LastName,
        string City
        );
}
