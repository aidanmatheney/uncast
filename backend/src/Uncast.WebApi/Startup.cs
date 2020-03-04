namespace Uncast.WebApi
{
    using System;
    using System.Data;
    using System.IO;
    using System.Reflection;

    using IdentityServer4.Stores;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.Net.Http.Headers;
    using Microsoft.OpenApi.Models;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Services;
    using Uncast.Entities;
    using Uncast.Services;
    using Uncast.Utils;

    internal sealed class Startup
    {
        private const string ConnectionStringEnvironmentVariableName = "UNCAST_WEBAPI_CONNECTIONSTRING";
        private const string DevelopmentCorsPolicyName = "DevelopmentCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            ThrowIf.Null(configuration, nameof(configuration));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddIdentityCore<AppUser>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;

                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Stores.MaxLengthForKeys = 128;
                    options.Stores.ProtectPersonalData = false; // TODO: Investigate whether this should be enabled
                })
                .AddRoles<AppRole>()
                .AddUserStore<AppUserStore>()
                .AddRoleStore<AppRoleStore>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddIdentityResources()
                .AddApiResources()
                .AddClients()
                .AddSigningCredentials();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityServerJwt()
                .AddIdentityCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicyNames.RequireCuratorRole, policy => policy.RequireRole(RoleNames.Curator));
                options.AddPolicy(AuthorizationPolicyNames.RequireAdministratorRole, policy => policy.RequireRole(RoleNames.Administrator));
            });

            services.AddCors(options =>
            {
                options.AddPolicy(DevelopmentCorsPolicyName, policy =>
                {
                    policy.WithOrigins
                    (
                        "http://localhost:3000" // Web app
                    );
                    policy.WithHeaders(HeaderNames.Authorization);
                });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Uncast Web API",
                    Version = "v1"
                });

                options.CustomOperationIds(api =>
                {
                    if (api.ActionDescriptor is ControllerActionDescriptor controllerAction)
                        return controllerAction.ActionName;

                    throw new InvalidOperationException($"Unknown {nameof(ActionDescriptor)} type: {api.ActionDescriptor.GetType()}");
                });

                var docFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var docFilePath = Path.Combine(AppContext.BaseDirectory, docFileName);
                options.IncludeXmlComments(docFilePath);
            });

            services.AddScoped<IUserStore<AppUser>, AppUserStore>();
            services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();
            services.AddScoped<IPersistedGrantStore, AppPersistedGrantStore>();
            services.AddScoped<IDeviceFlowStore, AppDeviceFlowStore>();

            var baseConnectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
            if (baseConnectionString is null)
                throw new InvalidOperationException($"{ConnectionStringEnvironmentVariableName} environment variable is not set");

            var connectionString = new MySqlConnectionStringBuilder(baseConnectionString)
            {
                AllowUserVariables = true,
                AllowLoadLocalInfile = true
            };

            services.AddScoped(serviceProvider => new MySqlConnection(connectionString.ToString()));
            services.AddScoped<IDbConnection>(serviceProvider => serviceProvider.GetRequiredService<MySqlConnection>());

            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
            services.AddScoped<IAppPersistedGrantService, AppPersistedGrantService>();
            services.AddScoped<IAppDeviceFlowService, AppDeviceFlowService>();

            services.AddScoped<IPodcastService, PodcastService>();
            services.AddScoped<ILogService, LogService>();

            services.AddScoped<IEmailSender, ToLogEmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Show the fake personally identifiable information to aid debugging
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseCors(DevelopmentCorsPolicyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
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