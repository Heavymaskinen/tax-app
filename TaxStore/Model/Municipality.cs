using System;
using System.Collections.Generic;
using System.Linq;
using TaxStore.Dto;

namespace TaxStore.Model
{
    public abstract class TaxStoreException : Exception
    {
        public TaxStoreException(string message) : base(message) { }
        public TaxStoreException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidSchedulingException : TaxStoreException
    {
        public InvalidSchedulingException(string message) : base(message) { }
    }

    public class Municipality
    {
        private IList<Tax> taxes;

        public string Name { get; private set; }

        public Municipality(string name)
        {
            Name = name;
            taxes = new List<Tax>();
        }

        public MunicipalityData ToData()
        {
            return new MunicipalityData() { MunicipalityID = this.Name, TaxSchedules = taxes.Select(tax => tax.ToData()).ToList() };
        }

        public double GetTaxForDate(DateTime date)
        {
            Tax foundTax = null;
            foreach (var tax in taxes.Where(tax => tax.CoversDate(date)).Select(tax => tax))
            {
                if (foundTax == null)
                {
                    foundTax = tax;
                }
                else
                {
                    foundTax = foundTax.FindPrimary(tax);
                }
            }

            return foundTax == null ? double.NaN : foundTax.TaxValue;
        }

        public void ScheduleYearlyTax(int year, double tax)
        {
            SchedulePeriodTax(new DateTime(year, 01, 01), new DateTime(year, 12, 31), tax);
        }

        public void ScheduleMonthlyTax(int year, int month, double tax)
        {
            var lastDayInMonth = DateTime.DaysInMonth(year, month);
            SchedulePeriodTax(new DateTime(year, month, 01), new DateTime(year, month, lastDayInMonth), tax);
        }

        public void ScheduleDailyTax(DateTime date, double tax)
        {
            SchedulePeriodTax(date, date, tax);
        }

        public void SchedulePeriodTax(DateTime startDate, DateTime endDate, double tax)
        {
            var taxObj = new Tax(tax, startDate, endDate);
            AddTax(taxObj);
        }

        private void AddTax(Tax taxObj)
        {
            EnsureValidScheduling(taxObj);
            taxObj.MunicipalityName = Name;
            taxes.Add(taxObj);
        }

        private void EnsureValidScheduling(Tax taxObj)
        {
            var exists = taxes.Where(tax => tax.Equals(taxObj)).Count<Tax>() > 0;
            if (exists)
            {
                throw new InvalidSchedulingException("A tax has already been scheduled for exact same dates.");
            }
        }
    }
}
