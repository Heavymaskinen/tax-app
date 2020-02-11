using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using TaxApiModel;

namespace ConsoleClient
{

    /// <summary>
    /// Basic test tool to use the TaxApplication API
    /// </summary>
    public class ApiClient
    {
        private string url;
        private HttpClient client;

        public ApiClient(string url)
        {
            this.url = url;
            client = new HttpClient();
            SetupHeaders();
        }

        public TaxResponse GetTax(string municipality, string date)
        {
            return ProcessGet(municipality, date).Result;
        }

        public string ScheduleTax(string municipality, double tax, string startDate, string endDate)
        {
            return ProcessSchedule(municipality, startDate, endDate, tax).Result.StatusCode.ToString();
        }

        private void SetupHeaders()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Tax schedule client");
        }

        private async Task<TaxResponse> ProcessGet(string municipality, string date)
        {
            var task = CreateStreamTask(url + "tax?municipality=" + municipality + "&date=" + date);
            return await GetObject(task, typeof(TaxResponse)) as TaxResponse;
        }

        private async Task<HttpResponseMessage> ProcessSchedule(string municipality, string startDate, string endDate, double tax)
        {
            DateTime startD = DateTime.Parse(startDate);
            DateTime endD = DateTime.Parse(endDate);
            Console.WriteLine("Start " + startD.ToShortDateString());
            Console.WriteLine("End " + endD.ToShortDateString());
            var request = new ScheduleRequest() { Tax = tax, Municipality = municipality, StartDate = startD, EndDate = endD };
            return await client.PostAsJsonAsync<ScheduleRequest>(url + "schedule", request);
        }

        private static async Task<object> GetObject(Task<Stream> task, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            object unserialized = serializer.ReadObject(await task);
            return unserialized;
        }

        private Task<Stream> CreateStreamTask(string requestUri)
        {
            return client.GetStreamAsync(requestUri);
        }
    }
}
