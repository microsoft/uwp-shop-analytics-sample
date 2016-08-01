using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ShopAnalyticsPCL;
using System.Threading.Tasks;

namespace UWPTests
{
    [TestClass]
    public class UWPClientTests
    {
        Client client = new Client();

        [TestMethod]
        public async Task CreateEvent()
        {
            await client.CreateEvent(true);
            await client.CreateEvent(false);
        }


        [TestMethod]
        public async Task ReadAll()
        {
            var events = await client.ReadAllEvents();
            Assert.IsNotNull(events);
            foreach (var te in events)
            {
                System.Diagnostics.Debug.WriteLine($"type {te.EventType} on {te.EventTime.ToString()}");
            }
        }
    }
}
