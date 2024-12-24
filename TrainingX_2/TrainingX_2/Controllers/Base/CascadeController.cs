using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Base;

namespace TrainingX_2.Controllers.Base
{
    public class CascadeController : Controller
    {
        private readonly ApplicationDBContext _context;
        public CascadeController(ApplicationDBContext context)
        {
            _context = context;
        }
        public JsonResult Province()
        {
            var prv = _context.Provinces.ToList().OrderBy(s => s.NameInThai);
            return new JsonResult(prv);
        }
        public JsonResult District(string name)
        {
            var dst = _context.Districts.Where(d => d.Province.NameInThai == name).ToList().OrderBy(s => s.NameInThai);
            return new JsonResult(dst);
        }
        public JsonResult Subdistrict(string name)
        {
            var sdt = _context.Subdistricts.Where(s => s.District.NameInThai == name).ToList().OrderBy(s => s.NameInThai);
            return new JsonResult(sdt);
        }
        public JsonResult Zipcode(string name)
        {
            var zpc = _context.Subdistricts.FirstOrDefault(s => s.NameInThai == name);
            return new JsonResult(zpc);
        }
    }
}
