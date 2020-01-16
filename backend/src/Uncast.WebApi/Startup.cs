namespace Uncast.WebApi
{
    using System;
    using System.Data;
    using System.IO;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Services;
    using Uncast.Utils;

    internal sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ThrowIf.Null(configuration, nameof(configuration));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Uncast Web API",
                    Version = "v1"
                });

                var docFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var docFilePath = Path.Combine(AppContext.BaseDirectory, docFileName);
                options.IncludeXmlComments(docFilePath);
            });

            var connectionString = Environment.GetEnvironmentVariable("UNCAST_WEBAPI_CONNECTIONSTRING");
            if (connectionString is null)
                throw new InvalidOperationException("UNCAST_WEBAPI_CONNECTIONSTRING environment variable is not set");
            services.AddScoped<IDbConnection>(serviceProvider => new MySqlConnection(connectionString));

            services.AddScoped<IPodcastService, PodcastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/api-docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api-docs/v1/swagger.json", "Uncast Web API V1");
                options.RoutePrefix = "api-docs";
            });
        }
    }
}