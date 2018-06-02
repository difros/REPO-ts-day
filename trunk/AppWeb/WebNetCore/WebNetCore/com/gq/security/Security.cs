using GQ.Core.utils;
using GQ.Security;
using GQ.Security.exception;
using GQ.Sql.MySQL;
using GQService.com.gq.menu;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebNetCore.Data.constantes;
using WebNetCore.Data.sql.domain;
using WebNetCore.Data.sql.dto;

namespace WebNetCore.com.gq.security
{
    public static class Security
    {
        #region ROLES DEFINICION

        /// <summary>
        /// DEFINICION DE ROL ADMINISTRADOR
        /// </summary>
        public const string ROL_ADMI = "rol_administrador_general";

        /// <summary>
        /// DEFINICION DE ROL ADMINISTRADOR DE EMPRESAS
        /// </summary>
        public const string ROL_ADMI_EMPRESA = "rol_administrador";

        /// <summary>
        /// DEFINICION DE ROL MOBILE
        /// </summary>
        public const string ROL_MOBILE = "rol_usuario";


        public static readonly string[] ROLES = new string[] { ROL_ADMI, ROL_MOBILE, ROL_ADMI_EMPRESA};

        #endregion

        #region MENUES PADRES DEFINICION

        public const string MENU_CONFIG_ID = "90-00-00";
        public const string MENU_ESTADISTICAS_ID = "30-00-00";

        #endregion

        #region SEGURIDAD

        private static Dictionary<Type, Dictionary<string, SecurityDescription>> TypeMethodSecurityDescription = new Dictionary<Type, Dictionary<string, SecurityDescription>>();

        private static SecurityDescription GetTypeMethodSecurityDescription(Type type, string method)
        {
            if (!TypeMethodSecurityDescription.ContainsKey(type))
            {
                TypeMethodSecurityDescription.Add(type, new Dictionary<string, SecurityDescription>());
            }
            if (!TypeMethodSecurityDescription[type].ContainsKey(method))
            {
                SecurityDescription attribute = null;
                var methodFind = ClassUtils.GetMethod(type, method);
                if (methodFind.Length > 0)
                {
                    attribute = (SecurityDescription)methodFind[0].GetCustomAttribute(typeof(SecurityDescription), true);
                }
                TypeMethodSecurityDescription[type].Add(method, attribute);
            }

            return TypeMethodSecurityDescription[type][method];
        }

        private static Dictionary<Type, SecurityDescription> TypeSecurityDescription = new Dictionary<Type, SecurityDescription>();

        private static SecurityDescription GetTypeSecurityDescription(Type type)
        {
            if (!TypeSecurityDescription.ContainsKey(type))
            {
                SecurityDescription attribute = (SecurityDescription)type.GetCustomAttribute(typeof(SecurityDescription), true);
                TypeSecurityDescription.Add(type, attribute);
            }
            return TypeSecurityDescription[type];
        }

        public static GQ_UsuariosDto usuarioLogueado
        {
            get
            {
                return GQ.Security.Security.UsuarioLogueado<GQ_UsuariosDto>();
            }
        }

        public static long UsuarioId()
        {
            return usuarioLogueado != null ? Security.usuarioLogueado.Id.Value : -1;
        }

        public static string getNameObject(object value)
        {
            return ClassUtils.getNameObject(value);
        }

