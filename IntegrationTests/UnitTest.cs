using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ShopAnalyticsPCL.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ShopAnalyticsPCL.Resources;
using System.Diagnostics;

namespace IntegrationTests
{
    [TestClass]
    public class EventsApiFakeClient
    {
        private HttpClient client;
        private readonly string baseuri = Keys.AzureWebAppUri;

        [TestMethod]
        public async Task SendOneEvent()
        {
            var newEvent = new TriggeredEvent
            {
                EventType = true,
                EventTime = DateTime.Now
            };

            client = new HttpClient { BaseAddress = new Uri(baseuri) };
            var json = JsonConvert.SerializeObject(newEvent);
            Debug.WriteLine(json);
            HttpContent contentPost = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage message = await client.PostAsync("api/event", contentPost);
            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var errorDetail = await message.Content.ReadAsStringAsync();
                Assert.Fail(errorDetail);
            }
            Assert.IsNotNull(message);
            Debug.WriteLine(message);
        }
    }
}
