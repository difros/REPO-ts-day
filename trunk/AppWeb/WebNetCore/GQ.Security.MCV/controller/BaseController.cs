using GQ.Core.utils;
using GQ.Data;
using GQ.Data.exception;
using GQ.Security.exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Reflection;

namespace GQ.Security.MCV.controller
{
    public class BaseController : Controller
    {

        public void LogExecuting(ActionExecutingContext context)
        {
            Log.Log.GetLog().Debug(this, context.RouteData.Values["action"].ToString() + " | " + Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments));
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

            if (IsActionResult(method.ReturnType) == false)
            {
                if (IsReturnData(method.ReturnType) == false)
                {
                    if (context.Result == null)
                    {
                        if (context.Exception != null)
                        {
                            context.Result = Json(new ReturnData { isError = true, data = new GenericError(context.Exception) });
                            Log.Log.GetLog().Error(this, method.Name, context.Exception);
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
                                if (((ObjectResult)context.Result).Value is ReturnData)
                                {
                                    context.Result = Json(((ObjectResult)context.Result).Value);
                                }
                                else
                                {
                                    context.Result = Json(new ReturnData { data = ((ObjectResult)context.Result).Value });
                                }
                            }
                            catch (Exception ex)
                            {
                                context.Result = Json(new ReturnData { isError = true, data = new GenericError(ex) });
                                Log.Log.GetLog().Error(this, method.Name, ex);
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

            if (method.Equals("Index"))
                method = null;

            if (Security.UsuarioLogueado<object>() == null && Security.IsExcludeController(ClassUtils.getNameObject(this)) == false)
            {
                if (IsActionResult(value.ReturnType))
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
                if (IsActionResult(value.ReturnType))
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
        /// <param name="returnType"></param>
        /// <returns></returns>
        protected bool IsReturnData(Type returnType)
        {
            bool result = false;

            if (returnType.Equals(typeof(ReturnData)) || (returnType.BaseType != null && returnType.BaseType.Equals(typeof(ReturnData))))
            {
                result = true;
            }
            else if (returnType.IsGenericType)
            {

                foreach (var item in returnType.GenericTypeArguments)
                {
                    result = result || IsReturnData(item);
                    if (result)
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        protected bool IsActionResult(Type returnType)
        {
            bool result = false;

            if (returnType.Equals(typeof(IActionResult)))
            {
                result = true;
            }
            else if (returnType.IsGenericType)
            {
                foreach (var item in returnType.GenericTypeArguments)
                {
                    result = result || IsActionResult(item);
                    if (result)
                        break;
                }
            }
            else
            {
                result = returnType.GetInterface("IActionResult") != null;
            }
            return result;
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
