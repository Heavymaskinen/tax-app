using System;
using System.Runtime.Serialization;

namespace TaxApiModel
{
    [DataContract]
    public class ScheduleRequest
    {
        public ScheduleRequest()
        {
        }

        [DataMember(Name ="municipality")]
        public string Municipality { get; set; }

        [DataMember(Name = "tax")]
        public double Tax { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }
    }
}
