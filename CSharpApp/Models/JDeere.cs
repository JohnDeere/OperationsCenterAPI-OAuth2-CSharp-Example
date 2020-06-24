using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpApp.Models 
{
    public class JDeere
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string APIEndPoint { get; set; }
        public string Scopes { get; set; }
        public string Token { get; set; }
        public string APIPath { get; set; }
    }
}

