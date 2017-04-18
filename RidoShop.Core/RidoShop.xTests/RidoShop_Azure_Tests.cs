    using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace RidoShop.xTests
{
    public class RidoShop_Azure_Tests
    {
        Client client = new Client();

        [Fact]
        public async Task CreateEvent()
        {
            //await client.CreateEvent(false);
            await client.CreateEvent(true);
            //await client.CreateEvent(false);
        }
        [Fact]
        public async Task ReadAll()
        {
            var events = await client.ReadAllEvents();
            Assert.NotNull(events);
            foreach (var te in events)
            {
                Debug.WriteLine($"type {te.EventType} on {te.EventTime.ToString()}");
            }
        }

    }
}
