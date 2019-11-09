using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

            services.AddIdentityServer()
                .AddTestUsers(TestUsers.Users)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddDeveloperSigningCredential(false);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole();
            //app.UseDeveloperExceptionPage();

            //app.UseIdentityServer();

            //app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseIdentityServer();
            //app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}