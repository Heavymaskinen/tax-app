using System;
using TaxStore.External;
using TaxStore.Model;

namespace TaxStore.Usecase
{
    public class ReadMunicipalityTax
    {
        private IMunicipalityRepository storage;

        public class Request
        {
            public Request() { }
            public Request(string municipality, DateTime date)
            {
                Municipality = municipality;
                Date = date;
            }

            public string Municipality { get; set; }
            public DateTime Date { get; set; }
        }

        public class Response
        {
            public double Tax { get; private set; }
            public string Message { get; private set; }

            internal static Response OK(double tax)
            {
                return new Response() { StatusOk = true, Tax = tax, Message = "OK" };
            }

            internal static Response OKEmpty()
            {
                return new Response() { StatusOk = true, Tax = double.NaN, Message = "OK" };
            }

            internal static Response NotOK(string message)
            {
                return new Response() { StatusOk = false, Tax = double.NaN, Message = message };
            }

            public bool StatusOk
            {
                get; private set;
            }

            public bool IsEmpty()
            {
                return double.IsNaN(Tax);
            }
        }

        public ReadMunicipalityTax(IMunicipalityRepository storage)
        {
            this.storage = storage;
        }

        public Response Read(Request request)
        {
            var municipalityData = storage.GetMunicipality(request.Municipality);

            if (municipalityData == null)
            {
                return Response.NotOK("No such municipality [" + request.Municipality + "]");
            }

            var municipality = new MunicipalityFactory().Create(municipalityData);
            return Response.OK(municipality.GetTaxForDate(request.Date));

        }

    }
}
