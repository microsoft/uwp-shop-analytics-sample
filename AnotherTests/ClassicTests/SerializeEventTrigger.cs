using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAnalyticsPCL.Models;

namespace ClassicTests
{
    [TestClass]
    public class SerializeEventTrigger
    {
        [TestMethod]
        public void SerializeAndDeserialize()
        {
            var te = new TriggeredEvent() { EventType = true, EventTime = DateTime.Now };

            var json = te.ToJson();

            System.Diagnostics.Debug.WriteLine(json);

            var te2 = TriggeredEvent.FromJson(json);
            
            Assert.AreEqual(te2.EventType, true); 
            
        }
    }
}
