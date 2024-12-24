using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Base;

namespace TrainingX_2.Controllers.Base
{
    public class TopicAssessmentController : Controller
    {
        private readonly ApplicationDBContext _context;
        public TopicAssessmentController(ApplicationDBContext context)
        {
            _context = context;
        }
        // GET: TopicAssessmentController
        public async Task<IActionResult> Index(
            int? pageNumber)
        {
            if (pageNumber == null)
                pageNumber = 1;

            var topics = from t in _context.TopicAssessments
                         select t;

            int pageSize = 5;
            return View(await TrainingX_2.Controllers.PaginatedList<TopicAssessment>.CreateAsync(topics.AsNoTracking(), pageNumber ?? 1, pageSize)); ;
        }

        // POST: TopicAssessmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TopicAssessment topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(topic);
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
            count = _context.TopicAssessments.Where(x => x.Name == key).Count();
            return Json(count);
        }

        // POST: TopicAssessmentController/Edit/5
        [HttpPost]
        public JsonResult Edit(TopicAssessment t)
        {
            int response = 0;
            if (ModelState.IsValid)
            {
                var old = _context.TopicAssessments.FirstOrDefault(x => x.Id == t.Id);

                if (old != null)
                {
                    old.Name = t.Name;
                    _context.TopicAssessments.Update(old);
                    response = _context.SaveChanges();
                }
            }
            return Json(new { response = response, currData = t });
        }

        [HttpPost]
        public JsonResult ChangeStatus(int Id)
        {
            var updated = _context.TopicAssessments.FirstOrDefault(x => x.Id == Id);
            int response = 0;

            if (updated != null)
            {
                //temp = (Status)updated.status;
                if (updated.Status == Status.ใช้งาน) updated.Status = Status.ไม่ใช้งาน;
                else updated.Status = Status.ใช้งาน;
                _context.TopicAssessments.Update(updated);
                response = _context.SaveChanges();
                updated = _context.TopicAssessments.FirstOrDefault(x => x.Id == Id);
            }
            return Json(new { currAffect = response, currData = updated });

        }
    }
}
