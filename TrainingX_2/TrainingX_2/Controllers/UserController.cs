using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Base;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(ApplicationDBContext context,
            IWebHostEnvironment hostEnvironment,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index(
            int? pageNumber)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if (user.role == Role.Member)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }

            if (pageNumber == null)
                pageNumber = 1;

            var Users = from u in _context.Users.Where(x => x.role != Role.Admin)
                        select u;

            int pageSize = 10;
            return View(await TrainingX_2.Controllers.PaginatedList<User>.CreateAsync(Users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Detail(string? Id)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if (user.role == Role.Member)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }

            if (Id == null) return BadRequest();
            var student = await _context.Users.FirstOrDefaultAsync(c => c.Id == Id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        public JsonResult ChangeStatus(string Id)
        {
            var updated = _context.Users.FirstOrDefault(x => x.Id == Id);
            //Status temp = Status.ใช้งาน;
            int response = 0;

            if (updated != null)
            {
                //temp = (Status)updated.status;
                if (updated.Status_user == Status.ใช้งาน) updated.Status_user = Status.ไม่ใช้งาน;
                else updated.Status_user = Status.ใช้งาน;
                _context.Users.Update(updated);
                response = _context.SaveChanges();
                updated = _context.Users.FirstOrDefault(x => x.Id == Id);
            }
            return Json(new { currAffect = response, currData = updated, currId = Id });

        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            #region ViewData
            if (user.Address_province == null)
            {
                ViewData["Provinces"] = new SelectList(_context.Provinces.ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_province);

            }
            else
            {
                ViewData["Provinces"] = new SelectList(_context.Provinces.ToList().OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_province);
                if (user.Address_district == null)
                {
                    ViewData["Districts"] = new SelectList(_context.Districts
                        .Where(d => d.Province.NameInThai == user.Address_province)
                        .ToList()
                        .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_district);
                }
                else
                {
                    ViewData["Districts"] = new SelectList(_context.Districts
                        .Where(d => d.Province.NameInThai == user.Address_province)
                        .ToList()
                        .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_district);

                    if (user.Address_subdistrict == null)
                    {
                        ViewData["Subdistricts"] = new SelectList(_context.Subdistricts
                            .Where(d => d.District.NameInThai == user.Address_district)
                            .ToList()
                            .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_subdistrict);
                    }
                    else
                    {
                        ViewData["Subdistricts"] = new SelectList(_context.Subdistricts
                            .Where(d => d.District.NameInThai == user.Address_district)
                            .ToList()
                            .OrderBy(s => s.NameInThai), "NameInThai", "NameInThai", user.Address_subdistrict);
                    }
                }

            }
            #endregion

            //_userManager.GetUserId(User)
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User us, string? pre_input)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == us.Id);

            string picture_name = null;
            if (us.ImageFile != null)
            {
                picture_name = Uploadfile(us.ImageFile, "profile");
                user.Profile_picture = picture_name;
            }
            else
            {
                if (pre_input == null)
                {
                    user.Profile_picture = picture_name;
                }
            }

            user.Name_title = us.Name_title;
            user.FirstName = us.FirstName;
            user.LastName = us.LastName;
            user.Birthday = us.Birthday;
            user.PhoneNumber = us.PhoneNumber;
            user.Address_street = us.Address_street;
            user.Address_province = us.Address_province;
            user.Address_district = us.Address_district;
            user.Address_subdistrict = us.Address_subdistrict;
            user.Address_zipcode = us.Address_zipcode;

            _context.Users.Update(user);
            _context.SaveChanges();


            return RedirectToAction(nameof(Profile));
        }

        private string Uploadfile(IFormFile file, string folder)
        {
            string filename = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "uploads/" + folder);
                filename = Guid.NewGuid().ToString() + "-" + file.FileName;
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
