namespace CSharpApp.Models
{
    public class JDeere
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WellKnown { get; set; }
        public string ServerUrl { get; set; }
        public string CallbackUrl { get; set; }
        public Token AccessToken { get; set; }
        public string Scopes { get; set; }
        public string State { get; set; }
        public string APIURL { get; set; }
    }
}

