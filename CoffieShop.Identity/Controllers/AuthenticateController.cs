using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace CoffieShop.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        [HttpGet(Name = "Consume")]
        public async Task<IActionResult> Authenticate()
        {
            var client = new HttpClient();

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "https://localhost:5443/connect/token",
                ClientId = "coffie.Client",
                ClientSecret = "c6Zyz7oDyZyZ22q3U0l8SWHMzCMF",
                UserName = "M2ri",
                Password = "Pa$$w0rd",
                Scope = "role profile openid"
            });

            if (tokenResponse.IsError)
            {

                Console.WriteLine(tokenResponse.Error);
            }

            var response = tokenResponse.Json;
            //var accesstoken = (string)response["access_token"];
            return Ok(response);
        }
    }
}
