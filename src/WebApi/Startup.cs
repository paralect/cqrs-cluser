namespace WebApi
{
    using DataService;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence.MongoDb;
    using ParalectEventSourcing.Serialization;
    using ParalectEventSourcing.Utils;

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
            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });
            services.AddMemoryCache();

            var mongoDbConnectionSettings = new MongoDbConnectionSettings();
            var mongoClient = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(mongoDbConnectionSettings.HostName, mongoDbConnectionSettings.Port)
            });

            services
                .AddTransient<ICommandBus, RabbitMqCommandBus>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                // TODO consider creating channels per thread
                .AddTransient<IChannel, Channel>()
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<RabbitMqConnectionSettings>(new RabbitMqConnectionSettings())
                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddTransient<IShipmentDataService, ShipmentDataService>()
                .AddSingleton<IMongoClient>(mongoClient)
                .AddTransient<IDatabase, Database>();
                
            services.AddCors();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(cpb => cpb.AllowAnyOrigin().AllowCredentials().AllowAnyHeader().AllowAnyMethod());

            app.UseWebSockets();
            app.UseSignalR();

            app.UseMvc();
        }
    }
}
