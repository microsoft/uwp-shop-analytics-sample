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

    public class BaseProxy<T> where T : class
    {
        public static async Task<T> Get(string url)
        {
            HttpClient http = new HttpClient() { BaseAddress = new Uri(AppConfig.RidoShopServerUrl) };
            var response = await http.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

    }

    public class ShopEventsClient : BaseProxy<IEnumerable<ShopSensorEvent>>
    {
        public static async Task<IEnumerable<ShopSensorEvent>> GetAllEvents()
        {
            return await Get("api/ShopSensor");
        }

        public static async Task<IEnumerable<ShopSensorEvent>> GetLastWeekEvents()
        {
            return await Get("api/ShopSensor?DaysSince=7");
        }

        public static async Task<IEnumerable<ShopSensorEvent>> GetLastMonthEvents()
        {
            return await Get("api/ShopSensor?DaysSince=30");
        }
    }


    public class ShopEventsProxy
    {
        
        static HttpClient http = new HttpClient() { BaseAddress = new Uri(AppConfig.RidoShopServerUrl) };

        public static async Task<IEnumerable<ShopSensorEvent>> GetAllEvents()
        {
            var response = await http.GetAsync("api/ShopSensor");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ShopSensorEvent>>(content);
        }

        public static async Task<IEnumerable<DayStats>> GetWeeklyData()
        {
            var response = await http.GetAsync("api/ShopSensor/ByDay");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DayStats>>(content);
        }

        public static async Task<IEnumerable<HourStats>> GetHourlyData()
        {
            var response = await http.GetAsync("api/ShopSensor/ByHour");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<HourStats>>(content);
        }
    }
}
