using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Helpers;
using TrainingX_2.Models;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IUpdateService _updateService;
        public HomeController(ILogger<HomeController> logger,
            ApplicationDBContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUpdateService updateService
            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

            _updateService = updateService;

        }

        public async Task<IActionResult> Index(int? pageNumber,
            string? searchName,
            int? searchCategory,
            string? searchProvince,
            DateTime? startdate,
            DateTime? enddate,
            DateTime? enrolldate
            )
        {
            await _updateService.UpdateStateofTrainingAsync();
            #region ViewData
            ViewData["Categories"] = new SelectList(_context.Categories
                .Where(c => c.Status == Status.ใช้งาน)
                .ToList().OrderBy(s => s.Name), "Id", "Name");
            ViewData["Provinces"] = new SelectList(_context.Provinces
                .ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai");
            #endregion
            if (pageNumber == null)
                pageNumber = 1;

            var Trainings = from t in _context.Trainings.Where(t => t.State == State_Train.โพสต์แล้ว)
                            select t;

            if (!String.IsNullOrEmpty(searchName))
            {
                Trainings = Trainings.Where(t => t.Name!.Contains(searchName));
                //ViewData["searchName"] = searchName;
            }
            if (searchCategory != null)
            {
                Trainings = Trainings.Where(t => t.CategId == searchCategory);
                //ViewData["Categories"] = new SelectList(_context.Categories
                //.Where(c => c.Status == Status.ใช้งาน)
                //.ToList().OrderBy(s => s.Name), "Id", "Name", searchCategory);
            }
            if (!String.IsNullOrEmpty(searchProvince))
            {
                Trainings = Trainings.Where(t => t.Venue_province!.Contains(searchProvince));
                //ViewData["Provinces"] = new SelectList(_context.Provinces
                //.ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai"
                //, searchProvince);
            }
            if (startdate.HasValue)
            {
                Trainings = Trainings.Where(s => s.Start_train_Date <= startdate);
                Trainings = Trainings.Where(s => s.End_train_Date >= enddate);
                //ViewData["traindate"] = traindate.Value.ToString("yyyy-MM-dd");
                //ViewData["enddate"] = enddate.Value.ToString("yyyy-MM-dd");
            }
            if (enrolldate.HasValue)
            {
                Trainings = Trainings.Where(e => e.Start_enroll_Date.Value.Date <= enrolldate.Value.Date);
                Trainings = Trainings.Where(e => e.End_enroll_Date.Value.Date >= enrolldate.Value.Date);
                //ViewData["enrolldate"] = enrolldate.Value.ToString("yyyy-MM-dd");
            }

            int pageSize = 12;
            return View(await TrainingX_2.Controllers.PaginatedList<Training>.CreateAsync(Trainings.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult MemberIndex()
        {
            return View();
        }
        public IActionResult AdminIndex()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NeedToLoginPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}