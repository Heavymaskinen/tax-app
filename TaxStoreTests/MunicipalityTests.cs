using System;
using NUnit.Framework;
using TaxStore.Model;

namespace TaxStoreTests
{
    [TestFixture]
    public class MunicipalityTests
    {

        [Test]
        public void NoScheduledTax_ReturnNAN()
        {
            var mun = new Municipality("Hillerød");
            var result = mun.GetTaxForDate(new DateTime(2020, 02, 02));
            Assert.AreEqual(double.NaN, result);
        }

        [Test]
        public void OneScheduledTax_IsReturned()
        {
            var mun = new Municipality("Hillerød");
            mun.ScheduleYearlyTax(2020, 0.2);
            Assert.AreEqual(0.2, mun.GetTaxForDate(new DateTime(2020, 02, 02)));
        }

        [Test]
        public void OverlappingMonthlyTaxSchedule_IsReturned()
        {
            var mun = new Municipality("Hillerød");
            mun.ScheduleYearlyTax(2020, 0.2);
            mun.ScheduleMonthlyTax(2020, 02, 0.4);
            Assert.AreEqual(0.4, mun.GetTaxForDate(new DateTime(2020, 02, 27)));
        }

        [Test]
        public void OverlappingSpecificTaxSchedule_IsReturned()
        {
            var mun = new Municipality("Hillerød");
            mun.ScheduleYearlyTax(2020, 0.2);
            mun.SchedulePeriodTax(new DateTime(2020, 02, 22), new DateTime(2020, 02, 29), 0.8);
            mun.ScheduleMonthlyTax(2020, 02, 0.4);

            Assert.AreEqual(0.8, mun.GetTaxForDate(new DateTime(2020, 02, 27)));
        }

        [Test]
        public void OverlappingSingleDayTaxSchedule_IsReturned()
        {
            var mun = new Municipality("Hillerød");
            mun.ScheduleYearlyTax(2020, 0.2);
            mun.SchedulePeriodTax(new DateTime(2020, 02, 22), new DateTime(2020, 02, 29), 0.8);
            mun.ScheduleDailyTax(new DateTime(2020, 02, 29), 0.1);

            Assert.AreEqual(0.1, mun.GetTaxForDate(new DateTime(2020, 02, 29)));
        }

        [Test]
        public void NotOverlappingSingleDayTaxSchedule_YearlyIsReturned()
        {
            var mun = new Municipality("Hillerød");
            mun.ScheduleYearlyTax(2020, 0.2);
            mun.SchedulePeriodTax(new DateTime(2020, 04, 22), new DateTime(2020, 04, 29), 0.8);
            mun.ScheduleDailyTax(new DateTime(2020, 05, 29), 0.1);

            Assert.AreEqual(0.2, mun.GetTaxForDate(new DateTime(2020, 02, 29)));
        }

        [Test]
        public void DoubleYearlyTax_Fail()
        {
            Assert.Throws(typeof(InvalidSchedulingException),()=>{
                var mun = new Municipality("Hillerød");
                mun.ScheduleYearlyTax(2020, 0.2);
                mun.ScheduleYearlyTax(2020, 0.4);
            });
        }

        [Test]
        public void DoubleMonthlyTax_Fail()
        {
            Assert.Throws(typeof(InvalidSchedulingException), () => {
                var mun = new Municipality("Hillerød");
                mun.ScheduleMonthlyTax(2020,2, 0.2);
                mun.ScheduleMonthlyTax(2020, 2, 0.2);
            });
        }

        [Test]
        public void DoubleSpecificTax_Fail()
        {
            Assert.Throws(typeof(InvalidSchedulingException), () => {
                var mun = new Municipality("Hillerød");
                mun.SchedulePeriodTax(new DateTime(2020,2,20), new DateTime(2020, 4, 20), 0.2);
                mun.SchedulePeriodTax(new DateTime(2020, 2, 20), new DateTime(2020, 4, 20), 0.2);
            });
        }
    }
}
