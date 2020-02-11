using System;
using TaxStore.External;
using TaxStore.Model;

namespace TaxStore.Usecase
{
    public class ScheduleTax
    {
        public class Request
        {
            public string Municipality { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public double Tax { get; set; }

            public Request() { }

            public Request(string municipalityName, DateTime startDate, DateTime endDate)
            {
                Municipality = municipalityName;
                StartDate = startDate;
                EndDate = endDate;
            }
        }

        public class Response
        {
            public bool StatusOk { get; private set; }
            public string Message { get; private set; }

            public static Response OK()
            {
                return new Response { StatusOk = true };
            }

            public static Response NotOK(string message)
            {
                return new Response { StatusOk = false, Message = message };
            }
        }

        private IMunicipalityRepository storage;

        public ScheduleTax(IMunicipalityRepository storage)
        {
            this.storage = storage;
        }

        public Response Schedule(Request request)
        {
            var municipalityData = storage.GetMunicipality(request.Municipality);
            if (municipalityData == null)
            {
                return Response.NotOK("No such municipality [" + request.Municipality + "]");
            }

            var municipality = new MunicipalityFactory().Create(municipalityData);

            try
            {
                municipality.SchedulePeriodTax(request.StartDate, request.EndDate, request.Tax);
            }
            catch (InvalidSchedulingException e)
            {
                return Response.NotOK(e.Message);
            }

            storage.SaveMunicipality(municipality.ToData());
            return Response.OK();
        }

    }
}
