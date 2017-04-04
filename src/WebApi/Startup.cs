namespace WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Messaging;
    using ParalectEventSourcing.Utils;
    using RabbitMQ.Client;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<ICommandBus, RabbitMqCommandBus>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                // TODO consider creating channels per thread
                .AddTransient<IChannel, Channel>()
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IConnection>(sp => sp.GetService<IConnectionFactory>().CreateConnection())
                .AddTransient<IModel>(sp => sp.GetService<IConnection>().CreateModel())
                .AddTransient<IMessageSerializer, MessageSerializer>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
