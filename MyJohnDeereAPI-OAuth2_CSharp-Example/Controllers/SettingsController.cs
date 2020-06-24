using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Interfaces;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Models;

namespace MyJohnDeereAPI_OAuth2_CSharp_Example.Controllers 
{
    public class SettingsController : Controller
    {
        public IActionResult Index([FromServices]IConfiguration configuration)
        {
            ViewBag.ClientId = configuration["JDeere:ClientId"];
            ViewBag.ClientSecret = configuration["JDeere:ClientSecret"];
            ViewBag.AuthorizationEndpoint = configuration["JDeere:AuthorizationEndpoint"];
            ViewBag.Scopes = configuration["JDeere:Scopes"];
            ViewBag.TokenEndpoint = configuration["JDeere:TokenEndpoint"];
            ViewBag.APIEndPoint = configuration["JDeere:APIEndPoint"];

            return View();
        }
        [HttpPost]
        public IActionResult UpdateSettings(string ClientId, string ClientSecret, string AuthorizationEndpoint, string Scopes, string TokenEndpoint, string APIEndPoint)
        {
            JDeere obj = new JDeere
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                AuthorizationEndpoint = AuthorizationEndpoint,
                Scopes = Scopes,
                TokenEndpoint = TokenEndpoint,
                APIEndPoint = APIEndPoint
            };

            _writableLocations.Update(opt => {
                opt.ClientId = obj.ClientId;
                opt.ClientSecret = obj.ClientSecret;
                opt.AuthorizationEndpoint = obj.AuthorizationEndpoint;
                opt.Scopes = obj.Scopes;
                opt.TokenEndpoint = obj.TokenEndpoint;
                opt.APIEndPoint = obj.APIEndPoint;
            });
            return View(obj);
        }

        private readonly IWritableOptions<JDeere> _writableLocations;
        public SettingsController(IWritableOptions<JDeere> writableLocations)
        {
            _writableLocations = writableLocations;
        }

    }
}