using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Interfaces;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace MyJohnDeereAPI_OAuth2_CSharp_NETCore_Example.Controllers
{
    public class APIController : Controller
    {
        protected JDeere MySettings { get; set; }

        [HttpPost]
        public async Task<IActionResult> EditAPIDetailsAsync([FromServices]IConfiguration configuration, string ClientId, string ClientSecret, string AuthorizationEndpoint, string TokenEndpoint, string APIEndPoint)
        {
            JDeere obj = new JDeere
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                AuthorizationEndpoint = AuthorizationEndpoint,
                TokenEndpoint = TokenEndpoint,
                APIEndPoint = APIEndPoint
            };

            _writableLocations.Update(opt => {
                opt.ClientId = obj.ClientId;
                opt.ClientSecret = obj.ClientSecret;
                opt.AuthorizationEndpoint = obj.AuthorizationEndpoint;
                opt.TokenEndpoint = obj.TokenEndpoint;
                opt.APIEndPoint = obj.APIEndPoint;
            });

            ViewBag.ClientId = configuration["JDeere:ClientId"];
            ViewBag.ClientSecret = configuration["JDeere:ClientSecret"];
            ViewBag.AuthorizationEndpoint = configuration["JDeere:AuthorizationEndpoint"];
            ViewBag.TokenEndpoint = configuration["JDeere:TokenEndpoint"];
            ViewBag.APIEndPoint = configuration["JDeere:APIEndPoint"];

            TempData["ClientId"] = ViewBag.ClientId;
            TempData["ClientSecret"] = ViewBag.ClientSecret;
            TempData["AuthorizationEndpoint"] = ViewBag.AuthorizationEndpoint;
            TempData["TokenEndpoint"] = ViewBag.TokenEndpoint;
            TempData["APIEndPoint"] = ViewBag.APIEndpoint;

            var APISave = ViewBag.APIEndPoint;
            var settings = new JDeere
            {
                Token = await HttpContext.GetTokenAsync("access_token")
            };
            var token = await HttpContext.GetTokenAsync("access_token");
            var Url = APISave.ToString();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStringAsync();
            var djson = JsonConvert.DeserializeObject<Stream>(stream);
            var json = JsonConvert.SerializeObject(stream, Formatting.Indented);
            var jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);
            ViewBag.APIPath = jsonFormatted.Value.ToString();

            return View(obj);
        }

        private readonly IWritableOptions<JDeere> _writableLocations;
        public APIController(IWritableOptions<JDeere> writableLocations)
        {
            _writableLocations = writableLocations;
        }
        public async Task<IActionResult> Index([FromServices]IConfiguration configuration, string ClientId, string ClientSecret, string AuthorizationEndpoint, string TokenEndpoint, string APIEndPoint)
        {
            JDeere obj = new JDeere
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                AuthorizationEndpoint = AuthorizationEndpoint,
                TokenEndpoint = TokenEndpoint,
                APIEndPoint = APIEndPoint
            };

            _writableLocations.Update(opt => {
                opt.ClientId = obj.ClientId;
                opt.ClientSecret = obj.ClientSecret;
                opt.AuthorizationEndpoint = obj.AuthorizationEndpoint;
                opt.TokenEndpoint = obj.TokenEndpoint;
                opt.APIEndPoint = obj.APIEndPoint;
            });

            ViewBag.ClientId = configuration["JDeere:ClientId"];
            ViewBag.ClientSecret = configuration["JDeere:ClientSecret"];
            ViewBag.AuthorizationEndpoint = configuration["JDeere:AuthorizationEndpoint"];
            ViewBag.TokenEndpoint = configuration["JDeere:TokenEndpoint"];
            ViewBag.APIEndPoint = configuration["JDeere:APIEndPoint"];

            TempData["ClientId"] = ViewBag.ClientId;
            TempData["ClientSecret"] = ViewBag.ClientSecret;
            TempData["AuthorizationEndpoint"] = ViewBag.AuthorizationEndpoint;
            TempData["TokenEndpoint"] = ViewBag.TokenEndpoint;
            TempData["APIEndPoint"] = ViewBag.APIEndpoint;

            var APISave = TempData["APIEndPoint"];
            ViewBag.APIEndPoint = APISave;

            var settings = new JDeere
            {
                Token = await HttpContext.GetTokenAsync("access_token")
            };

            var token = await HttpContext.GetTokenAsync("access_token");
            var Url = APISave.ToString();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.SerializeObject(stream, Formatting.Indented);
            var json2 = new JsonResult(json) { SerializerSettings = new JsonSerializerSettings() { Formatting = Formatting.Indented } };
            ViewBag.APIPath = json2.Value.ToString();

            return View(settings);
        }
    }
}