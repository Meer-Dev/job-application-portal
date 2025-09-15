using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using solvefy.task.Configuration;
using solvefy.task.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.IO;
using Hangfire;
using Hangfire.SqlServer;
using solvefy.task.Jobs;
using Hangfire; // Add this
using Hangfire.Dashboard; // Add this
using Hangfire.SqlServer; // Add this
using Microsoft.AspNetCore.Http; // Add this

namespace solvefy.task.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private const string _apiVersion = "v1";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddControllersWithViews(
                options => { options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute()); }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            // ✅ HANGFIRE Configuration
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
            });

            services.AddHangfireServer();

            // ✅ FIXED: Configure CORS properly
            // ✅ FIXED CORS Configuration
            var corsOrigins = _appConfiguration["App:CorsOrigins"]
                ?.Split(",", StringSplitOptions.RemoveEmptyEntries)
                ?.Select(o => o.Trim().RemovePostFix("/"))
                ?.ToArray() ?? new string[0];

            services.AddCors(options =>
            {
                options.AddPolicy(_defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(corsOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            ConfigureSwagger(services);

            // Configure Abp and Dependency Injection
            services.AddAbpWithoutCreatingServiceProvider<taskWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config"
                    )
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; });

            app.UseStaticFiles();

            app.UseRouting();

            // ✅ ADD THIS: Use CORS before other middleware
            app.UseCors(_defaultCorsPolicyName);

            app.UseAuthentication();
            app.UseAbpRequestLocalization();

            // ✅ HANGFIRE Dashboard (Add authentication if needed)
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/AbpUserConfiguration/GetAll", async context =>
                {
                    var response = new
                    {
                        localization = new { },
                        auth = new { },
                        nav = new { },
                        setting = new { },
                        clock = new { },
                        timing = new { },
                        security = new { }
                    };

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                });
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"task API {_apiVersion}");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("solvefy.task.Web.Host.wwwroot.swagger.ui.index.html");
                options.DisplayRequestDuration();
            });

            // ✅ Schedule the recurring job AFTER app is configured
            ScheduleJobs(app.ApplicationServices);
        }

        private void ScheduleJobs(IServiceProvider serviceProvider)
        {
            // Schedule daily job to check job positions with low candidate applications
            RecurringJob.AddOrUpdate<JobPositionCandidateCheckerJob>(
                "check-job-positions-candidates",
                job => job.Execute(null),
                Cron.Daily(9) // Run daily at 9 AM
            );
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "task API",
                    Description = "task",
                    Contact = new OpenApiContact
                    {
                        Name = "task",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/aspboilerplate"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
                    }
                });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //add summaries to swagger
                bool canShowSummaries = _appConfiguration.GetValue<bool>("Swagger:ShowSummaries");
                if (canShowSummaries)
                {
                    // Declare variables first
                    var hostXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var hostXmlPath = Path.Combine(AppContext.BaseDirectory, hostXmlFile);

                    var applicationXml = "solvefy.task.Application.xml";
                    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXml);

                    var webCoreXmlFile = "solvefy.task.Web.Core.xml";
                    var webCoreXmlPath = Path.Combine(AppContext.BaseDirectory, webCoreXmlFile);

                    // Check if files exist before adding them
                    if (File.Exists(hostXmlPath))
                        options.IncludeXmlComments(hostXmlPath);

                    if (File.Exists(applicationXmlPath))
                        options.IncludeXmlComments(applicationXmlPath);

                    if (File.Exists(webCoreXmlPath))
                        options.IncludeXmlComments(webCoreXmlPath);
                }
            });
        }

        // ✅ Simple Hangfire Authorization Filter (allows all users - customize as needed)
        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // For development - allow all access
                // In production, add proper authentication/authorization
                return true;
            }
        }
    }
}