        public static bool hasPermission(object value, string method = null, bool returnException = true, params object[] parameters)
        {
            if (value == null)
                return false;

            bool result = true;
            bool isLogued = Security.usuarioLogueado != null;

            Type objectType = (value is Type) ? (Type)value : value.GetType();

            SecurityDescription attribute = GetTypeSecurityDescription(objectType);

            if (attribute != null)
            {
                if (attribute.Estado == SecurityDescription.SeguridadEstado.Activo)
                {
                    result = result && _hasPermission(value, null, returnException);
                }
                else if (attribute.Estado == SecurityDescription.SeguridadEstado.SoloLogueo && isLogued == false)
                {
                    result = false;
                }

                if (result && method != null)
                {
                    attribute = GetTypeMethodSecurityDescription(objectType, method);
                    if (attribute != null)
                    {
                        if (attribute.Estado == SecurityDescription.SeguridadEstado.Activo)
                        {
                            result = result && _hasPermission(value, method, returnException);
                        }
                        else if (attribute.Estado == SecurityDescription.SeguridadEstado.SoloLogueo && isLogued == false)
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }

            if (result == false && returnException)
            {
                throw new SecurityException(value, method);
            }

            return result;
        }

        private static List<GQ_Accesos> _AccesosCollection;
        public static List<GQ_Accesos> AccesosCollection
        {
            get
            {
                if (_AccesosCollection == null)
                    _AccesosCollection = MySQLService.Instance.GetSession<GQ_Accesos>().findBy(x => true).ToList();
                return _AccesosCollection;
            }
        }

        private static bool _hasPermission(object value, string method, bool returnException = true)
        {
            bool result = false;

            GQ_Accesos accesos = AccesosCollection.Where(x => x.ClassName == getNameObject(value) && x.MethodName == method).FirstOrDefault();

            var lr = MySQLService.Instance.GetSession<GQ_Usuarios_Perfiles>().findBy(x => x.UsuarioId == usuarioLogueado.Id.Value).Select(x => (long?)x.PerfilId).ToList();
            var roles = MySQLService.Instance.GetSession<GQ_Perfiles>().findBy(x => lr.Contains(x.Id) && x.Estado == Constantes.ESTADO_ACTIVO).ToList();

            if (roles.Count > 0)
            {
                var notFind = true;
                result = true;
                foreach (var rol in roles)
                {
                    var ar = MySQLService.Instance.GetSession<GQ_Perfiles_Accesos>().findBy(y => y.PerfilId == rol.Id && y.AccesoId == accesos.Id).FirstOrDefault();
                    if (ar != null)
                    {
                        result = result && ar.Permite;
                        notFind = false;
                    }
                }
                if (notFind)
                    result = result && false;
            }

            if (result == false && returnException)
            {
                throw new SecurityException(value, method);
            }
            return result;
        }

        public static bool hasControllerPermission(string value)
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                string accion = "";
                string controller = "";

                var array = value.Split('/');

                if (array.Length > 0)
                    controller = "IOTWebDrAromasApp.Controllers." + array[0] + "Controller"; ///TODO Tengo que ver esta Parte del Cotroller

                if (array.Length > 1)
                    accion = array[1];

                bool result = false;

                var accesos = AccesosCollection.Where(x => x.ClassName == controller && ((accion != null && x.MethodName == accion) || (x.MethodName == null))).FirstOrDefault();
                if (accesos != null)
                {
                    var lr = MySQLService.Instance.GetSession<GQ_Usuarios_Perfiles>().findBy(x => x.UsuarioId == usuarioLogueado.Id.Value).Select(x => (long?)x.PerfilId).ToList();
                    var roles = MySQLService.Instance.GetSession<GQ_Perfiles>().findBy(x => lr.Contains(x.Id) && x.Estado == Constantes.ESTADO_ACTIVO).ToList();

                    if (roles.Count > 0)
                    {
                        var notFind = true;
                        result = true;
                        foreach (var rol in roles)
                        {
                            var ar = MySQLService.Instance.GetSession<GQ_Perfiles_Accesos>().findBy(y => y.PerfilId == rol.Id && y.AccesoId == accesos.Id).FirstOrDefault();
                            if (ar != null)
                            {
                                result = result && ar.Permite;
                                notFind = false;
                            }
                        }
                        if (notFind)
                            result = result && false;
                    }
                }
                return result;
            }
            else
            {
                return true;
            }
        }

        #endregion

        public static Dictionary<string, GQ_PerfilesDto> roles = new Dictionary<string, GQ_PerfilesDto>();

