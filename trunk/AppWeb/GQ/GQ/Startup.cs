using FluentNHibernate.Cfg.Db;
using GQService.com.gq.dto;
using GQService.com.gq.log;
using GQService.com.gq.service;
using log4net.Repository;
using GQ.com.gq.security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using System;
using GQ.Controllers;
using GQDataService.com.gq.mapping;
using GQDataService.com.gq.migrations;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace GQ
{
    public class Startup
    {
        private TimeSpan TimeOut = TimeSpan.FromHours(4);

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            //if (env.IsDevelopment())
            //{
            //    // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
            //    builder.AddApplicationInsightsSettings(developerMode: true);
            //}
            Configuration = builder.Build();

            ILoggerRepository rep = log4net.LogManager.CreateRepository("GeminusQhom");

            log4net.Config.XmlConfigurator.Configure(rep, new System.IO.FileInfo(env.ContentRootPath + "\\log4netConfig.xml"));

            Log.Info(this, "****************************************************************************************");
            Log.Info(this, "****************************************************************************************");
            Log.Info(this, "************************************   Startup  ****************************************");
            Log.Info(this, "****************************************************************************************");
            Log.Info(this, "****************************************************************************************");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IServiceProvider result = null;
            try
            {
                Log.Info(this, "Inicializando ConfigureServices");
                // Add framework services.
                //services.AddApplicationInsightsTelemetry(Configuration);

                services.AddLocalization(options => options.ResourcesPath = "Resources");

                services.AddMvc()
                /// Esto es para el Idioma
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

                /// Esto es para que maneje sessiones
                //services.AddSession();

                // Ponemos en Singleton el HTTPContext
                services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

                // Usamos StructureMap
                GQService.com.gq.structureMap.ObjectFactory.Container.Populate(services);

                result = GQService.com.gq.structureMap.ObjectFactory.Container.GetInstance<IServiceProvider>();
            }
            catch (Exception e)
            {
                Log.Error(this, "ERROR ConfigureServices", e);
            }
            finally
            {
                Log.Info(this, "Finalizando ConfigureServices");
            }
            return result;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            try
            {
                Log.Info(this, "Inicializando Configure");

                // Porque no carga el Assembly
                var a = new MapGq_accesos();
                //Para que podamos usar el HTTPContext
                System.Web.HttpContext.ServiceProvider = svp;

                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                //loggerFactory.AddDebug();

                Log.Info(this, "************************************   Migrator  ****************************************");
                GQService.com.gq.service.Migrator.Configure(Configuration.GetSection("ConnectionDB"));
                GQService.com.gq.service.Migrator.Start(MigratorConfig.GetAssembly());
                Log.Info(this, "************************************   Migrator  ****************************************");

                Log.Info(this, "************************************   ServiceConfigure  ****************************************");
                ServiceConfigure.Configure(MigratorConfig.GetAssembly(), MySQLConfiguration.Standard
                         .QuerySubstitutions("1 true, 0 false")
                         .ConnectionString(Configuration.GetSection("ConnectionDB").GetSection("ConnectionString").Value)
                         .Driver<NHibernate.Driver.MySqlDataDriver>());
                Log.Info(this, "************************************   ServiceConfigure  ****************************************");

                Log.Info(this, "************************************   DtoConfiguration  ****************************************");
                DtoConfiguration.Configure();
                Log.Info(this, "************************************   DtoConfiguration  ****************************************");

                Log.Info(this, "************************************   SecurityConfigure  ****************************************");
                GQService.com.gq.security.SecurityConfigure.Configure(TimeSpan.FromSeconds(100), Security.hasPermission, new Type[] { typeof(LoginController) , typeof(LocksessionController) });
                Security.CreateAccessSecurity();
                Log.Info(this, "************************************   SecurityConfigure  ****************************************");

                //IDIOMA 
                // REVISAR https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-AU"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("en"),
                    new CultureInfo("es-ES"),
                    new CultureInfo("es-MX"),
                    new CultureInfo("es"),
                };

                var requestLocalizationOptions = new RequestLocalizationOptions
                {
                    RequestCultureProviders =
                {
                    new CookieRequestCultureProvider()
                },
                    DefaultRequestCulture = new RequestCulture("es"),
                    // Formatting numbers, dates, etc.
                    SupportedCultures = supportedCultures,
                    // UI strings that we have localized.
                    SupportedUICultures = supportedCultures
                };

                app.UseRequestLocalization(requestLocalizationOptions);
                //app.UseApplicationInsightsRequestTelemetry();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBrowserLink();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                //app.UseApplicationInsightsExceptionTelemetry();
                app.UseStaticFiles();

                //Indicamos que vamos a usas sesioens
                //app.UseSession();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}"
                        );
                });
            }
            catch (Exception e)
            {
                Log.Error(this, "ERROR Configure", e);
            }
            finally
            {
                Log.Info(this, "Finalizando Configure");
            }
        }
    }
}
