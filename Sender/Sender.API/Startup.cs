using Api.Common;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sender.API.Database;
using Sender.API.ExternalServices;
using Sender.API.Repository;
using Sender.API.Sections;
using Sender.API.Services;
using Sender.Common.Contracts;

namespace Sender.API
{
    public sealed class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<SenderDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString"]));

            services.AddScoped<IEmailRepository, EmailRepository>();

            services.Configure<SmtpSection>(Configuration.GetSection("SmtpSettings"));
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddScoped<SmtpClient>(_ => new ());

            services.AddScoped<ISenderProvider, SenderProvider>();
            services.AddScoped<ISenderManager, SenderManager>();

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SenderDbContext dbContext)
        {
            BaseConfigure(app, env);

            dbContext.Database.Migrate();
        }
    }
}