        public static void CreateAccessSecurity(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Assembly assembly = typeof(Security).Assembly;

                SecurityDescription attribute = null;
                MenuDescription menuAttribute = null;

                Dictionary<MenuDescription, object> menues = new Dictionary<MenuDescription, object>();
                ArrayList items = new ArrayList();

                GQ_AccesosDto itemClass = null;
                GQ_AccesosDto itemMethod = null;

                ///
                ///
                /// CREACION DE ROLES
                ///
                ///

                if (ROLES.Length > 0)
                {
                    for (int i = 0; i < ROLES.Length; i++)
                    {
                        if (roles.ContainsKey(ROLES[i]) == false)
                            roles.Add(ROLES[i], null);
                    }
                }

                var RolColl = MySQLService.Instance.GetSession<GQ_Perfiles>().findBy(x => true).ToList();

                for (int i = 0; i < roles.Keys.Count; i++)
                {
                    string key = roles.Keys.ElementAt(i);
                    var rol = RolColl.Where(x => x.KeyName == key).FirstOrDefault();

                    if (rol == null)
                    {
                        rol = new GQ_Perfiles();
                        rol.Nombre = key;
                        rol.KeyName = key;
                        rol.Estado = Constantes.ESTADO_ACTIVO;
                        rol.Creado = DateTime.Now;
                        rol.Modificado = DateTime.Now;

                        MySQLService.Instance.GetSession<GQ_Perfiles>().Insert(rol);
                    }

                    roles[key] = new GQ_PerfilesDto(rol);
                }

                ///
                ///
                /// CREACION DE ACCESOS
                ///
                ///

                var elementsType = (from iface in assembly.GetTypes()
                                    where iface.Namespace != null && iface.Namespace.Contains("Controller")
                                    select iface);

                foreach (Type item in elementsType)
                {
                    menuAttribute = (MenuDescription)item.GetCustomAttribute(typeof(MenuDescription), true);
                    if (menuAttribute != null)
                    {
                        menues.Add(menuAttribute, item);
                    }
                    attribute = (SecurityDescription)item.GetCustomAttribute(typeof(SecurityDescription), true);

                    if (attribute != null)
                    {
                        itemClass = new GQ_AccesosDto();
                        itemClass.ClassName = getNameObject(item);

                        if (attribute.Estado == SecurityDescription.SeguridadEstado.Activo)
                        {
                            itemClass.MethodName = null;
                            itemClass.Nombre = attribute.Name;
                            itemClass.extra = attribute;

                            //obtenerProfiles(attribute, roles);

                            items.Add(itemClass);
                        }

                        foreach (MethodInfo method in item.GetMethods())
                        {
                            menuAttribute = (MenuDescription)method.GetCustomAttribute(typeof(MenuDescription), true);
                            if (menuAttribute != null)
                            {
                                menues.Add(menuAttribute, method);
                            }

                            attribute = (SecurityDescription)method.GetCustomAttribute(typeof(SecurityDescription), true);
                            if (attribute != null && attribute.Estado == SecurityDescription.SeguridadEstado.Activo)
                            {
                                itemMethod = new GQ_AccesosDto();
                                itemMethod.ClassName = itemClass.ClassName;
                                itemMethod.MethodName = method.Name;
                                itemMethod.Nombre = attribute.Name;
                                itemMethod.extra = attribute;

                                //obtenerProfiles(attribute, roles);

                                items.Add(itemMethod);
                            }
                        }
                    }
                }

                var AccesoColl = MySQLService.Instance.GetSession<GQ_Accesos>().findBy(x => true).ToList();

                foreach (GQ_AccesosDto ii in items)
                {
                    var data = AccesoColl.Where(x => x.ClassName == ii.ClassName && x.MethodName == ii.MethodName).FirstOrDefault();
                    if (data == null)
                    {
                        data = new GQ_Accesos();
                        data.ClassName = ii.ClassName;
                        data.MethodName = ii.MethodName;
                        data.Nombre = ii.Nombre;

                        MySQLService.Instance.GetSession<GQ_Accesos>().Insert(data);
                    }
                    else
                    {
                        data.Nombre = ii.Nombre;
                        MySQLService.Instance.GetSession<GQ_Accesos>().Update(data);
                    }

                    attribute = (SecurityDescription)ii.extra;
                    if (attribute.Perfiles != null && attribute.Perfiles.Length > 0)
                    {
                        for (int i = 0; i < attribute.Perfiles.Length; i++)
                        {
                            var rol = roles[attribute.Perfiles[i]];
                            var ar = rol.PerfilesAccesos.Where(x => x.AccesoId == data.Id).FirstOrDefault();

                            if (ar == null)
                            {
                                ar = new GQ_Perfiles_AccesosDto { PerfilId = rol.Id.Value, AccesoId = data.Id.Value, Permite = true };
                                rol.PerfilesAccesos.Add(ar);
                                rol.Modificado = DateTime.Now;
                            }
                        }
                    }
                }

                foreach (var item in roles.Values)
                {
                    foreach (var itemA in item.PerfilesAccesos)
                    {
                        var itemInsert = itemA.GetEntity();
                        MySQLService.Instance.GetSession<GQ_Perfiles_Accesos>().Update(itemInsert);
                    }

                }

                ///
                ///
                /// CREACION DE USUARIO ADMINISTRADOR
                ///
                ///
                var UsuarioColl = MySQLService.Instance.GetSession<GQ_Usuarios>().findBy(x => true);

                var usuario = UsuarioColl.Where(x => x.NombreUsuario == "admin").FirstOrDefault();

                if (usuario == null)
                {
                    usuario = new GQ_Usuarios
                    {
                        Nombre = "Admin",
                        Apellido = "Admin",
                        NombreUsuario = "admin",
                        Password = Constantes.Encriptar("admin1234"),
                        RequiereContraseña = false,
                        Email = "",
                        Estado = Constantes.ESTADO_ACTIVO,
                        Creado = DateTime.Now,
                        Modificado = DateTime.Now
                    };

                    MySQLService.Instance.GetSession<GQ_Usuarios>().Insert(usuario);

                    var up = new GQ_Usuarios_Perfiles
                    {
                        UsuarioId = usuario.Id.Value,
                        PerfilId = roles[Security.ROL_ADMI].Id.Value,
                    };

                    MySQLService.Instance.GetSession<GQ_Usuarios_Perfiles>().Insert(up);
                }

                ///
                ///
                /// CREACION DE MENUES
                ///
                ///

                var MenuColl = MySQLService.Instance.GetSession<GQ_Menues>().findBy(x => true).ToList();

                var menu = MenuColl.Where(x => x.KeyName == MENU_CONFIG_ID).FirstOrDefault();

                if (menu == null)
                {
                    menu = new GQ_Menues();
                    menu.MenuPosition = MENU_CONFIG_ID;
                    menu.KeyName = MENU_CONFIG_ID;
                    menu.Nombre = "menu_configuracion";
                    menu.MenuIcono = "fa fa-fw fa-cogs";
                    menu.Creado = DateTime.Now;
                    menu.Modificado = DateTime.Now;
                    menu.Estado = Constantes.ESTADO_ACTIVO;
                    MySQLService.Instance.GetSession<GQ_Menues>().Insert(menu);
                }

                menu = MenuColl.Where(x => x.KeyName == MENU_ESTADISTICAS_ID).FirstOrDefault();

                if (menu == null)
                {
                    menu = new GQ_Menues();
                    menu.MenuPosition = MENU_ESTADISTICAS_ID;
                    menu.KeyName = MENU_ESTADISTICAS_ID;
                    menu.Nombre = "menu_tablero_control";
                    menu.MenuIcono = "fa fa-fw fa-dashboard";
                    menu.Creado = DateTime.Now;
                    menu.Modificado = DateTime.Now;
                    menu.Estado = Constantes.ESTADO_ACTIVO;
                    MySQLService.Instance.GetSession<GQ_Menues>().Insert(menu);
                }

                foreach (var m in menues.Keys)
                {
                    menu = MenuColl.Where(x => x.KeyName == m.Id).FirstOrDefault();
                    if (menu == null)
                    {
                        var obj = menues[m];

                        menu = new GQ_Menues();
                        menu.MenuPosition = m.Id;
                        menu.KeyName = m.Id;
                        menu.Nombre = m.Description;
                        menu.MenuIcono = "";
                        menu.MenuPadre = m.IdParent;
                        menu.Creado = DateTime.Now;
                        menu.Modificado = DateTime.Now;
                        menu.Estado = Constantes.ESTADO_ACTIVO;

                        if (obj is Type)
                            menu.MenuUrl = ((Type)obj).Name.Replace("Controller", "");
                        else if (obj is MethodInfo)
                            menu.MenuUrl = ((MethodInfo)obj).ReflectedType.Name.Replace("Controller", "") + @"/" + ((MethodInfo)obj).Name;

                        MySQLService.Instance.GetSession<GQ_Menues>().Insert(menu);
                    }
                }
            }
        }
    }
}


