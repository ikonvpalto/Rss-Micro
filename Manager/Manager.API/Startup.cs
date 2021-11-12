using System;
using System.Collections.Generic;
using System.Linq;
using Api.Common;
using Downloader.Facade;
using Filter.Facade;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Manager.API.Database;
using Manager.API.Repositories;
using Manager.API.Services;
using Manager.Common.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sender.Facade;

namespace Manager.API
{
    public sealed class Startup : BaseStartup
    {
        private const string ConnectionStringSetting = "ConnectionString";
        private const string DownloaderAddressParam = "DownloaderAddress";
        private const string FilterAddressParam = "FilterAddress";
        private const string SenderAddressParam = "SenderAddress";

        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<ManagerDbContext>(options =>
                options.UseNpgsql(Configuration[ConnectionStringSetting]));

            services.AddScoped<IJobPeriodicityRepository, JobPeriodicityRepository>();

            services.AddScoped<IManagerManager, ManagerManager>();
            services.AddScoped<IManagerProvider, ManagerProvider>();
            services.AddScoped<IMailingService, MailingService>();

            services.AddDownloaderProxies(Configuration[DownloaderAddressParam]);
            services.AddFilterProxies(Configuration[FilterAddressParam]);
            services.AddSenderProxies(Configuration[SenderAddressParam]);

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration[ConnectionStringSetting]));

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 2;
            });

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ManagerDbContext dbContext)
        {
            BaseConfigure(app, env);

            app.UseHangfireDashboard("/hangfire", new ()
            {
                Authorization = new List<IDashboardAuthorizationFilter> { new DashboardAuthorizationFilter() },
                IgnoreAntiforgeryToken = true,
                DisplayNameFunc = (context, job) =>
                {
                    if (job.Args?.FirstOrDefault() is Guid jobGuid)
                    {
                        return $"Publish event: {jobGuid}";
                    }

                    return job.ToString();
                }
            });

            dbContext.Database.Migrate();
        }
    }
}
