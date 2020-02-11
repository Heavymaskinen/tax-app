using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxApiModel;
using TaxStore.Usecase;

namespace TaxApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private ILogger<TaxController> logger;

        public ScheduleController(ILogger<TaxController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Schedule a Tax for a given time period
        /// </summary>
        /// <param name="request">
        /// Dates should be formatted as yyyy-mm-dd
        /// </param>
        /// <returns>Success or not</returns>
        [HttpPost]
        public ActionResult<bool> AddSchedule([FromBody] ScheduleRequest request)
        {
            if (request.StartDate == null || request.EndDate == null)
            {
                logger.LogDebug("Received request with empty date(s)");
                return new ActionResult<bool>(false);
            }

            var usecase = new ScheduleTax(ApiConfiguration.Repository);
            var response = usecase.Schedule(new ScheduleTax.Request()
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Municipality = request.Municipality,
                Tax = request.Tax
            });

            return new ActionResult<bool>(response.StatusOk);
        }

        [HttpGet]
        public ActionResult<string> GetInfo()
        {
            return "Use POST to schedule taxes";
        }
    }
}
