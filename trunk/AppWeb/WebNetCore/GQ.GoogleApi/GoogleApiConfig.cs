using GQ.Core.service;
using System;

namespace GQ.GoogleApi
{
    public class GoogleApiConfig
    {
        public string KeyAccess { get; set; }

        public static GoogleApiConfig Configure(Func<GoogleApiConfig, GoogleApiConfig> config)
        {
            GoogleApiConfig c = new GoogleApiConfig();
            c = config(c);

            ServicesContainer.AddSingleton<GoogleApiConfig>(c);

            return c;
        }
    }
}
