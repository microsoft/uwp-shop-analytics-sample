using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidoShop.Client
{
    class AppConfig
    {
        public static string RidoShopServerUrl => "http://localhost:58032";
        public static string PushKey => "Endpoint=sb://<YourAzureNotification>.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=<YOURKEY>";
    }
}
