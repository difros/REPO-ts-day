using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace GQ.Core.service
{
    public static class ServicesContainer
    {
        public static IHostingEnvironment HostingEnvironment { get; private set; }
        public static IServiceCollection Services { get; private set; }

        public static IServiceProvider ServicesProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        public static T Get<T>(this IConfiguration configuration)
        {
            return ConfigurationBinder.Get<T>(configuration);
        }

        public static void AddHostingEnvironment(IHostingEnvironment Environment)
        {
            ServicesContainer.HostingEnvironment = Environment;
        }

        public static void AddServices(IServiceCollection services)
        {
            ServicesContainer.Services = services;
        }

        public static void CreateService()
        {
            AddServices(new ServiceCollection());
        }

        public static void AddSingleton<TService>(TService value) where TService : class
        {
            Services.AddSingleton<TService>(value);
        }

        public static void AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            Services.AddSingleton<TService, TImplementation>();
        }

        public delegate void DelegateConfigurationBuilder(IConfigurationBuilder Builder);

        public static IConfiguration ConfigurationBuilder(DelegateConfigurationBuilder actionBuilder = null)
        {
            var Builder = new ConfigurationBuilder()
               .SetBasePath(HostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", optional: true)
               .AddJsonFile("config.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            _contentRootPath = HostingEnvironment.ContentRootPath;

            if (actionBuilder != null)
            {
                actionBuilder(Builder);
            }

            Configuration = Builder.Build();

            return Configuration;
        }

        public static void SetConfiguration(IConfiguration Configuration)
        {
            ServicesContainer.Configuration = Configuration;
        }

        private static string _contentRootPath = "";

        public static void SetContentRootPath(string value)
        {
            _contentRootPath = value;
        }

        public static string ContentRootPath()
        {
            return _contentRootPath;
        }

        public static void AddScoped<TService>() where TService : class
        {
            Services.AddScoped<TService>();
        }

        public static void AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            Services.AddScoped<TService>(implementationFactory);
        }

        public static void AddTransient<TService>() where TService : class
        {
            Services.AddTransient<TService>();
        }

        public static TService GetService<TService>() where TService : class
        {
            return ServicesProvider.GetService<TService>();
        }

        public static TService GetRequiredService<TService>()
        {
            return ServicesProvider.GetRequiredService<TService>();
        }

        public static object GetService(Type type)
        {
            return ServicesProvider.GetService(type);
        }

        public static IServiceProvider BuildServiceProvider()
        {
            ServicesProvider = Services.BuildServiceProvider();
            return ServicesProvider;
        }
    }
}
