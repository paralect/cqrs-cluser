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
            }

            app.Map("/getIp", GetIp);

            app.UseStaticFiles();
        }

        private static void GetIp(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                try
                {
                    var client = new HttpClient();

                    var webApiConfig =
                        await client.GetStringAsync(
                            "http://localhost:8001/api/v1/namespaces/default/services/web-api");

                    var @object = (dynamic) JsonConvert.DeserializeObject(webApiConfig);
                    var externalIp = (string) @object.status.loadBalancer.ingress[0].ip;

                    await context.Response.WriteAsync("WebApi IP: " + externalIp);
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