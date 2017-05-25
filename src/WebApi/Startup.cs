namespace WebApi
{
    using System.Threading.Tasks;
    using DataProtection;
    using DataService;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.HealthChecks;
    using MongoDB.Driver;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence.MongoDb;
    using ParalectEventSourcing.Serialization;
    using ParalectEventSourcing.Utils;

    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });

            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });
            services.AddMemoryCache();

            RegisterConnectionSettings(services);
            RegisterCommonServices(services);

            services.AddCors();

            services.AddMvc();

            // TODO temporary disable data protection for SignalR scaling
            // https://github.com/aspnet/DataProtection/issues/192
            services.DisableDataProtection();
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

        private void RegisterConnectionSettings(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<RabbitMqConnectionSettings>(options => Configuration.GetSection("RabbitMQ").Bind(options))
                .Configure<MongoDbConnectionSettings>(options => Configuration.GetSection("MongoDB").Bind(options));
        }

        private void RegisterCommonServices(IServiceCollection services)
        {
            services
                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<IWriteModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<ICommandBus, RabbitMqCommandBus>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                .AddTransient<IShipmentDataService, ShipmentDataService>()
                .AddSingleton<IMongoClient>(sp => new MongoClient(sp.GetService<IOptions<MongoDbConnectionSettings>>().Value.ConnectionString))
                .AddTransient<IDatabase, Database>();
        }
    }
}
