namespace Frontend
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.HealthChecks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class Startup
    {
        private static IServiceProvider _serviceProvider;
        private IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                checks.AddUrlCheck(Configuration["WebApiUrlHC"], TimeSpan.FromMilliseconds(1));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _serviceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.Map("/getWebApiUrl", GetLocalWebApiUrl);
            }
            else if (env.IsProduction())
            {
                app.Map("/getWebApiUrl", GetProductionWebApiUrl);
            }

            app.UseStaticFiles();
        }

        private static void GetLocalWebApiUrl(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await HealthCheck();

                var webApiUrl = "http://localhost:5001";
                var response = new { webApiUrl };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            });
        }

        private static void GetProductionWebApiUrl(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var externalIpIsReady = false;
                string externalIp = null;
                while (!externalIpIsReady)
                {
                    var httpClient = new HttpClient();
                    try
                    {
                        var webApiConfig =
                            await httpClient.GetStringAsync(
                                "http://localhost:8001/api/v1/namespaces/default/services/webapi");

                        var @object = (dynamic)JsonConvert.DeserializeObject(webApiConfig);
                        externalIp = (string)@object.status.loadBalancer.ingress[0].ip;
                        externalIpIsReady = true;
                    }
                    catch
                    {
                        // external IP is not ready
                    }
                }

                await HealthCheck();

                var webApiUrl = $"http://{externalIp}:5001";
                var response = new { webApiUrl };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            });
        }

        private static async Task HealthCheck()
        {
            var healthCheckService = _serviceProvider.GetService<IHealthCheckService>();
            var isHealthy = false;
            while (!isHealthy)
            {
                if ((await healthCheckService.CheckHealthAsync()).CheckStatus == CheckStatus.Healthy)
                    isHealthy = true;
                else
                    await Task.Delay(1000);
            }
        }
    }
}