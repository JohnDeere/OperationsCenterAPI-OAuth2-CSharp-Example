using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CSharpApp.Interfaces;
using CSharpApp.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Interface
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var configuration = (IConfigurationRoot)provider.GetService<IConfiguration>();
                var environment = provider.GetService<IWebHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(environment, options, configuration, section.Key, file);
            });
        }
        public static void ConfigureAPIOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<JDeere>(configuration.GetSection("JDeere"));
            services.AddAuthentication(config =>
            {
                // We check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; ;
                // When we sign in we will deal out a cookie
                config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = "OurServer";

            })
                .AddCookie()

                .AddOAuth("OurServer", config =>
                {
                    config.ClientId = configuration["JDeere:ClientId"];
                    config.ClientSecret = configuration["JDeere:ClientSecret"];
                    config.CallbackPath = "/callback";
                    config.AuthorizationEndpoint = configuration["JDeere:AuthorizationEndpoint"];
                    config.TokenEndpoint = configuration["JDeere:TokenEndpoint"];
                    config.Scope.Add(configuration["JDeere:Scopes"]);
                    config.SaveTokens = true;
                    config.Events = new OAuthEvents()
                    {
                        OnCreatingTicket = context =>
                        {
                            byte[] ConvertFromBase64String(string input)
                            {
                                var accessToken = context.AccessToken;

                                var base64payload = accessToken.Split('.')[1];
                                if (String.IsNullOrWhiteSpace(base64payload)) return null;
                                try
                                {
                                    string working = base64payload.Replace('-', '+').Replace('_', '/'); ;
                                    while (working.Length % 4 != 0)
                                    {
                                        working += '=';
                                    }
                                    return Convert.FromBase64String(working);
                                }
                                catch (Exception)
                                {
                                    return null;
                                }
                            }
                            var bytes = ConvertFromBase64String(context.AccessToken);
                            var jsonPayload = Encoding.UTF8.GetString(bytes);

                            return Task.CompletedTask;
                        }
                    };
                });
        }
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}







