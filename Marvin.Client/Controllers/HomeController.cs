using Marvin.Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;

namespace Marvin.Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Dictionary<string, string> _homes = new Dictionary<string, string>();
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
    
        public async Task<IActionResult> Index()
        {
            var result = await LogIdentityInformations();
            ViewBag.Claims = result;
            return View();
        }

        [Authorize(Policy = "AbleToFetch")]
        public async Task<IActionResult> Privacy()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/weatherForecast");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var weathers = await System.Text.Json.JsonSerializer.DeserializeAsync<List<WeatherForecastDto>>(responseStream);
                return View(weathers);
            }
        }

        public async Task<Dictionary<string, string>> LogIdentityInformations()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var userClaimSB = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                _homes.Add(claim.Type, claim.Value);
                userClaimSB.AppendLine(
                  $"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }

            _logger.LogInformation($"Identity token & user claims: " +
               $"\n{identityToken} \n{userClaimSB}");

            _logger.LogInformation($"Access Token: {accessToken}");

            _logger.LogInformation($"Identity Token: {identityToken}");

            _logger.LogInformation($"Refresh Token: {refreshToken}");

            return _homes;
        }
    }
}
