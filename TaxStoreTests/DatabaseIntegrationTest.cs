using System;
using NUnit.Framework;
using TaxDatabase;
using TaxStore.Model;
using TaxStore.Usecase;

namespace TaxStoreIntegrationTests
{
    public class DatabaseIntegrationTest
    {
        private ScheduleTax.Request testRequest = new ScheduleTax.Request()
        {
            StartDate = new DateTime(2020, 01, 01),
            EndDate = new DateTime(2020, 12, 31),
            Municipality = "Odense",
            Tax = 0.2
        };

        [SetUp]
        public void PrepareDatabase()
        {
            var storage = new DatabaseRepository();
            storage.Database.EnsureCreated();
            if (storage.Municipalities.Find(testRequest.Municipality) == null)
            {
                storage.Municipalities.Add(new Municipality(testRequest.Municipality).ToData());
                storage.SaveChanges();
            } else
            {
                storage.Taxes.RemoveRange(storage.Taxes);
                storage.SaveChanges();
            }
            
        }

        [Test]
        public void ScheduleAndGet()
        {
            var storage = new DatabaseRepository();
            var scheduleCase = new ScheduleTax(storage);
            var scheduleResponse = scheduleCase.Schedule(testRequest);

            Assert.IsTrue(scheduleResponse.StatusOk, scheduleResponse.Message);

            var readCase = new ReadMunicipalityTax(storage);
            var readResponse = readCase.Read(new ReadMunicipalityTax.Request() { Date = new DateTime(2020, 02, 02), Municipality = "Odense" });
            Assert.IsTrue(readResponse.StatusOk, readResponse.Message);
            Assert.AreEqual(0.2, readResponse.Tax);
        }
    }
}
