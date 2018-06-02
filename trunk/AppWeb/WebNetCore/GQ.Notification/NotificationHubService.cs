using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace GQ.Notification
{
    public class NotificationHubService
    {
        #region Connection
        public string NotificationConnectionString { get; set; }

        public string NotificationHubPath { get; set; }

        private NotificationHubClient _hub = null;

        public NotificationHubClient hub
        {
            get
            {
                if (_hub == null)
                {
                    _hub = NotificationHubClient.CreateClientFromConnectionString(NotificationConnectionString, NotificationHubPath);
                }
                return _hub;
            }
        }
        #endregion

        /*
         * 
   2017-11-29 14:43:23,231 [22] ERROR GeminusQhom - GQ.Notification.NotificationHubService.SendNotificationIOS
   at Microsoft.Azure.NotificationHubs.Messaging.Configuration.KeyValueConfigurationManager.CreateNameValueCollectionFromConnectionString(String connectionString)
   at Microsoft.Azure.NotificationHubs.NotificationHubManager..ctor(String connectionString, String notificationHubPath)
   at Microsoft.Azure.NotificationHubs.NotificationHubClient..ctor(String connectionString, String notificationHubPath)
   at GQ.Notification.NotificationHubService.get_hub()
         * */

        public void SendNotification(NotificationMessage Notification)
        {
            try
            {
                SendNotificationAndroid(Notification, Notification.id);
                SendNotificationIOS(Notification, Notification.id);

            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error("GQ.Notification.NotificationHubService.SendNotification", e);
            }
        }

        public async void SendNotificationAndroid(NotificationMessage Notification, string tag)
        {
            try
            {
                Notification.id = tag;
                var result = await hub.SendGcmNativeNotificationAsync(Notification.ToAndroid(), tag);
            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error("GQ.Notification.NotificationHubService.SendNotificationAndroid", e);
                Log.Log.GetLog().Info("- TAG: " + tag + " , Notification:" + Notification.ToAndroid());
            }
        }

        public async void SendNotificationIOS(NotificationMessage Notification, string tag)
        {
            try
            {
                Notification.id = tag;
                var result = await hub.SendAppleNativeNotificationAsync(Notification.ToIOS(), tag);
            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error("GQ.Notification.NotificationHubService.SendNotificationIOS", e);
                Log.Log.GetLog().Info("- TAG: " + tag + " , Notification:" + Notification.ToIOS());
            }
        }

        public async void unRegisterAllTags(string RegistrationId)
        {
            try
            {
                var registration = await hub.GetRegistrationAsync<RegistrationDescription>(RegistrationId);

                registration.Tags.Clear();

                await hub.UpdateRegistrationAsync(registration);
            }
            catch
            {

            }

        }
    }

    #region Clases Auxiliares
    public class AlertIOS
    {
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string title_loc_key { get; set; }
        public string[] title_loc_args { get; set; }
        public string action_loc_key { get; set; }
        public string loc_key { get; set; }
        public string[] loc_args { get; set; }
        public string launch_image { get; set; }

    }

    public class APS
    {
        public APS()
        {

        }
        public APS(NotificationMessage noti)
        {
            this.sound = noti.sound;
            this.badge = noti.badge;
            this.url = noti.url;
            this.content_available = noti.content_available;
            this.category = noti.category;

            this.alert.id = noti.id;
            this.alert.title = noti.title;
            this.alert.body = noti.body;
            this.alert.title_loc_key = noti.title_loc_key;
            this.alert.title_loc_args = noti.title_loc_args;
            this.alert.action_loc_key = noti.action_loc_key;
            this.alert.loc_key = noti.loc_key;
            this.alert.loc_args = noti.loc_args;
            this.alert.launch_image = noti.launch_image;
        }

        public AlertIOS alert { get; set; } = new AlertIOS();
        public string sound { get; set; } = "default";
        public long badge { get; set; } = 0;
        public string url { get; set; } = "";
        public long content_available { get; set; } = 1;
        public string category { get; set; }

    }

    public class NotificationMessage
    {
        //Configuracion
        public string sound { get; set; } = "default";
        public long badge { get; set; } = 0;
        public string url { get; set; } = "";
        public long content_available { get; set; } = 1;
        public string category { get; set; }

        //Mensaje
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string title_loc_key { get; set; }
        public string[] title_loc_args { get; set; }
        public string action_loc_key { get; set; }
        public string loc_key { get; set; }
        public string[] loc_args { get; set; }
        public string launch_image { get; set; }
        public Hashtable DataExtra { get; set; } = new Hashtable();

        public string TIPO { get; set; }

        public string ToAndroid()
        {
            Hashtable Data = new Hashtable();
            Data.Add("data", this);
            return JsonConvert.SerializeObject(Data);
        }

        public string ToIOS()
        {
            Hashtable Data = new Hashtable();
            Data.Add("aps", new APS(this));
            Data.Add("data", DataExtra);

            string rsult = JsonConvert.SerializeObject(Data);

            rsult = rsult.Replace("loc_key", "loc-key");
            rsult = rsult.Replace("loc_args", "loc-args");
            rsult = rsult.Replace("launch_image", "launch-image");
            rsult = rsult.Replace("title_loc_key", "title-loc-key");
            rsult = rsult.Replace("title_loc_args", "title-loc-args");
            rsult = rsult.Replace("action_loc_key", "action-loc-key");
            rsult = rsult.Replace("content_available", "content-available");

            return rsult;
        }

        public string ToWindows()
        {
            Hashtable Data = new Hashtable();
            Data.Add("data", this);
            return JsonConvert.SerializeObject(Data);
        }
    }

    #endregion
}