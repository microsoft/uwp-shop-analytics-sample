using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidoShop.Server
{

    public class DocDb
    {
        public string Uri { get; set; }
        public string Key { get; set; }
    }

    public class Notifications
    {
        public string HubName { get; set; }
        public string FullListener { get; set; }
    }

    public class AppConfig
    {

        public DocDb DocDb { get; set; }
        public Notifications Notifications { get; set; }


    }
}
