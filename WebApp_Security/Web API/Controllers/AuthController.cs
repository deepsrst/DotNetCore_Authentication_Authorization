using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            //verify the credential
            if (credential.UserName == "admin" && credential.Password == "admin")
            {
                // Creating the security context
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email,"admin@myweb.com"),
                new Claim("Department","HR"),
                new Claim("Admin","true"),
                new Claim("Manager","true"),
                new Claim("EmploymentDate","2023-05-01")

            };

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt

                });
            }


            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint.");
            return Unauthorized(ModelState);
        }


        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey") ?? "");
            // generate the JWT
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                ));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


    }

    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
