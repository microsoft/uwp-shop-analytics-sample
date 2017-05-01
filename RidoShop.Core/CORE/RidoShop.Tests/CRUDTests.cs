using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RidoShop.Tests
{
    public class CRUDTests
    {

        [Fact]
        public async Task TodayLessThanAll()
        {
            var c = new Client();
            await c.CreateEvent(true);
            var all = await c.ReadAllEvents();
            Console.WriteLine(all.Count().ToString());
        }

        [Fact]
        public async Task Add_Read_Event()
        {
            var c = new Client();
            
            var all = await c.ReadTodayEvents();
            await c.CreateEvent(false);
            var all2 = await c.ReadTodayEvents();

            
        }
    }
}
