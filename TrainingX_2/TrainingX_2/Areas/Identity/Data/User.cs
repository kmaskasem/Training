using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [Display(Name = "คำนำหน้าชื่อ")]
    public NameTitle? Name_title { get; set; }

    [Required]
    [Display(Name = "ชื่อจริง")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "นามสกุล")]
    public string LastName { get; set; }

    [NotMapped]
    [Display(Name = "ชื่อจริง")]
    public string FullName
    {
        get { return Name_title + FirstName + " " + LastName; }
    }

    [Display(Name = "ประเภทผู้ใช้งาน")]
    [Column("Role")]
    public Role? role { get; set; } = Role.Member;

    [NotMapped]
    [Display(Name = "ที่อยู่")]
    public string? Address
    {
        get
        {
            return Address_street +
                 ((Address_subdistrict != null) ? " ต." + Address_subdistrict : "") +
                 ((Address_district != null) ? " อ." + Address_district : "") +
                 ((Address_province != null) ? " จ." + Address_province : "") +
                 ((Address_zipcode != null) ? " " + Address_zipcode : "");
        }
    }

    [Display(Name = "รายละเอียด")]
    public string? Address_street { get; set; }
    [Display(Name = "จังหวัด")]
    public string? Address_province { get; set; }
    [Display(Name = "อำเภอ")]
    public string? Address_district { get; set; }
    [Display(Name = "ตำบล")]
    public string? Address_subdistrict { get; set; }
    [Display(Name = "ไปรษณีย์")]
    public string? Address_zipcode { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
    [Display(Name = "วันเกิด")]
    public DateTime? Birthday { get; set; }

    public string? Profile_picture { get; set; }

    [NotMapped]
    [DisplayName("อัปโหลดภาพโปรไฟล์")]
    public IFormFile? ImageFile { get; set; }

    [Display(Name = "สถานะ")]
    public Status? Status_user { get; set; } = Status.ใช้งาน;

    //public NameTitle NameTitle { get; set; }
    public ICollection<Training> Trainings { get; set; }
    //public ICollection<Enrollment> Enrollments { get; set; }


}

public enum NameTitle
{
    นาย, นาง, นางสาว,Miss,
    [Display(Name = "Mrs.")]
    Mrs,

    [Display(Name = "Mr.")]
    Mr
}

public enum Role
{
    Admin, Member
}
public enum Status
{
    ใช้งาน, ไม่ใช้งาน
}


