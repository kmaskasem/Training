using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Models.Base
{
    public class Bank
    {
        [Key]
        [Column("bank_Id")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "ชื่อธนาคาร")]
        public string Name { get; set; }
        [Display(Name = "สถานะ")]
        public Status? Status { get; set; } = TrainingX_2.Areas.Identity.Data.Status.ใช้งาน;
    }
}
