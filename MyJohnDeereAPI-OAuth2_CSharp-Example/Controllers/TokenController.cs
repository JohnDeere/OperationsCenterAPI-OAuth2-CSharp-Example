using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Models;
using Newtonsoft.Json;

namespace MyJohnDeereAPI_OAuth2_CSharp_Example.Controllers
{
    public class TokenController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TokenController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "OurServer")]
        public async Task<IActionResult> Index([FromServices] IConfiguration configuration)
        {
            var settings = new JDeere();
            settings.Token = await HttpContext.GetTokenAsync("access_token");

            ViewBag.ClientId = configuration["JDeere:ClientId"];
            ViewBag.ClientSecret = configuration["JDeere:ClientSecret"];
            ViewBag.AuthorizationEndpoint = configuration["JDeere:AuthorizationEndpoint"];
            ViewBag.TokenEndpoint = configuration["JDeere:TokenEndpoint"];
            ViewBag.APIEndPoint = configuration["JDeere:APIEndPoint"];

            var serverResponse = await AccessTokenRefreshWrapper(
                () => SecuredGetRequest("http://localhost:9090/token/index"));

            var apiResponse = await AccessTokenRefreshWrapper(
                () => SecuredGetRequest("https://sandboxapi.deere.com:443/platform/"));
            string apidata = String.Format("https://sandboxapi.deere.com:443/platform/");
 
            var token = await HttpContext.GetTokenAsync("access_token");
            var Url = "https://sandboxapi.deere.com:443/platform/";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(Url);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.SerializeObject(stream, Formatting.Indented);
            var json2 = new JsonResult(json) { SerializerSettings = new JsonSerializerSettings() { Formatting = Formatting.Indented } };
            settings.APIPath = json2.Value.ToString();

            return View(settings);
        }

        private async Task<HttpResponseMessage> SecuredGetRequest(string url)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> AccessTokenRefreshWrapper(
            Func<Task<HttpResponseMessage>> initialRequest)
        {
            var response = await initialRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                response = await initialRequest();
            }
            return response;
        }

        private async Task RefreshAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var refreshTokenClient = _httpClientFactory.CreateClient();

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:9090/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCredentials = "username:password";
            var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64Credentials = Convert.ToBase64String(encodedCredentials);

            request.Headers.Add("Authorization", $"Basic {base64Credentials}");

            var response = await refreshTokenClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            var newAccessToken = responseData.GetValueOrDefault("access_token");
            var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);
        }
    }
}
