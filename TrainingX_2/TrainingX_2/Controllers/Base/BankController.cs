using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TrainingX_2.Controllers.Base
{

    public class BankController : Controller
    {

        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitofWork;
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public BankController(ApplicationDBContext context,
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
        // GET: BankNameController
        public async Task<IActionResult> Index(
            int? pageNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }
            else if(user.role == Role.Member) 
            {
                return RedirectToAction("NeedToLoginPage", "Home");
            }

            if (pageNumber == null)
                pageNumber = 1;

            var Banks = from t in _context.Banks
                        select t;

            int pageSize = 5;
            return View(await TrainingX_2.Controllers.PaginatedList<Bank>.CreateAsync(Banks.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // POST: BankNameController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")]  Bank bank)
        {
            var b = new Bank()
            {
                Name = bank.Name
            };

            _context.Banks.Add(b);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public JsonResult MacthCountByName(string key)
        {
            var count = 0;
            count = _context.Banks.Where(x => x.Name == key).Count();
            return Json(count);
        }

        // POST: BankNameController/Edit/5
        [HttpPost]
        public JsonResult Edit(Bank b)
        {
            int response = 0;
            if (ModelState.IsValid)
            {
                response = -1;
                var old = _context.Banks.FirstOrDefault(x => x.Id == b.Id);

                response = -2;
                if (old != null)
                {
                    response = -3;
                    old.Name = b.Name;
                    _context.Banks.Update(old);
                    response = _context.SaveChanges();
                }
            }
            return Json(new { response = response, currData = b });
        }

        [HttpPost]
        public JsonResult ChangeStatus(int Id)
        {
            var updated = _context.Banks.FirstOrDefault(x => x.Id == Id);
            //Status temp = Status.ใช้งาน;
            int response = 0;

            if (updated != null)
            {
                //temp = (Status)updated.status;
                if (updated.Status == Status.ใช้งาน) updated.Status = Status.ไม่ใช้งาน;
                else updated.Status = Status.ใช้งาน;
                _context.Banks.Update(updated);
                response = _context.SaveChanges();
                updated = _context.Banks.FirstOrDefault(x => x.Id == Id);
            }
            return Json(new { currAffect = response, currData = updated });

        }
    }

}
