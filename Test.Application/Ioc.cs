using Microsoft.Extensions.DependencyInjection;

namespace Test.Application
{
    public interface IAssemblyRoot
    {
    }
    public static class Ioc
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IAssemblyRoot>());
            return services;
        }
    }
}
