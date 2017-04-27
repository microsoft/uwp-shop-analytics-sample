using Newtonsoft.Json;
using RidoShop.Model;
using ShopEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RidoShop.Client.BackendServices
{
    public class ShopEventsProxy
    {
        public static async Task<IEnumerable<ShopSensorEvent>> GetAllEvents()
        {
            HttpClient http = new HttpClient() { BaseAddress = new Uri(AppConfig.RidoShopServerUrl) };
            var response = await http.GetAsync("api/event");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ShopSensorEvent>>(content);
        }
    }
}
