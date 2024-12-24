using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.Linq;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Helpers;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IUnitOfWork _unitofWork;
        private IMapper _mapper;

        private readonly IUpdateService _updateService;
        public EnrollmentController(ApplicationDBContext context,
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
        public async Task<IActionResult> ParticipantsList(int? pageNumber,
            string? searchTrainingName,
            int? searchTrainingCategory,
            Status_Enroll? searchCheckStatus,
            string? searchParticipantName,
            DateTime? searchEnrollDate
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
            var MyTrainingList = _context.Trainings
                .Include("Category")
                .Where(t => t.OwnerId == _userManager.GetUserId(User))
                .ToList().OrderBy(s => s.Name);

            var CategList = MyTrainingList
                .Select(c => c.Category)
                .Distinct()
                .ToList().OrderBy(s => s.Name);

            ViewData["Categories"] = new SelectList(CategList,
                "Id","Name");

            ViewData["Trainings"] = new SelectList(MyTrainingList, "Name", "Name");
            ViewData["CheckStatuses"] = Enum.GetValues(typeof(Status_Enroll))
                .Cast<Status_Enroll>().Where(c => c != Status_Enroll.ยกเลิกการลงทะเบียน)
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });
            #endregion

            if (pageNumber == null)
                pageNumber = 1;


            string currentUserId = _userManager.GetUserId(User);

            var Enrollments = _context.Enrollments
                .Include("Training")
                .Include("Participant")
                .Where(e => e.Training.OwnerId == currentUserId);
                //.Where(e => e.Training.State == State_Train.โพสต์แล้ว);
            //.Include(e => e.Training)
            //    .Include(u => u.Participant)
            //    .Where(e => e.Training.OwnerId == currentUserId);


            if (!String.IsNullOrEmpty(searchTrainingName))
            {
                Enrollments = Enrollments.Where(t => t.Training.Name!.Contains(searchTrainingName));
                ViewData["Cost"] = (_context.Trainings.FirstOrDefault(t => t.Name == searchTrainingName)).Cost;
                ViewData["Trainings"] = new SelectList(_context.Trainings
                    .Where(t => t.OwnerId == _userManager.GetUserId(User))
                    .ToList().OrderBy(s => s.Name), "Name", "Name", searchTrainingName);
            }
            if (searchTrainingCategory != null)
            {
                Enrollments = Enrollments.Where(t => t.Training.CategId == searchTrainingCategory);
            }
            if (searchCheckStatus != null)
                Enrollments = Enrollments.Where(t => t.Check_status == searchCheckStatus);
            if (!String.IsNullOrEmpty(searchParticipantName))
            {
                Enrollments = Enrollments
                    .Where(t => t.Participant.FirstName!.Contains(searchParticipantName)
                    || t.Participant.LastName!.Contains(searchParticipantName));
            }
            if (searchEnrollDate.HasValue)
            {
                Enrollments = Enrollments.Where(e => e.Payment_date.Value.Date == searchEnrollDate.Value.Date);
            }

            int pageSize = 10;

            return View(await TrainingX_2.Controllers.PaginatedList<Enrollment>.CreateAsync(Enrollments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public JsonResult getTrainingToCheck(int cid)
        {
            var t = _context.Trainings
                //.Include("Category")
                .Where(t => t.OwnerId == _userManager.GetUserId(User))
                .Where(t=> t.CategId == cid)
                .ToList().OrderBy(s => s.Name);
            return new JsonResult(t);
        }
        public JsonResult getTrainingEnrolled(int cid)
        {
            var t = _context.Enrollments
                //.Include("Training")
                .Include(a => a.Training.Category)
                //.Include("Participant")
                .Where(e => e.ParticipantId == _userManager.GetUserId(User))
                .Where(e => e.Training.State == State_Train.โพสต์แล้ว)
                .Where(e => e.Check_status != Status_Enroll.ยกเลิกการลงทะเบียน)
                
                .Select(c => c.Training)
                .Where(c => c.CategId == cid)
                .Distinct()
                .ToList().OrderBy(s => s.Name);

            //if (TList.Count == 0) { return NotFound("No products found for this user."); }
            //Enrollments = Enrollments.Where(t => t.Training.CategId == searchTrainingCategory);

            return new JsonResult(t);
        }

        public async Task<IActionResult> MyEnrollmentsList(int? pageNumber,
            string? searchTrainingName,
            int? searchTrainingCategory
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

            string currentUserId = _userManager.GetUserId(User);
            #region ViewData
            var EnrollList = _context.Enrollments
                //.Include("Training")
                .Include(a => a.Training.Category)
                .Include("Participant")
                .Where(e => e.ParticipantId == currentUserId)
                .Where(e => e.Training.State == State_Train.โพสต์แล้ว)
                .Where(e => e.Check_status != Status_Enroll.ยกเลิกการลงทะเบียน)
                .ToList().OrderBy(s => s.Training.Name);

            //var TList = _context.Enrollments
            //    //.Include("Training")
            //    .Include(a => a.Training.Category)
            //    .Where(e => e.ParticipantId == currentUserId)
            //    .Where(e => e.Training.State == State_Train.โพสต์แล้ว)
            //    .Where(e => e.Check_status != Status_Enroll.ยกเลิกการลงทะเบียน)
            //    .Select(c => c.Training)
            //    .Where(c => c.CategId == 36)
            //    .Distinct()
            //    .ToList().OrderBy(s => s.Name);

            //if (TList == null) { }

            var CategList = EnrollList
                .Select(c => c.Training.Category)
                .Distinct()
                .ToList().OrderBy(s => s.Name);

            ViewData["Categories"] = new SelectList(
                CategList, "Id", "Name");
            ViewData["Trainings"] = new SelectList(
                EnrollList, "Training.Name", "Training.Name");
            #endregion

            if (pageNumber == null)
                pageNumber = 1;

            var Enrollments = _context.Enrollments
                //.Include("Training")
                .Include(a => a.Training.Category)
                .Include("Participant")
                .Where(e => e.ParticipantId == currentUserId)
                .Where(e => e.Training.State == State_Train.โพสต์แล้ว)
                .Where(e => e.Check_status != Status_Enroll.ยกเลิกการลงทะเบียน);
            //var Enrollments = from enrollment in _context.Enrollments
            //            join training in _context.Trainings on enrollment.TrainingId equals training.Id
            //            join category in _context.Categories on training.CategId equals category.Id
            //            where enrollment.ParticipantId == currentUserId
            //                  select enrollment;
            if (!String.IsNullOrEmpty(searchTrainingName))
            {
                Enrollments = Enrollments.Where(t => t.Training.Name!.Contains(searchTrainingName));
                //ViewData["Cost"] = (_context.Trainings.FirstOrDefault(t => t.Name == searchTrainingName)).Cost;
                ViewData["Trainings"] = new SelectList(
                EnrollList, "Training.Name", "Training.Name", searchTrainingName);
            }
            if (searchTrainingCategory != null)
            {
                Enrollments = Enrollments.Where(t => t.Training.CategId == searchTrainingCategory);
            }


            int pageSize = 10;

            return View(await TrainingX_2.Controllers.PaginatedList<Enrollment>.CreateAsync(Enrollments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> MyHistoryEnrollmentsList(int? pageNumber,
            string? searchTrainingName,
            int? searchTrainingCategory,
            Status_Enroll? searchCheckStatus,
            string? searchParticipantName,
            DateTime? searchEnrollDate
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
            ViewData["Categories"] = new SelectList(_context.Categories.Where(c => c.Status == Status.ใช้งาน).ToList().OrderBy(s => s.Name), "Id", "Name");
            ViewData["Trainings"] = new SelectList(_context.Trainings
                .Where(t => t.OwnerId == _userManager.GetUserId(User))
                .ToList().OrderBy(s => s.Name), "Name", "Name");
            ViewData["CheckStatuses"] = Enum.GetValues(typeof(Status_Enroll))
                .Cast<Status_Enroll>().Where(c => c != Status_Enroll.ยกเลิกการลงทะเบียน)
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });
            #endregion

            if (pageNumber == null)
                pageNumber = 1;


            string currentUserId = _userManager.GetUserId(User);

            var Enrollments = _context.Enrollments
                .Include(a => a.Training.Category)
                .Where(e => e.ParticipantId == currentUserId)
                .Where(e => e.Training.State == State_Train.สำเร็จ || e.Check_status == Status_Enroll.ยกเลิกการลงทะเบียน);



            int pageSize = 10;

            return View(await TrainingX_2.Controllers.PaginatedList<Enrollment>.CreateAsync(Enrollments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Check(
            Enrollment enrollment
            )
        {

            Enrollment e = _context.Enrollments
                .FirstOrDefault(t => t.Id == enrollment.Id);

            e.Check_status = enrollment.Check_status;
            e.Note = enrollment.Note;
            _context.Enrollments.Update(e);
            _context.SaveChanges();
            return RedirectToAction(nameof(ParticipantsList));

        }

        public async Task<IActionResult> Enroll(string Id)
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

            Training training = _context.Trainings
                .Include("Bank")
                .Include("Owner")
                .FirstOrDefault(t => t.Id == Id);

            ViewBag.TrainingId = training.Id;
            ViewBag.TrainingName = training.Name;
            ViewBag.TrainingOwner = training.Owner.FullName;
            if (training.BankId != null)
            {
                ViewBag.TrainingBank = training.Bank.Name;
            ViewBag.TrainingBankAccountHolder = training.Bank_account_holder;
            ViewBag.TrainingBankAccountNumber = training.Bank_account_number;

            }
            
            ViewBag.Cost = training.Cost;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(
            Enrollment enrollment,
            int? Cost
            )
        {
            string picture_name = null;
            if (enrollment.PaymentImageFile != null)
                picture_name = Uploadfile(enrollment.PaymentImageFile, "payments");

            Enrollment e = new Enrollment()
            {
                Payment_date = enrollment.Payment_date,
                TrainingId = enrollment.TrainingId,
                ParticipantId = enrollment.ParticipantId,
                Payment_picture = picture_name,
                Phone = enrollment.Phone,
                Enroll_date = DateTime.Now,
            };
            if (Cost == 0)
                e.Check_status = Status_Enroll.ตรวจสอบผ่าน;
            
            _unitofWork.Enrollment.Insert(e);
            _unitofWork.Save();
            return RedirectToAction("MyEnrollmentsList", "Enrollment");

        }

        private string Uploadfile(IFormFile file, string folder)
        {
            string filename = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "uploads/enrollment/" + folder);
                filename = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return filename;
        }

        public async Task<IActionResult> Cancel(string? Id)
        {
            if (Id == null) return BadRequest();
            var e = await _context.Enrollments.FirstOrDefaultAsync(c => c.Id == Id);
            if (e == null) return NotFound();
            e.Check_status = Status_Enroll.ยกเลิกการลงทะเบียน;
            _context.Enrollments.Update(e);
            _context.SaveChanges();
            return RedirectToAction(nameof(MyEnrollmentsList));
        }
        [HttpPost]
        public JsonResult UpdatePhoneNumber(string Id, string phone)
        {
            var updated = _context.Enrollments.FirstOrDefault(x => x.Id == Id);
            //Status temp = Status.ใช้งาน;
            int response = 0;

            if (updated != null)
            {
                updated.Phone = phone;
                _context.Enrollments.Update(updated);
                response = _context.SaveChanges();
                updated = _context.Enrollments.FirstOrDefault(x => x.Id == Id);

            }
            return Json(new { currAffect = response, currData = updated });
        }
    }
}
