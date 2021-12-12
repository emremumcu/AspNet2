using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet2.AppLib.Evaluators;
using AspNet2.AppLib.StartupExt;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNet2
{
    public class Startup
    {
        private IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; set; }
                
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Env = env;

            // Configuration can also be build manually like the below ConfigurationRoot
            Configuration = configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile("data.json", optional: true, reloadOnChange: true)
            ;

            ConfigurationRoot = builder.Build();
        }

        /// <summary>
        /// When the application is requested for the first time, it calls ConfigureServices method. 
        /// ASP.net core has built-in support for Dependency Injection. We can add services to DI container using this method.
        /// Use ConfigureServices method to configure Dependency Injection (services).
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {
            services._InitMVC(Env);

            services._AddSession();

            services._AddCookiePolicyOptions();

            services._ConfigureViewLocationExpander();

            services._AddAuthentication();

            services._AddAuthorization();

            // TODO: Remove in PROD
            // This logins user automatically (Mocking a real user in dev env)
            services.AddSingleton<IPolicyEvaluator, TestPolicyEvaluator>();
        }

        /// <summary>
        /// This method is used to define how the application will respond on each HTTP request.
        /// This method is also used to configure middleware in HTTP pipeline.
        /// Use Configure method to set up middlewares, routing rules etc.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app._InitApp();

            

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app._UseCookiePolicy();

            app.UseRouting();

            app._UseSession();

            app._UseAuthentication();
            app._UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            /*
             * The big problem with the AuthorizeFilter approach is that it's an MVC-only feature. 
             * ASP.NET Core 3.0+ provides a different mechanism for setting authorization on endpoints the 
             * RequireAuthorization() extension method on IEndpointConventionBuilder.
             * 
             * Instead of configuring a global AuthorizeFilter, call RequireAuthorization() when configuring 
             * the endpoints of your application, in Configure():
             */
            ////app.UseEndpoints(endpoints =>
            ////{
            ////    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
            ////    endpoints.MapHealthChecks("/health").RequireAuthorization();
            ////    endpoints.MapRazorPages().RequireAuthorization("MyCustomPolicy");
            ////    endpoints.MapHealthChecks("/healthz").RequireAuthorization("OtherPolicy", "MainPolicy");
            ////});
        }
    }
}


// ------------------------------------------------------------------------------------------------------------
// The following Startup.Configure method adds security-related middleware components in the recommended order:
// ------------------------------------------------------------------------------------------------------------

//public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    if (env.IsDevelopment()) {
//        app.UseDeveloperExceptionPage();
//        app.UseDatabaseErrorPage();
//    }
//    else {
//        app.UseExceptionHandler("/Error");
//        app.UseHsts();
//    }
//    app.UseHttpsRedirection();
//    app.UseStaticFiles();
//    app.UseCookiePolicy();
//    app.UseRouting();
//    app.UseRequestLocalization();
//    app.UseCors();
//    app.UseAuthentication();
//    app.UseAuthorization();
//    app.UseSession();
//    app.UseResponseCaching();
//    app.UseEndpoints(endpoints =>
//    {
//        endpoints.MapRazorPages();
//        endpoints.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");
//    });
//}