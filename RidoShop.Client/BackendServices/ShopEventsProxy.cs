using Newtonsoft.Json;
using RidoShop.Model;
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
        
        static HttpClient http = new HttpClient() { BaseAddress = new Uri(AppConfig.RidoShopServerUrl) };

        public static async Task<IEnumerable<ShopSensorEvent>> GetAllEvents()
        {
            var response = await http.GetAsync("api/ShopSensor");
            var content = await response.Content.ReadAsStringAsync();
            return ShopSensorEvent.FromJson(content);            
        }

        public static async Task<IEnumerable<ShopSensorEvent>> GetTodayEvents()
        {
            var response = await http.GetAsync("api/ShopSensor?daysSince=1");
            var content = await response.Content.ReadAsStringAsync();
            return ShopSensorEvent.FromJson(content);
        }

        public static async Task<int> GetTotalEventsToday()
        {
            var response = await http.GetAsync("api/ShopSensor/total?daysSince=1");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(content);
        }

        public static async Task<DateTime> GetLastTime()
        {
            var response = await http.GetAsync("api/ShopSensor/last?daysSince=30");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DateTime>(content);
        }

        public static async Task<IEnumerable<DayStats>> GetWeeklyData()
        {
            var response = await http.GetAsync("api/ShopSensor/ByDay");
            var content = await response.Content.ReadAsStringAsync();
            return DayStats.FromJson(content);            
        }

        public static async Task<IEnumerable<HourStats>> GetHourlyData()
        {
            var response = await http.GetAsync("api/ShopSensor/ByHour");
            var content = await response.Content.ReadAsStringAsync();
            return HourStats.FromJson(content);
        }
    }
}
