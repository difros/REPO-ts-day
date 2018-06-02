using GQ.Core.service;
using Microsoft.AspNetCore.Http;

namespace System.Web
{
    public static class HttpContext
    {
        static HttpContext() { }

        public static void Configure()
        {
            ServicesContainer.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static Microsoft.AspNetCore.Http.HttpContext Current => ServicesContainer.GetRequiredService<IHttpContextAccessor>().HttpContext;

    }
}
