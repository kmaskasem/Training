using System.ComponentModel.DataAnnotations;

namespace TrainingX_2.Models.Base
{
    public class Subdistrict
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        [MaxLength(150)]
        public string NameInThai { get; set; }
        [MaxLength(150)]
        public string? NameInEnglish { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
        public int? ZipCode { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
