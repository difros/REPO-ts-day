using GQ.Core.service;
using GQ.Data.dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace GQ.Sql.Oracle.UnitTest
{
    [TestClass]
    public class TestOracleDb
    {
        [TestMethod]
        public void TestConection()
        {
            IHostingEnvironment Environment = new HostingEnvironment();
            ServicesContainer.AddHostingEnvironment(Environment);

            var Configuration = ServicesContainer.ConfigurationBuilder();

            ServicesContainer.AddServices(new Microsoft.Extensions.DependencyInjection.ServiceCollection());

            OracleServiceDBConfiguration.SetConnectionString("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.246.6.109)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=qhom11)));User Id=trz;Password=trz;")
                .SetAssembly(typeof(GQ_Usuarios).Assembly).SetOption((builder) =>
                {
                    builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    return builder;
                });

            var service = ServicesContainer.BuildServiceProvider();

            using (var scope = service.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<OracleService>().Migrate();
            }
        }


        public class HostingEnvironment : IHostingEnvironment
        {
            public string EnvironmentName { get; set; }
            public string ApplicationName { get; set; }
            public string WebRootPath { get; set; }
            public IFileProvider WebRootFileProvider { get; set; }
            public string ContentRootPath { get; set; } = @"D:\PROYECTOS\GQBase\code\web\net\src\trunk\AppWeb\WebNetCore\WebNetCore\";
            public IFileProvider ContentRootFileProvider { get; set; }
        }

        public class GQ_Usuarios : _GQ_Usuarios, IEntityOracle
        {
        }

        public class _GQ_Usuarios : IEntity
        {
            [Key]
            public virtual System.Int64? Id { get; set; }

            [MaxLength(128)]
            [Required]
            public virtual System.String NombreUsuario { get; set; }

            [MaxLength(128)]
            [Required]
            public virtual System.String Nombre { get; set; }

            [MaxLength(128)]
            [Required]
            public virtual System.String Apellido { get; set; }

            [MaxLength(128)]
            [Required]
            public virtual System.String Email { get; set; }

            [MaxLength(128)]
            [Required]
            public virtual System.String Password { get; set; }

            [Required]
            public virtual System.Boolean RequiereContraseña { get; set; }

            [MaxLength(48)]
            public virtual System.String EmpresaId { get; set; }

            [MaxLength(1)]
            public virtual System.String Estado { get; set; }

            public virtual System.DateTime? Creado { get; set; }

            public virtual System.Int64? CreadoPor { get; set; }

            public virtual System.DateTime? Modificado { get; set; }

            public virtual System.Int64? ModificadoPor { get; set; }


        }
    }
}
