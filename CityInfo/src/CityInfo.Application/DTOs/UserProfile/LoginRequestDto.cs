namespace CityInfo.Application.DTOs.UserProfile
{
    public record LoginRequestDto(
        string UserName,
        string Password
        );
}
