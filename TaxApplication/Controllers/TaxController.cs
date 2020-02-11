using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxApiModel;
using TaxStore.Usecase;

namespace TaxApplication
{
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private ILogger<TaxController> logger;

        public TaxController(ILogger<TaxController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the tax for a municipality at a given time.
        /// Returns -1 for tax if no tax has been scheduled.
        /// </summary>
        /// <param name="municipality"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<TaxResponse> Get([FromQuery] string municipality, [FromQuery] string date) {
            if (string.Empty.Equals(date) || date == null)
            {
                logger.LogDebug("Received request without a date");
                return new ActionResult<TaxResponse>(new TaxResponse(-1, false, municipality, "Supply a date"));
            }

            var parsed = DateTime.Parse(date);
            var usecase = new ReadMunicipalityTax(ApiConfiguration.Repository);
            var response = usecase.Read(new ReadMunicipalityTax.Request() { Date = parsed, Municipality = municipality });
            
            var tax = double.IsNaN(response.Tax) ? -1 : response.Tax;
            return new ActionResult<TaxResponse>(new TaxResponse(tax, true, municipality, response.Message));
        }
    }
}
