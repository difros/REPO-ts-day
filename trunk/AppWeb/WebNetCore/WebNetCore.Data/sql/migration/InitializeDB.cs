using GQ.Sql.MySQL;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using WebNetCore.Data.constantes;

namespace WebNetCore.Data.sql.migrations
{
    [DbContext(typeof(MySQLService))]
    [Migration("19000101TK00000")]
    public partial class InitializeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
            name: "GQ_Accesos",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                Nombre = table.Column<string>(nullable: false, maxLength: 45),
                ClassName = table.Column<string>(nullable: false, maxLength: 255),
                MethodName = table.Column<string>(nullable: true, maxLength: 255),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccesoId", x => x.Id);
            });

            migrationBuilder.CreateIndex("IDX_GQ_Accesos_Class_Metodo", "GQ_Accesos", new string[] { "ClassName", "MethodName" });

            migrationBuilder.CreateTable(
            name: "GQ_Perfiles",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                Nombre = table.Column<string>(nullable: false, maxLength: 45),
                KeyName = table.Column<string>(nullable: true, maxLength: 50),
                Estado = table.Column<string>(nullable: true, maxLength: 1, defaultValue: Constantes.ESTADO_ACTIVO),
                Creado = table.Column<DateTime>(nullable: true),
                CreadoPor = table.Column<long>(nullable: true),
                Modificado = table.Column<DateTime>(nullable: true),
                ModificadoPor = table.Column<long>(nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PerfilId", x => x.Id);
            });

            migrationBuilder.CreateTable(
            name: "GQ_Perfiles_Accesos",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                PerfilId = table.Column<long>(nullable: false),
                AccesoId = table.Column<long>(nullable: false),
                Permite = table.Column<bool>(nullable: false, defaultValue: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PerfilesAccesosId", x => x.Id);
            });

            migrationBuilder.CreateTable(
            name: "GQ_Usuarios",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                NombreUsuario = table.Column<string>(nullable: false, maxLength: 128),
                Nombre = table.Column<string>(nullable: false, maxLength: 128),
                Apellido = table.Column<string>(nullable: false, maxLength: 128),
                Email = table.Column<string>(nullable: false, maxLength: 128),
                Password = table.Column<string>(nullable: false, maxLength: 128),
                RequiereContraseña = table.Column<bool>(nullable: false, defaultValue: false),
                EmpresaId = table.Column<string>(nullable: true, maxLength: 48),
                Estado = table.Column<string>(nullable: true, maxLength: 1, defaultValue: Constantes.ESTADO_ACTIVO),
                Creado = table.Column<DateTime>(nullable: true),
                CreadoPor = table.Column<long>(nullable: true),
                Modificado = table.Column<DateTime>(nullable: true),
                ModificadoPor = table.Column<long>(nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UsuarioId", x => x.Id);
                table.UniqueConstraint("UC_NombreUsuario", x => x.NombreUsuario);
            });

            migrationBuilder.CreateTable(
            name: "GQ_Usuarios_Perfiles",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                UsuarioId = table.Column<long>(nullable: false),
                PerfilId = table.Column<long>(nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UsuarioPerfilId", x => x.Id);
            });

            migrationBuilder.CreateTable(
            name: "GQ_Menues",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false).Annotation("MySql:ValueGeneratedOnAdd", true),
                Nombre = table.Column<string>(nullable: false, maxLength: 128),
                MenuPosition = table.Column<string>(nullable: false, maxLength: 128),
                KeyName = table.Column<string>(nullable: true, maxLength: 128),
                MenuPadre = table.Column<string>(nullable: true, maxLength: 128),
                MenuUrl = table.Column<string>(nullable: true, maxLength: 128),
                MenuIcono = table.Column<string>(nullable: true, maxLength: 128),
                Estado = table.Column<string>(nullable: true, maxLength: 1, defaultValue: Constantes.ESTADO_ACTIVO),
                Creado = table.Column<DateTime>(nullable: true),
                CreadoPor = table.Column<long>(nullable: true),
                Modificado = table.Column<DateTime>(nullable: true),
                ModificadoPor = table.Column<long>(nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MenuesId", x => x.Id);
            });
        }
    }
}