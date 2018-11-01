using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;

namespace PalTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var message = Configuration.GetValue<string>("WELCOME_MESSAGE");
            if (string.IsNullOrEmpty(message))
            {
                throw new ApplicationException("WELCOME_MESSAGE not configured.");
            }
            services.AddSingleton(sp => new WelcomeMessage(message));

            services.AddSingleton(sp => new CloudFoundryInfo(
                Configuration.GetValue<string>("PORT"),
                Configuration.GetValue<string>("MEMORY_LIMIT"),
                Configuration.GetValue<string>("CF_INSTANCE_INDEX"),
                Configuration.GetValue<string>("CF_INSTANCE_ADDR")
            ));

            services.AddScoped<ITimeEntryRepository, MySqlTimeEntryRepository>();

            services.AddDbContext<TimeEntryContext>(options => options.UseMySql(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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