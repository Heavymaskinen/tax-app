using System;
using NUnit.Framework;
using TaxStore.External;
using TaxStore.Model;
using TaxStore.Usecase;

namespace TaxStoreTests
{
    public class Tests
    {
        [Test]
        public void NoSuchMunicipality_NotOk()
        {
            var usecase = new ReadMunicipalityTax(new FakeStorage(null));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.IsFalse(result.IsOk());
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void NoTaxScheduled_Ok_Empty()
        {
            var usecase = new ReadMunicipalityTax(new FakeStorage(new Municipality("Himlen")));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.IsTrue(result.IsOk());
            Assert.IsTrue(result.IsEmpty());
        }

        [Test]
        public void TaxScheduled_Ok_WithValue()
        {
            Municipality municipality = new Municipality("Himlen");
            municipality.ScheduleYearlyTax(2020, 0.2);
            var usecase = new ReadMunicipalityTax(new FakeStorage(municipality));
            var result = usecase.Read(new ReadMunicipalityTax.Request("Himlen", DateTime.Now));
            Assert.AreEqual(0.2, result.Tax);
        }
    }

    class FakeStorage : IStorage
    {
        private Municipality municipality;

        public FakeStorage(Municipality municipality)
        {
            this.municipality = municipality;
        }

        public Municipality GetMunicipality(string municipalityName)
        {
            return this.municipality;
        }
    }
}