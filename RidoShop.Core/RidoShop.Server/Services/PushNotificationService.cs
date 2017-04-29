using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidoShop.Server.Services
{
    public class PushNotificationService
    {
        public static async Task<NotificationOutcome> TriggerPushNotification(bool eventType, string name, string key)
        {
            string windowsToastPayload;
            // Get the Notification Hubs credentials for the Mobile App.
            // Create the notification hub client.
            var hub = NotificationHubClient
                .CreateClientFromConnectionString(key, name);

            // Define a WNS payload
            if (eventType == true)
            {
                windowsToastPayload = GetPayload("Someone has entered the store");
            }
            else
            {
                windowsToastPayload = GetPayload("Someone has exited the store");                
            }

            return await hub.SendWindowsNativeNotificationAsync(windowsToastPayload);
        }

        private static  string GetPayload(string message)
        {
            string toastTitle = "RidoShop Event";
            string baseImage = "http://ridoshopserver.azurewebsites.net/StoreFrontImage.png";
            return $"<toast><visual><binding template=\"ToastImageAndText03\"><image id='1' src='{baseImage}' /><text id='1'>{toastTitle}</text><text id=\"2\">{message}</text></binding></visual></toast>";
        }
    }
}
