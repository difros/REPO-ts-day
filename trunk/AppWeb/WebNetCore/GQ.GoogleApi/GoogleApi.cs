using GQ.Core.service;
using GQ.Html.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GQ.GoogleApi
{
    public static class GoogleApi
    {
        private static GoogleApiConfig Config { get { return ServicesContainer.GetService<GoogleApiConfig>(); } } // string KeyAccess = "AIzaSyDk7RqOuOMnk-t5CO84dEU025-cLe0kpak";
        private static string URiApi = "https://maps.googleapis.com/";
        private static string UriMap = URiApi + "maps/api/";
        private static string UriTimeZone = UriMap + "timezone/";

        private static Int32 GetTimestamp { get { return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; } }

        public async static Task<dynamic> TimeZone(double lat, double lon)
        {
            var paramData = new Dictionary<string, string>();
            paramData.Add("location", lat.ToString().Replace(',', '.') + "," + lon.ToString().Replace(',', '.'));
            paramData.Add("timestamp", GetTimestamp.ToString());
            paramData.Add("key", Config.KeyAccess);
            HttpRest rest = new HttpRest();
            var data = await rest.GetString(UriTimeZone + "json", paramData);
            return JsonConvert.DeserializeObject(data);
        }
    }
}
