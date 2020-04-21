using Kodo.Robots.Domain.Interfaces.Repositories;
using Kodo.Robots.Domain.Interfaces.UoW;
using Kodo.Robots.Infra.Data.Repositories;
using Kodo.Robots.Infra.Data.UoW;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kodo.Robots.Infra.CrossCutting.IoC.Providers
{
    public class NativeInjectorBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRobotRepository, RobotRepository>();

        }
    }
}
