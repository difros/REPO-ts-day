using GQ.Core.service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace GQ.Sql.Oracle
{
    public static class OracleServiceDBConfigurationExtension
    {

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value, ServiceLifetime contextLifetime) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        public static OracleServiceDBConfiguration SetOption(this OracleServiceDBConfiguration value, Func<DbContextOptionsBuilder, DbContextOptionsBuilder> function)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<OracleService>();
            builder = function(builder);
            value.Options = builder.Options;
            return value;
        }

        public static OracleServiceDBConfiguration SetOption(this OracleServiceDBConfiguration value, DbContextOptions option)
        {
            value.Options = option;
            return value;
        }

        public static OracleServiceDBConfiguration SetAssembly(this OracleServiceDBConfiguration value, Assembly assembly)
        {
            value.Assembly = assembly;
            return value;
        }
    }

    public class OracleServiceDBConfiguration : IServiceDBConfigure
    {
        public static OracleServiceDBConfiguration SetConnectionString(string ConnectionString)
        {
            var value = new OracleServiceDBConfiguration();
            value.ConnectionString = ConnectionString;

            ServicesContainer.AddSingleton<OracleServiceDBConfiguration>(value);

            ServicesContainer.AddTransient<OracleService>();

            return value;
        }

        public string TypeName { get { return typeof(OracleService).Name; } }

        public DbContextOptions Options { get; set; }

        public Assembly Assembly { get; set; }

        public string ConnectionString { get; set; }
    }
}
