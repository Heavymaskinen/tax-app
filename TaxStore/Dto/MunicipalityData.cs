using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxStore.Dto
{
    public class MunicipalityData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MunicipalityID { get; set; }
        public ICollection<TaxData> TaxSchedules { get; set; }
    }
}
