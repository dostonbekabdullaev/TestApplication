using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection;
using Test.Application;
using Test.Core.Configuration;
using Test.Cache.CacheService;
using Test.DAL.Data;
using Test.DAL.Repository;
using Test.WebService.ExceptionHandler;

namespace Test.WebService
{
    internal class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
                new ConfigurationOptions { 
                    EndPoints = { GetRedisEndpoint() }, 
                    AllowAdmin = true,
                    AbortOnConnectFail = false,
                }));
            services.AddTransient<IRedisCacheService, RedisCacheService>();
            
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            var path = _configuration.GetConnectionString("DefaultConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(path?.Replace('$','"')));

            services.AddScoped<IEntityRepository, EntityRepository>();

            services.AddApplication();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.ConfigureOptions<ConfigurationSetup>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        private string GetRedisEndpoint()
        {
            var primaryEndpoint = _configuration["Application:RedisPrimaryEndpoint"];
            var readerEndpoint = _configuration["Application:RedisReaderEndpoint"];

            return $"{primaryEndpoint}{(string.IsNullOrEmpty(readerEndpoint) ? "" : $",${readerEndpoint}")}";
        }
    }
}
