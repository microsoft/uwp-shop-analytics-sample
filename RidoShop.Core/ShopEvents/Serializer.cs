using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ShopEvents
{
    public static class Serializer<T>
    {

        static DataContractJsonSerializer s = new DataContractJsonSerializer(
              typeof(T),
              new DataContractJsonSerializerSettings
              {
                  DateTimeFormat = new DateTimeFormat("yyyy-MM-ddThh:mm:ss")
              });

        public static T FromJson(string json)
        {
            using (var ms = new MemoryStream(UTF8Encoding.UTF8.GetBytes(json)))
            {
                ms.Position = 0;
                return (T)s.ReadObject(ms);
            }
        }

        public static string ToJson(object o)
        {            
            using (var ms = new MemoryStream())
            {
                s.WriteObject(ms, o);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
