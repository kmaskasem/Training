using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Models.Base
{
    public class TopicAssessment
    {
        [Key]
        [Column("topic_assessment_Id")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "โปรดกรอกข้อมูล")]
        [Display(Name = "ชื่อหัวข้อการประเมิน")]
        public string Name { get; set; }
        [Display(Name = "สถานะ")]
        public Status? Status { get; set; } = TrainingX_2.Areas.Identity.Data.Status.ใช้งาน;
    }
}
