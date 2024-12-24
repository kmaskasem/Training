using System.ComponentModel.DataAnnotations;

namespace TrainingX_2.Models.Base
{
    public class Province
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        [MaxLength(150)]
        public string NameInThai { get; set; }
        [MaxLength(150)]
        public string NameInEnglish { get; set; }

    }
}
