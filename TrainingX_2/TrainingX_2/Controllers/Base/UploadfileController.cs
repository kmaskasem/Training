using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Controllers.Base
{
    public class UploadfileController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private object filename;
        public UploadfileController(ApplicationDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public string Uploadfile(IFormFile ImageFile)
        {
            string filename = null;
            if (ImageFile != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "images\\profile");
                filename = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;
                string filePath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
            }
            return filename;
        }
    }
}
