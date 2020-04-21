using Kodo.Robots.Api.Configurations;
using Kodo.Robots.Infra.CrossCutting.IoC.Providers;
using Kodo.Robots.Infra.CrossCutting.Swagger.Providers;
using Kodo.Robots.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kodo.Robots.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapperSetup();

            // In Memory
            services.AddDbContext<RobotsContext>(opt => opt.UseInMemoryDatabase("Robots"));

            services.AddControllers(options =>
            {
                // To configure Api versioning.
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.UseCentralRoutePrefix(new RouteAttribute("api/v{version:apiVersion}"));

                // To configure controller routes in kebab-case.
                options.Conventions.Add(new RouteTokenTransformerConvention(
                                             new SlugifyParameterTransformer()));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // To show ModelState.IsValid errors.
                options.SuppressModelStateInvalidFilter = true;
            })

            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.UseCamelCasing(true);
            });

            services.AddSwaggerConfiguration();

            /*
             * Gabriel Vicente:
             * In case you need to use API versioning, you should add the [ApiVersion("x.0")] attribute to the
             * specific method. In case you need 2 method versions in the same controller, you also need to
             * add the two attributes (of the two versions) to the controller. E.g.:
             *
             *  [ApiVersion("1.0")]
             *  [ApiVersion("2.0")]
             *  [Authorize, Route("[controller]"), ApiController]
             *  public class TestController : ControllerBase
             *
             * Ref: https://www.talkingdotnet.com/support-multiple-versions-of-asp-net-core-web-api/
             */
            int? _apiVersion = Configuration.GetSection(nameof(ApplicationSettings))?.Get<ApplicationSettings>()?.ApiVersion;

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(_apiVersion ?? 1, 0);
                options.UseApiBehavior = false;
            });


            services.AddLogging(l => l.AddConsole());

            //Adding dependencies from another layers (isolated from Api presentation)
            NativeInjectorBootstrapper.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwaggerConfiguration();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
