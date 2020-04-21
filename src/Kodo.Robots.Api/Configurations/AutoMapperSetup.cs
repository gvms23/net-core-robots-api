using AutoMapper;
using Kodo.Robots.Api.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kodo.Robots.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            // Registering Mappings automatically only works if the 
            // Automapper Profile classes are in ASP.NET project
            AutoMapperConfiguration.RegisterMappings();
        }
    }
}
