namespace Frontend
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
                var webApiUrl = "http://localhost:5001";
                var response = new { webApiUrl };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            });
        }

        private static void GetProductionWebApiUrl(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                try
                {
                    var client = new HttpClient();

                    var webApiConfig =
                        await client.GetStringAsync(
                            "http://localhost:8001/api/v1/namespaces/default/services/webapi");

                    var @object = (dynamic)JsonConvert.DeserializeObject(webApiConfig);

                    string webApiUrl = null;
                    try
                    {
                        var externalIp = (string)@object.status.loadBalancer.ingress[0].ip;
                        webApiUrl = $"http://{externalIp}:5001";
                    }
                    catch
                    {
                        // probably, external IP is not created yet
                    }

                    var response = new { webApiUrl };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }
    }
}