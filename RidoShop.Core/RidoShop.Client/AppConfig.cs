using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidoShop.Client
{
    class AppConfig
    {
        public static string RidoShopServerUrl => "http://ridoshopserver.azurewebsites.net";
        public static string PushKey => "Endpoint=sb://photoseventsnh.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=hc0UfF534NCsHG+BgWQVnWvL5E9eTHGDx2I3PiITtOk=";
    }
}
