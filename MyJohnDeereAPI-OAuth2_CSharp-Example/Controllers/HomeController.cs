using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyJohnDeereAPI_OAuth2_CSharp_Example.Models;

namespace MyJohnDeereAPI_OAuth2_CSharp_Example.Controllers 
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        protected JDeere MySettings { get; set; }
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.ClientId = _configuration["JDeere:ClientId"];
            ViewBag.ClientSecret = _configuration["JDeere:ClientSecret"];
            ViewBag.AuthorizationEndpoint = _configuration["JDeere:AuthorizationEndpoint"];
            ViewBag.Scopes = _configuration["JDeere:Scopes"];
            ViewBag.TokenEndpoint = _configuration["JDeere:TokenEndpoint"];
            ViewBag.APIEndpoint = _configuration["JDeere:APIEndpoint"];

            TempData["ClientId"] = ViewBag.ClientId;
            TempData["ClientSecret"] = ViewBag.ClientSecret;
            TempData["AuthorizationEndpoint"] = ViewBag.AuthorizationEndpoint;
            TempData["Scopes"] = ViewBag.Scopes;
            TempData["TokenEndpoint"] = ViewBag.TokenEndpoint;
            TempData["APIEndPoint"] = ViewBag.APIEndpoint;

            return View();
        }
        public IActionResult ReadMe()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}

