using Authentications;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerplateAPI
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSwagger(services);
            AddHealthChecks(services);
            ConfigureLogs(services);
            ConfigureMetrics(services);
            services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddAuthentication(x =>
            {
             x.DefaultChallengeScheme = AuthenticaionScheme.Bearer;
            })
           .AddScheme<AuthenticationSchemeOptions, BasicAuthenticaionHandler>(AuthenticaionScheme.Basic, null)
           .AddScheme<AuthenticationSchemeOptions, BearerAuthenticaionHandler>(AuthenticaionScheme.Bearer, null);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMetricsAllEndpoints();
            app.UseEndpoints(endpoint => {
                endpoint.MapControllers();
                endpoint.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
            });
            EnableSwagger(app);

        }
    }
}
