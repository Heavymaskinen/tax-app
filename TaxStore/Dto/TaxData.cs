using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxStore.Dto
{
    public class TaxData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TaxValue { get; set; }
        public string MunicipalityDataID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
    }
}