using System.Net.Http;
using System.Security.Claims;
using Api.Common;
using Auth.API.Sections;
using Auth.API.Services;
using Auth.Common.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auth.API
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.Configure<AuthSection>(Configuration.GetSection("Auth0"));

            var authSection = Configuration.GetSection("Auth0").Get<AuthSection>();

            services.AddScoped<IAuthService, AuthService>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient("Auth0");
                httpClient.BaseAddress = new(authSection.PreparedDomain);

                return new (
                    s.GetRequiredService<IOptions<AuthSection>>(),
                    httpClient,
                    s.GetRequiredService<ILogger<AuthService>>());
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authSection.PreparedDomain;
                    options.Audience = authSection.ApiIdentifier;
                    options.TokenValidationParameters = new()
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            BaseConfigure(app, env);
        }
    }
}
