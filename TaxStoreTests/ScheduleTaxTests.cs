using System;
using NUnit.Framework;
using TaxStore.Model;
using TaxStore.Usecase;
using TaxStoreTests.Stubs;

namespace TaxStoreTests
{
    public class ScheduleTaxTests
    {
        [Test]
        public void NoSuchMunicipality_NotOk()
        {
            var usecase = new ScheduleTax(new FakeRepository(null));
            var result = usecase.Schedule(new ScheduleTax.Request()
            {
                Municipality = "Odense",
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2020, 12, 31),
                Tax = 0.2
            });
            Assert.IsFalse(result.StatusOk);
        }

        [Test]
        public void NoScheduledTaxes_ScheduleOne()
        {
            var muni = new Municipality("Odense");
            var storage = new FakeRepository(muni.ToData());
            var usecase = new ScheduleTax(storage);

            var result = usecase.Schedule(new ScheduleTax.Request()
            {
                Municipality = "Odense",
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2020, 12, 31),
                Tax = 0.2
            }
            );

            var municipality = new MunicipalityFactory().Create(storage.savedMunicipality);

            Assert.AreEqual("Odense", municipality.Name);
            Assert.AreEqual(0.2, municipality.GetTaxForDate(new DateTime(2020, 04, 04)));
            Assert.IsTrue(result.StatusOk);
        }

        [Test]
        public void DoubleYearlyTax_NoOk()
        {
            var muni = new Municipality("Odense");
            muni.ScheduleYearlyTax(2020, 0.5);
            var storage = new FakeRepository(muni.ToData());
            var usecase = new ScheduleTax(storage);

            var result = usecase.Schedule(new ScheduleTax.Request()
            {
                Municipality = "Odense",
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2020, 12, 31),
                Tax = 0.2
            }
            );

            Assert.IsFalse(result.StatusOk);
        }


    }
}
