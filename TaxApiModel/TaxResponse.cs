using System;
using System.Runtime.Serialization;

namespace TaxApiModel
{
    [DataContract]
    public class TaxResponse
    {
        public TaxResponse(double tax, bool statusOk, string municipality, string message)
        {
            Tax = tax;
            Ok = statusOk;
            Municipality = municipality;
            Message = message;
        }

        [DataMember(Name = "tax")]
        public double Tax { get; set; }
        [DataMember(Name = "ok")]
        public bool Ok { get; set; }
        [DataMember(Name = "municipality")]
        public string Municipality { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
