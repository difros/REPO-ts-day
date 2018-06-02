using GQ.Core.service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace GQ.Sql.SQLServer
{
    public static class SQLServerServiceDBConfigurationExtension
    {

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value, ServiceLifetime contextLifetime) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        public static SQLServerServiceDBConfiguration SetOption(this SQLServerServiceDBConfiguration value, Func<DbContextOptionsBuilder, DbContextOptionsBuilder> function)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<SQLServerService>();
            builder = function(builder);
            value.Options = builder.Options;
            return value;
        }

        public static SQLServerServiceDBConfiguration SetOption(this SQLServerServiceDBConfiguration value, DbContextOptions option)
        {
            value.Options = option;
            return value;
        }

        public static SQLServerServiceDBConfiguration SetAssembly(this SQLServerServiceDBConfiguration value, Assembly assembly)
        {
            value.Assembly = assembly;
            return value;
        }
    }

    public class SQLServerServiceDBConfiguration : IServiceDBConfigure
    {
        public static SQLServerServiceDBConfiguration SetConnectionString(string ConnectionString)
        {
            var value = new SQLServerServiceDBConfiguration();
            value.ConnectionString = ConnectionString;

            ServicesContainer.AddSingleton<SQLServerServiceDBConfiguration>(value);

            ServicesContainer.AddTransient<SQLServerService>();

            return value;
        }

        public string TypeName { get { return typeof(SQLServerService).Name; } }

        public DbContextOptions Options { get; set; }

        public Assembly Assembly { get; set; }

        public string ConnectionString { get; set; }
    }
}
