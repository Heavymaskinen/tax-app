using System;
using TaxStore.Dto;

namespace TaxStore.Model
{
    public class Tax
    {
        private DateTime startDate;
        private DateTime endDate;

        public double TaxValue { get; private set; }
        public string MunicipalityName { get; internal set; }

        public Tax(double tax, DateTime startDate, DateTime endDate)
        {
            TaxValue = tax;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public TaxData ToData()
        {
            return new TaxData() { TaxValue = this.TaxValue, StartDate = startDate, EndDate = endDate, MunicipalityDataID = MunicipalityName };
        }

        internal bool CoversDate(DateTime date)
        {
            return date.CompareTo(startDate) > -1 && date.CompareTo(endDate) <= 0;
        }

        internal bool Equals(Tax tax)
        {
            return startDate.Equals(tax.startDate) && endDate.Equals(tax.endDate);
        }

        /// <summary>
        /// The tax with the closest end date is to be used. (I.e. monthly rather than yearly).
        /// If the end dates are the same, the shortest span wins.
        ///
        /// Note: Not pleased about the name...
        /// </summary>
        /// <returns>The instance with the shortest closest end date</returns>
        internal Tax FindPrimary(Tax other)
        {
            var comparison = endDate.CompareTo(other.endDate);
            if (comparison == 0)
            {
                return GetDuration() > other.GetDuration() ? other : this;
            }

            return endDate.CompareTo(other.endDate) < 0 ? this : other;
        }

        private long GetDuration()
        {
            return endDate.Ticks - startDate.Ticks;
        }
    }
}
