using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Base;
using TrainingX_2.ViewModels.CategoriesViewModel;

namespace TrainingX_2.Controllers.Base
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitofWork;
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public CategoryController(ApplicationDBContext context,
            IUnitOfWork unitofWork,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _context = context;
            _unitofWork = unitofWork;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: CategoryController
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

            var Categories = from t in _context.Categories
                             select t;

            var model = _unitofWork.Category.GetAll();
            var vm = _mapper.Map<List<CategoriesViewModel>>(model);

            int pageSize = 10;
            return View(await TrainingX_2.Controllers.PaginatedList<Category>.CreateAsync(Categories.AsNoTracking(), pageNumber ?? 1, pageSize)); ;
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public JsonResult MacthCountByName(string key)
        {
            var count = 0;
            count = _context.Categories.Where(x => x.Name == key).Count();
            return Json(count);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        public JsonResult Edit(Category c)
        {
            int response = 0;
            //var n = null;

            var model = _mapper.Map<Category>(c);
            //var old = _context.Categories.FirstOrDefault(x => x.Id == c.Id);
            //old.Name = c.Name;

            if (ModelState.IsValid)
            {
                //var old = _context.Categories.FirstOrDefault(x => x.Id == c.Id);

                response = -1;
                if (model != null)
                {
                    response = -2;
                    _unitofWork.Category.Update(model);
                    //_context.Categories.Update(old);
                    _unitofWork.Save();
                    //response = _context.SaveChanges();

                    response = 1;
                    //old = _context.Categories.FirstOrDefault(x => x.Id == c.Id);
                }
            }
            return Json(new { response = response, currData = _mapper.Map<Category>(c)});
        }

        [HttpPost]
        public JsonResult ChangeStatus(int Id)
        {
            var updated = _context.Categories.FirstOrDefault(x => x.Id == Id);
            //bool temp = true;
            int response = 0;

            if (updated != null)
            {
                //temp = (bool)updated.Status;
                //updated.Status = !temp;
                if (updated.Status == Status.ใช้งาน) updated.Status = Status.ไม่ใช้งาน;
                else updated.Status = Status.ใช้งาน;

                _context.Categories.Update(updated);
                response = _context.SaveChanges();
                updated = _context.Categories.FirstOrDefault(x => x.Id == Id);
            }
            return Json(new { currAffect = response, currData = updated });

        }
    }
}
