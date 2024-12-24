using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Base;

namespace TrainingX_2.Models.Training
{
    public class Training
    {
        [Key]
        [Column("training_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required(ErrorMessage = "โปรดกรอกชื่อกิจกรรม")]
        [Display(Name = "ชื่อกิจกรรมอบรม")]
        public string Name { get; set; }
        [Display(Name = "รายละเอียด")]
        public string? Description { get; set; }
        [Display(Name = "วิทยากร")]
        public string? Lecturer { get; set; }
        [Display(Name = "จุดประสงค์")]
        public string? Objectives { get; set; }
        public string? Training_picture { get; set; }
        [NotMapped]
        [DisplayName("อัพโหลดภาพโปรโมทกิจกรรมอบรม")]
        public IFormFile? ImageFile { get; set; }

        public string? Schedule_file { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ScheduleFile { get; set; }

        public DateTime? Created_Date { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่เริ่มการจัดกิจกรรมอบรม")]
        [Required(ErrorMessage = "โปรดกรอกวันที่เริ่มการจัดกิจกรรม")]
        public DateTime? Start_train_Date { get; set; }

        //public GetThaiDate()

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่สิ้นสุดการจัดกิจกรรมอบรม")]
        [Required(ErrorMessage = "โปรดกรอกวันที่สิ้นสุดการจัดกิจกรรม")]
        public DateTime? End_train_Date { get; set; }
        [Display(Name = "ชั่วโมง")]
        [Required(ErrorMessage = "โปรดกรอกระยะเวลา")]
        public int Hours { get; set; } = 0;
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่เริ่มลงการทะเบียน")]
        [Required(ErrorMessage = "โปรดกรอกวันที่เริ่มลงการทะเบียน")]
        public DateTime? Start_enroll_Date { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่สิ้นสุดการลงทะเบียน")]
        [Required(ErrorMessage = "โปรดกรอกวันที่สิ้นสุดการลงทะเบียน")]
        public DateTime? End_enroll_Date { get; set; }
        [Display(Name = "ค่าใช้อบรม")]
        [Required(ErrorMessage = "โปรดกรอกค่าใช้อบรม")]
        public float Cost { get; set; } = 0;
        [Display(Name = "ธนาคาร")]
        public int? BankId { get; set; }
        [ForeignKey(nameof(BankId))]
        public Bank? Bank { get; set; }
        [Display(Name = "เลขบัญชี")]
        public string? Bank_account_number { get; set; }
        [Display(Name = "ชื่อเจ้าของบัญชี")]
        public string? Bank_account_holder { get; set; }
        [Required(ErrorMessage = "โปรดกรอกจำนวนผู้เข้าร่วม")]
        [Display(Name = "จำนวนผู้เข้าร่วม")]
        public int NumberOfParticipants { get; set; }
        [Display(Name = "รายละเอียด")]
        public string? Venue_street { get; set; }
        [Display(Name = "ตำบล")]
        public string? Venue_subdistrict { get; set; }
        [Display(Name = "อำเภอ")]
        public string? Venue_district { get; set; }
        [Display(Name = "จังหวัด")]
        public string? Venue_province { get; set; }
        [Display(Name = "ไปรษณีย์")]
        public string? Venue_zipcode { get; set; }

        [NotMapped]
        [Display(Name = "ที่อยู่")]
        public string? Venue
        {
            get
            {
                return Venue_street +
                 ((Venue_subdistrict != null) ? " ต." + Venue_subdistrict : "") +
                 ((Venue_district != null) ? " อ." + Venue_district : "") +
                 ((Venue_province != null) ? " จ." + Venue_province : "") +
                 ((Venue_zipcode != null) ? " " + Venue_zipcode : "");
            }
        }
        [Display(Name = "สถานะการดำเนินการ")]
        public State_Train? State { get; set; } = State_Train.แบบร่าง;
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }
        [Required(ErrorMessage = "โปรดเลือกประเภทกิจกรมม")]
        public int CategId { get; set; }
        [ForeignKey(nameof(CategId))]
        //public int[]? selectedCategs { get; set; }
        public Category Category { get; set; }
        //public TrainingAssessment Assessment { get; set; }

        //public ICollection<TrainingLecturer> Lecturers { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        [NotMapped]
        public int? EnrolledCount { get; set; } = 0;

    }
}
public enum State_Train
{
    แบบร่าง, โพสต์แล้ว, สำเร็จ, ยกเลิก
}