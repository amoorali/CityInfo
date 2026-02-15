using Asp.Versioning;
using CityInfo.Application.DTOs.UserProfile;
using CityInfo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        #region [ Fields ]
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion

        #region [ Constructor ]
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        //#region [ Request Body ]
        //// we won't use this outside of this class, so we can scope it to this namespace
        //public class AuthenticationRequestBody
        //{
        //    public string? UserName { get; set; }
        //    public string? Password { get; set; }
        //}
        //#endregion

        #region [ Register ]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var existing = await _userManager.FindByNameAsync(request.UserName);

            if (existing != null)
                return Conflict("A user with that username already exists.");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new { e.Code, e.Description });
                
                return BadRequest(errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            var response = new AuthResponseDto(
                user.Id,
                user.UserName ?? "",
                user.FirstName,
                user.LastName,
                user.City
                );

            return CreatedAtAction(nameof(Register), new { id = user.Id }, response);
        }
        #endregion

        #region [ Login ]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.UserName, request.Password,
                isPersistent: true,
                lockoutOnFailure: true);

            if (!result.Succeeded)
                return Unauthorized();

            return NoContent();
        }
        #endregion

        #region [ Logout ]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }
        #endregion

        //#region [ Authentication ]
        //[HttpPost("authenticate")]
        //public ActionResult<string> Authenticate(
        //    AuthenticationRequestBody authenticationRequestBody)
        //{
        //    // Step 1: validate the username/password
        //    var user = ValidateUserCredentials(
        //        authenticationRequestBody.UserName,
        //        authenticationRequestBody.Password);

        //    if (user == null)
        //        return Unauthorized();

        //    // Step 2: create a token
        //    var securityKey = new SymmetricSecurityKey(
        //        Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
        //    var signingCredentials = new SigningCredentials(
        //        securityKey, SecurityAlgorithms.HmacSha256);

        //    var claimsForToken = new List<Claim>
        //    {
        //        new Claim("sub", user.UserId.ToString()),
        //        new Claim("given_name", user.UserName),
        //        new Claim("family_name", user.LastName),
        //        new Claim("city", user.City)
        //    };

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        _configuration["Authentication:Issuer"],
        //        _configuration["Authentication:Audience"],
        //        claimsForToken,
        //        DateTime.UtcNow,
        //        DateTime.UtcNow.AddHours(1),
        //        signingCredentials
        //        );

        //    var tokenToReturn = new JwtSecurityTokenHandler()
        //        .WriteToken(jwtSecurityToken);

        //    return Ok(tokenToReturn);
        //}

        //private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        //{
        //    // we don't have a user DB or table. If you have, check the passed-through
        //    // username/password against what's stored in the database.

        //    // For demo purposes, we assume the credentials are valid

        //    // return a new CityInfoUser (values would normally come from your user DB/table
        //    return new CityInfoUser(
        //        1,
        //        userName ?? "",
        //        "Amirali",
        //        "Fallahi",
        //        "Antwerp");
        //}
        //#endregion
    }
}
