using GQ.Core.service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace GQ.Sql.MySQL
{
    public static class MySQLServiceDBConfigurationExtension
    {

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value, ServiceLifetime contextLifetime) where TContext : DbContext;

        //public static ServiceDBConfigurationEntityFramework SetOption<TContext>( this ServiceDBConfigurationEntityFramework value,  Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped) where TContext : DbContext;

        public static MySQLServiceDBConfiguration SetOption(this MySQLServiceDBConfiguration value, Func<DbContextOptionsBuilder, DbContextOptionsBuilder> function)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<MySQLService>();
            builder = function(builder);
            value.Options = builder.Options;
            return value;
        }

        public static MySQLServiceDBConfiguration SetOption(this MySQLServiceDBConfiguration value, DbContextOptions option)
        {
            value.Options = option;
            return value;
        }

        public static MySQLServiceDBConfiguration SetAssembly(this MySQLServiceDBConfiguration value, Assembly assembly)
        {
            value.Assembly = assembly;
            return value;
        }
    }

    public class MySQLServiceDBConfiguration : IServiceDBConfigure
    {
        public static MySQLServiceDBConfiguration SetConnectionString(string ConnectionString)
        {
            var value = new MySQLServiceDBConfiguration();
            value.ConnectionString = ConnectionString;

            ServicesContainer.AddSingleton<MySQLServiceDBConfiguration>(value);

            ServicesContainer.AddTransient<MySQLService>();

            return value;
        }
        
        public string TypeName { get { return typeof(MySQLService).Name; } }

        public DbContextOptions Options { get; set; }

        public Assembly Assembly { get; set; }

        public string ConnectionString { get; set; }
    }
}
