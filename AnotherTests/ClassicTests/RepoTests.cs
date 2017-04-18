using EventApi.DocDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAnalyticsPCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicTests
{
    [TestClass]
    public class RepoTests
    {

        TriggeredEventRepository r = new TriggeredEventRepository(new Uri("https://ridoshopdb.documents.azure.com:443/"),
            "20ySChTRW4mVpQEb5aP3Oy3Nxvxiph0HH0EoE6hDA0vRG7XPuwcPaIclOQX5Gmh15afj7nTcNxmAlAD0mxTOzw==");

        [TestMethod]
        public async Task AddAndGet()
        {
            var te = new TriggeredEvent() { EventType = true, EventTime = new DateTime(2000, 2, 20) };
            
            var id = await  r.Add(te);
            Console.WriteLine(id);

            var te2 = await r.Get(id);
            Assert.AreEqual(te2.EventType, true);
            Assert.AreEqual(te2.EventTime, new DateTime(2000, 2, 20));
        }

        [TestMethod]
        public async Task GetAll()
        {
            var all = await r.GetAll();
            foreach (var item in all)
            {
                Console.WriteLine(item.EventTime);
            }
        }

    }
}
