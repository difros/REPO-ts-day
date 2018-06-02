using GQ.Core.service;
using GQ.Log;
using GQ.Security;
using GQ.Security.JWT;
using GQ.Sql.MySQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebNetCore.Controllers;
using WebNetCore.Data.sql.domain;

namespace WebNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            ServicesContainer.AddHostingEnvironment(env);

            Configuration = ServicesContainer.ConfigurationBuilder();

            Log.StartLog();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ServicesContainer.AddServices(services);

            MySQLServiceDBConfiguration.SetConnectionString(Configuration.GetSection("ConnectionString").GetSection("MySQL").Value).SetAssembly(typeof(GQ_Usuarios).Assembly).SetOption((builder) =>
            {
                return builder;
            });

            SecurityConfigure.Configure(
                TimeSpan.FromDays(1),
                SecurityJWT.UsuarioLogueado<GQ_Usuarios>,
                WebNetCore.com.gq.security.Security.hasPermission,
                new Type[] { typeof(LoginController) });

            System.Web.HttpContext.Configure();

            //services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc();
                ///// Esto es para el Idioma
                //.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                //    .AddDataAnnotationsLocalization()
                //    .AddRazorOptions(options =>
                //    {
                //        var previous = options.CompilationCallback;
                //        options.CompilationCallback = context =>
                //        {
                //            previous?.Invoke(context);
                //            context.Compilation = context.Compilation.AddReferences(MetadataReference.CreateFromFile(typeof(UtilHelper).Assembly.Location));
                //        };
                //    });

            return ServicesContainer.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            ////IDIOMA 
            //// REVISAR https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization
            //var supportedCultures = new[]
            //{
            //        new CultureInfo("en-US"),
            //        new CultureInfo("en-AU"),
            //        new CultureInfo("en-GB"),
            //        new CultureInfo("en"),
            //        new CultureInfo("es-ES"),
            //        new CultureInfo("es-MX"),
            //        new CultureInfo("es"),
            //    };

            //var requestLocalizationOptions = new RequestLocalizationOptions
            //{
            //    RequestCultureProviders =
            //    {
            //        new CookieRequestCultureProvider()
            //    },
            //    DefaultRequestCulture = new RequestCulture("es"),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures
            //};

            //app.UseRequestLocalization(requestLocalizationOptions);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<MySQLService>().Migrate();
            }

            com.gq.security.Security.CreateAccessSecurity(app);

        }
    }
}
