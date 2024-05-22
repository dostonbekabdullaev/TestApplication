using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Test.Application;
using Test.Application.Configuration;
using Test.DAL.Data;
using Test.DAL.Repository;

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
        }
    }
}
