using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace Test.Application.Configuration
{
    public class ConfigurationSetup : IConfigureOptions<Configuration>
    {
        private const string SectionName = "Application";
        private readonly IConfiguration _configuration;

        public ConfigurationSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(Configuration options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
