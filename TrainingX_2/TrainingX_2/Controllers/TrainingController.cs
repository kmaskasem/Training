using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2;
using TrainingX_2.Models.Training;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingX_2.ViewModels.CategoriesViewModel;
using TrainingX_2.Models.Base;
using TrainingX_2.ViewModels.TrainingsViewModel;
using System.ComponentModel.DataAnnotations;
using System;
using TrainingX_2.ViewModels;
using TrainingX_2.Helpers;

namespace TrainingX_2.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IUnitOfWork _unitofWork;
        private IMapper _mapper;

        private readonly IUpdateService _updateService;

        public TrainingController(ApplicationDBContext context,
            IWebHostEnvironment hostEnvironment,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUnitOfWork unitofWork,
            IMapper mapper,
            IUpdateService updateService
            )
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _userManager = userManager;
            _signInManager = signInManager;

            _unitofWork = unitofWork;
            _mapper = mapper;

            _updateService = updateService;
        }
        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        }
        public async Task<IActionResult> MyTraining(int? pageNumber,
            string? searchName,
            int? searchCategory,
            string? searchProvince,
            DateTime? startdate,
            DateTime? enddate,  
            DateTime? enrolldate
            )
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if (user.role == Role.Admin)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            #region ViewData
            ViewData["Categories"] = new SelectList(_context.Categories
                .Where(c => c.Status == Status.ใช้งาน)
                .ToList().OrderBy(s => s.Name), "Id", "Name");
            ViewData["Provinces"] = new SelectList(_context.Provinces
                .ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai");
            #endregion
            //await LoadAsync(user);
            //var Trainings = await _context.Trainings.Where(t => t.OwnerId == _userManager.GetUserId(User)).ToListAsync();
            //ViewData["IdIdId"] = _userManager.GetUserId(User);

            if (pageNumber == null)
                pageNumber = 1;

            var Trainings = from t in _context.Trainings
                            .Include("Category")
                            .Where(t => t.OwnerId == _userManager.GetUserId(User))
                            select t;
            //var categories = _context.Categories.ToList();
            //Trainings = from e in Trainings
            //            join d in categories on e.CategId equals d.Id into table1
            //            from d in table1.ToList()
            //            select e;

            //model = (from c in db.CertificateDB
            //         join u in db.Users on c.User equals u.Id
            //         join d in db.CompanyDB on c.Company equals d.Id
            //         select new CertificateViewModel
            //         {
            //             CertNo = c.CertNo,
            //             UserName = u.UserName,
            //             CompanyName = d.CompanyName,
            //             CertDescription = c.CertDescription,
            //             LastModifiedDate = c.LastModifiedDate
            //         }
            //).ToList();


            if (!String.IsNullOrEmpty(searchName))
            {
                Trainings = Trainings.Where(t => t.Name!.Contains(searchName));
            }
            if (searchCategory != null)
            {
                Trainings = Trainings.Where(t => t.CategId == searchCategory);
            }
            if (!String.IsNullOrEmpty(searchProvince))
            {
                Trainings = Trainings.Where(t => t.Venue_province!.Contains(searchProvince));
            }
            if (startdate.HasValue && enddate.HasValue)
            {
                Trainings = Trainings.Where(s => s.Start_train_Date <= startdate);
                Trainings = Trainings.Where(s => s.End_train_Date >= enddate);
            }
            if (enrolldate.HasValue)
            {
                Trainings = Trainings.Where(e => e.Start_enroll_Date.Value.Date <= enrolldate.Value.Date);
                Trainings = Trainings.Where(e => e.End_enroll_Date.Value.Date >= enrolldate.Value.Date);
            }
            //var model = _unitofWork.Training.GetAll();
            //var vm = _mapper.Map<List<TrainingsViewModel>>(model);


            int pageSize = 10;
            var pt = await TrainingX_2.Controllers.PaginatedList<Training>.CreateAsync(Trainings.AsNoTracking(), pageNumber ?? 1, pageSize);
            foreach (var t in pt)
            {
                t.EnrolledCount = _context.Enrollments
                    .Where(e =>e.TrainingId == t.Id)
                    .Count(e => e.Check_status == Status_Enroll.ยังไม่ตรวจสอบ
                    || e.Check_status == Status_Enroll.ตรวจสอบผ่าน);
            }
            return View(pt);
        }


        public async Task<IActionResult> Detail(string Id)
        {

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return RedirectToAction("NeedToLoginPage", "Home");
            //}
            //else if (user.role == Role.Admin)
            //{
            //    return RedirectToAction("NeedToLoginPage", "Home");
            //}

            Training t = _context.Trainings
                .Include("Category")
                .Include("Bank")
                .Include("Owner")
                .FirstOrDefault(t => t.Id == Id);

            ViewData["isEnroll"] = _context.Enrollments
                .Where(t => t.TrainingId == Id)
                .Count( t => t.ParticipantId == _userManager.GetUserId(User));

            ViewData["Enroll_Number"] = _context.Enrollments
                .Where(t => t.TrainingId == Id)
                .Count( t => t.Check_status == Status_Enroll.ยังไม่ตรวจสอบ
                || t.Check_status == Status_Enroll.ตรวจสอบผ่าน);
            

            ViewData["Title"] = t.Name;

            return View(t);
        }

        public async Task<IActionResult> Edit(string Id)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if (user.role == Role.Admin)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }

            Training t = _context.Trainings
                .FirstOrDefault(t => t.Id == Id);
            #region ViewData
            ViewData["Categories"] = new SelectList(_context.Categories.Where(c => c.Status == Status.ใช้งาน).ToList().OrderBy(s => s.Name), "Id", "Name", t.CategId);
            ViewData["Banks"] = new SelectList(_context.Banks.Where(c => c.Status == Status.ใช้งาน).ToList().OrderBy(b => b.Name), "Id", "Name", t.BankId);
            if (t.Venue_province == null)
            {
                ViewData["Provinces"] = new SelectList(_context.Provinces.ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_province);

            }
            else
            {
                ViewData["Provinces"] = new SelectList(_context.Provinces.ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_province);
                if (t.Venue_district == null)
                {
                    ViewData["Districts"] = new SelectList(_context.Districts
                        .Where(d => d.Province.NameInThai == t.Venue_province)
                        .ToList()
                        .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_district);
                }
                else
                {
                    ViewData["Districts"] = new SelectList(_context.Districts
                        .Where(d => d.Province.NameInThai == t.Venue_province)
                        .ToList()
                        .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_district);

                    if (t.Venue_subdistrict == null)
                    {
                        ViewData["Subdistricts"] = new SelectList(_context.Subdistricts
                            .Where(d => d.District.NameInThai == t.Venue_district)
                            .ToList()
                            .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_subdistrict);
                    }
                    else
                    {
                        ViewData["Subdistricts"] = new SelectList(_context.Subdistricts
                            .Where(d => d.District.NameInThai == t.Venue_district)
                            .ToList()
                            .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", t.Venue_subdistrict);
                    }
                }

            }
            #endregion
            //ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.Status == Status.ใช้งาน).ToList(),"Id","Name");


            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Training training,
            string? pre_input,
            string? pre_file
            )
        {

            Training edited = _context.Trainings
                .FirstOrDefault(t => t.Id == training.Id);

            string picture_name = null;
            if (training.ImageFile != null)
            {
                picture_name = Uploadfile(training.ImageFile, "img");
                edited.Training_picture = picture_name;
            }
            else
            {
                if (pre_input == null)
                {
                    edited.Training_picture = picture_name;
                }
            }

            string file_name = null;
            if (training.ScheduleFile != null)
            {
                file_name = Uploadfile(training.ScheduleFile, "file");
                edited.Schedule_file = file_name;
            }
            else
            {
                if (pre_file == null)
                {
                    edited.Schedule_file = file_name;
                }
            }


            if (training.ScheduleFile != null)
                file_name = Uploadfile(training.ScheduleFile, "file");

            edited.Name = training.Name;
            edited.Description = training.Description;
            edited.Lecturer = training.Lecturer;
            edited.Objectives = training.Objectives;
            //edited.Training_picture = picture_name;
            //edited.Schedule_file = file_name;
            //edited.State = State_Train.โพสต์แล้ว,
            edited.CategId = training.CategId;
            ////Train Date
            //edited.Start_train_Date = training.Start_train_Date;
            //edited.End_train_Date = training.End_train_Date;
            ////
            //edited.Hours = training.Hours;
            ////Enroll Date
            //edited.Start_enroll_Date = training.Start_enroll_Date;
            //edited.End_enroll_Date = training.End_enroll_Date;
            ////
            edited.Cost = training.Cost;
            edited.BankId = training.BankId;
            edited.Bank_account_number = training.Bank_account_number;
            edited.Bank_account_holder = training.Bank_account_holder;
            edited.NumberOfParticipants = training.NumberOfParticipants;
            edited.Venue_street = training.Venue_street;
            edited.Venue_subdistrict = training.Venue_subdistrict;
            edited.Venue_district = training.Venue_district;
            edited.Venue_province = training.Venue_province;
            edited.Venue_zipcode = training.Venue_zipcode;

            _context.Trainings.Update(edited);
            _context.SaveChanges();

            //edited.OwnerId = training.OwnerId
            //};
            //_unitofWork.Training.Insert(t);
            //_unitofWork.Save();


            return RedirectToAction(nameof(Detail), new { Id = training.Id });
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if (user.role == Role.Admin)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.Where(c => c.Status == Status.ใช้งาน)
                .ToList().OrderBy(s => s.Name), "Id", "Name");
            ViewBag.Banks = new SelectList(
                _context.Banks.Where(b => b.Status == Status.ใช้งาน)
                .ToList().OrderBy(s => s.Name), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Training training)
        {
            string picture_name = null;
            string file_name = null;
            if (training.ImageFile != null)
                picture_name = Uploadfile(training.ImageFile, "img");
            if (training.ScheduleFile != null) 
                file_name = Uploadfile(training.ScheduleFile, "file");

            Training t = new Training()
            {
                Name = training.Name,
                Description = training.Description,
                Lecturer = training.Lecturer,
                Objectives = training.Objectives,
                Training_picture = picture_name,
                Schedule_file = file_name,
                State = State_Train.โพสต์แล้ว,
                CategId = training.CategId,
                Created_Date = DateTime.Now,
                Start_train_Date = training.Start_train_Date,
                End_train_Date = training.End_train_Date,
                Hours = training.Hours,
                Start_enroll_Date = training.Start_enroll_Date,
                End_enroll_Date = training.End_enroll_Date,
                Cost = training.Cost,
                BankId = training.BankId,
                Bank_account_number = training.Bank_account_number,
                Bank_account_holder = training.Bank_account_holder,
                NumberOfParticipants = training.NumberOfParticipants,
                Venue_street = training.Venue_street,
                Venue_subdistrict = training.Venue_subdistrict,
                Venue_district = training.Venue_district,
                Venue_province = training.Venue_province,
                Venue_zipcode = training.Venue_zipcode,
                OwnerId = training.OwnerId
            };
            _unitofWork.Training.Insert(t);
            _unitofWork.Save();
            return RedirectToAction(nameof(MyTraining));
        }
        private string Uploadfile(IFormFile file, string folder)
        {
            string filename = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "uploads/training/" + folder);
                filename = Guid.NewGuid().ToString("N").Substring(0, 16) + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return filename;
        }

    }
}
