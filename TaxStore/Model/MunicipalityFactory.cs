using TaxStore.Dto;

namespace TaxStore.Model
{
    public class MunicipalityFactory
    {
        public MunicipalityFactory()
        {
        }

        public Municipality Create(MunicipalityData data)
        {
            var municipality = new Municipality(data.MunicipalityID);
            foreach (var schedule in data.TaxSchedules)
            {
                municipality.SchedulePeriodTax(schedule.StartDate, schedule.EndDate, schedule.TaxValue);
            }

            return municipality;
        }
    }
}
