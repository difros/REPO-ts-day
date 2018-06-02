using GQService.com.gq.data;
using GQService.com.gq.exception;
using GQService.com.gq.log;
using GQService.com.gq.security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GQService.com.gq.controller
{
    public class BaseController : Controller
    {

        public void LogExecuting(ActionExecutingContext context)
        {
            Log.Debug(this, context.RouteData.Values["action"].ToString() + " | " + Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LogExecuting(context);

            MethodInfo method = null;
            method = GetMethod(context.RouteData.Values["action"].ToString());
            
            IActionResult result = hasPermission(method, context);
            if (result != null)
            {
                context.Result = result;
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            MethodInfo method = GetMethod(context.RouteData.Values["action"].ToString());

            if (method.ReturnType.Equals(typeof(IActionResult)) == false)
            {
                if (method.ReturnType.Equals(typeof(ReturnData)) == false)
                {
                    if (context.Result == null)
                    {
                        if (context.Exception != null)
                        {
                            context.Result = Json(new ReturnData { isError = true, data = new GenericError(context.Exception) });
                            Log.Error(this, method.Name, context.Exception);
                        }
                        else
                        {
                            context.Result = Json(new ReturnData { data = null });
                        }

                    }
                    else
                    {
                        if (context.Result is ObjectResult)
                        {
                            try
                            {
                                context.Result = Json(new ReturnData { data = ((ObjectResult)context.Result).Value });
                            }
                            catch (Exception ex)
                            {
                                context.Result = Json(new ReturnData { isError = true, data = new GenericError(ex) });
                                Log.Error(this, method.Name, ex);
                            }
                        }
                    }
                }
            }
            ///
            /// Mantiene el Idioma seleccionado por cookies
            ///
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            context.HttpContext.Response.Cookies.Append(".AspNetCore.Culture", "c=" + System.Globalization.CultureInfo.CurrentCulture.Name + "|uic=" + System.Globalization.CultureInfo.CurrentCulture.Name, options);

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isPartial"></param>
        /// <returns></returns>
        protected IActionResult hasPermission(MethodInfo value, ActionExecutingContext context)
        {
            string method = value.Name;

            //SecurityDescription attributeClass = (SecurityDescription)this.GetType().GetTypeInfo().GetCustomAttribute(typeof(SecurityDescription), true);
            //SecurityDescription attributeMethod = (SecurityDescription)value.GetCustomAttribute(typeof(SecurityDescription), true);

            if (method.Equals("Index"))
                method = null;

            if (Security.usuarioLogueado<object>() == null && Security.IsExcludeController(Security.getNameObject(this)) == false)
            {
                if (value.ReturnType.Equals(typeof(IActionResult)))
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return Json(new ReturnData { isError = true, isSecurity = true, data = new SecurityException(this, method) });
                }
            }

            if (!Security.hasPermission(this, method, false, context))
            {
                if (value.ReturnType.Equals(typeof(IActionResult)))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(new ReturnData { isError = true, isSecurity = true, data = new SecurityException(this, method) });
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected MethodInfo GetMethod(string name)
        {
            Type type = this.GetType();
            var method = (from iface in type.GetMethods()
                          where iface.Name.Equals(name)
                          select iface).ToArray();

            return method[0];

        }
    }
}
