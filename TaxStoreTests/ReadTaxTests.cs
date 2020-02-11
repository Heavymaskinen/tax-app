using System;
using NUnit.Framework;
using TaxStore.External;
using TaxStore.Model;
using TaxStore.Usecase;
using TaxStoreTests.Stubs;

namespace TaxStoreTests
{
    public class ReadTaxTests
    {
        [Test]
        public void NoSuchMunicipality_NotOk()
        {
            var usecase = new ReadMunicipalityTax(new FakeRepository(null));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.IsFalse(result.StatusOk);
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void NoTaxScheduled_Ok_Empty()
        {
            var usecase = new ReadMunicipalityTax(new FakeRepository(new Municipality("Himlen").ToData()));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.IsTrue(result.StatusOk);
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void TaxScheduled_Ok_WithValue()
        {
            Municipality municipality = new Municipality("Himlen");
            municipality.ScheduleYearlyTax(2020, 0.2);
            var usecase = new ReadMunicipalityTax(new FakeRepository(municipality.ToData()));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.AreEqual(0.2, result.Tax);
        }
    }

}