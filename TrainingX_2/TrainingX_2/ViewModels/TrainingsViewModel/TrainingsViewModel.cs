using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Base;

namespace TrainingX_2.ViewModels.TrainingsViewModel
{
    public class TrainingsViewModel
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Lecturer { get; set; }

        public string? Objectives { get; set; }

        public string? Training_picture { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? Schedule_file { get; set; }

        public IFormFile ScheduleFile { get; set; }

        public DateTime? Created_Date { get; set; }
        public DateTime? Start_train_Date { get; set; }

        public DateTime? End_train_Date { get; set; }
        public int? Hours { get; set; }
        public DateTime? Start_enroll_Date { get; set; }

        public DateTime? End_enroll_Date { get; set; }

        public float? Cost { get; set; }
        public Bank? Bank { get; set; }
        public int? BankId { get; set; }
        public string? Bank_account_number { get; set; }
        public string? Bank_account_holder { get; set; }

        public int NumberOfParticipants { get; set; }

        public string? Venue_street { get; set; }
        public string? Venue_subdistrict { get; set; }
        public string? Venue_district { get; set; }
        public string? Venue_province { get; set; }
        public string? Venue_zipcode { get; set; }

        public State_Train? State { get; set; }
        public string OwnerId { get; set; }
        public User Owner { get; set; }


    }
}
