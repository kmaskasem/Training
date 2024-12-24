using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Models.Training
{
    public class Enrollment
    {

        [Key]
        [Column("enrollment_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [NotMapped]
        [Display(Name = "ชื่อจริง")]
        public string? FullName { get; set; }
        [Display(Name = "เบอร์ติดต่อ")]
        [MaxLength(10)]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่ลงทะเบียน")]
        public DateTime? Enroll_date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่ชำระเงิน")]
        public DateTime? Payment_date { get; set; }
        public string? Payment_picture { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile PaymentImageFile { get; set; }
        public string? Note { get; set; }

        [Display(Name = "สถานะการตรวจสอบ")]
        public Status_Enroll? Check_status { get; set; } = Status_Enroll.ยังไม่ตรวจสอบ;
        //public bool? Assess_status { get; set; } = false;

        public string ParticipantId { get; set; }
        [ForeignKey(nameof(ParticipantId))] 
        public User Participant { get; set; }
        public string TrainingId { get; set; }
        [ForeignKey(nameof(TrainingId))]
        public Training Training { get; set; }

    }
    public enum Status_Enroll
    {
        ยังไม่ตรวจสอบ, ตรวจสอบผ่าน, ตรวจสอบไม่ผ่าน, ยกเลิกการลงทะเบียน
    }
}
