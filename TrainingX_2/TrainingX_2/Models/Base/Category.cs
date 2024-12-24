using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Models.Base
{
    public class Category
    {
        [Key]
        [Column("categ_Id")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "ชื่อประเภท")]
        public string Name { get; set; }
        [Display(Name = "สถานะ")]
        public Status? Status { get; set; } = TrainingX_2.Areas.Identity.Data.Status.ใช้งาน;

        public ICollection<Training.Training> Trainings { get; set; }
    }
}

