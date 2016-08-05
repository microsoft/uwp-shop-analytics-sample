using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAnalyticsPCL;
using System.Threading.Tasks;

namespace ClassicTests
{
    [TestClass]
    public class ClientTests
    {
        Client client = new Client();

        [TestMethod]
        public async Task CreateEvent()
        {
            await client.CreateEvent(true);            
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
