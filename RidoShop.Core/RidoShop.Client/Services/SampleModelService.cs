using System.Collections.Generic;
using System.Threading.Tasks;


using Windows.UI.Xaml.Controls;
using ShopEvents.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System;

namespace RidoShop.Client.Services
{
    public class SampleModelService
    {
        public async Task<IEnumerable<TriggeredEvent>> GetDataAsync()
        {
            await Task.CompletedTask;
            HttpClient http = new HttpClient() { BaseAddress = new Uri(AppConfig.RidoShopServerUrl) };
            var response = await http.GetAsync("api/event");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<TriggeredEvent>>(content);
        }
    }
}
