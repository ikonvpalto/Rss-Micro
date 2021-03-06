using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Api.Common.Middlewares;
using Common.Exceptions;
using Common.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Common
{
    public abstract class BaseStartup
    {
        private const string CorsPolicy = "AllowAnyOrigins";

        protected BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var entryAssembly = GetEntryAssembly();
            var assemblyName = GetEntryAssemblyName();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(o =>
                {
                    o.InvalidModelStateResponseFactory =
                        context => throw new BadRequestException(context.ModelState.Values.First().Errors.First().ErrorMessage);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new TimeSpanJsonConverter());
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });


            services.AddSwaggerGen(c =>
            {
                var commentsFileName = entryAssembly?.GetName().Name + ".xml";
                var commentsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, commentsFileName);

                c.SwaggerDoc("v1", new () {Title = assemblyName, Version = "v1"});
                c.IncludeXmlComments(commentsFile);

                AddSwaggerDocs(c);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAutoMapper(entryAssembly);

            services.AddHttpClient();
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddSingleton(provider => provider.GetService<IHttpClientFactory>().CreateClient());

            services.AddCors(o => o.AddPolicy(CorsPolicy, builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        protected void BaseConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{GetEntryAssemblyName()} v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        protected abstract IServiceCollection RegisterServices(IServiceCollection services);

        protected virtual void AddSwaggerDocs(SwaggerGenOptions swaggerGenOptions) {}

        private string GetEntryAssemblyName() => GetEntryAssembly().GetName().Name ?? string.Empty;

        private Assembly GetEntryAssembly() => Assembly.GetEntryAssembly() ?? GetType().Assembly;
    }
}
