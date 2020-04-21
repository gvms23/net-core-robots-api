using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Kodo.Robots.Infra.CrossCutting.Swagger.Providers
{
    /// <summary>
    /// Referências: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
    /// </summary>
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Kodo.Robots API",
                        Version = "v1",
                        Description = "API REST - ASP .NET Core 3.1 to application <b>Robots</b> from <b>Kodo</b>.",
                        Contact = new OpenApiContact
                        {
                            Name = "Gabriel Vicente",
                            Email = "gabrielvicente.m@gmail.com",
                            Url = new Uri("http://linkedin.com/in/gvms23")
                        }
                    });

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Use: Enter 'Bearer {yourToken}'"
                    });

                options.IncludeXmlComments(Path.Combine("wwwroot", "api-docs.xml"));

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                        .UseSwaggerUI(args =>
                        {
                            /* Route to access the docs */
                            args.RoutePrefix = "documentation";

                            args.SwaggerEndpoint("../swagger/v1/swagger.json", "Documentation API v1");

                            args.DocumentTitle = "Robots API - Swagger UI";
                        });
        }
    }
}
