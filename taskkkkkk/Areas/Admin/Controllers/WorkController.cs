using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _context;
        private Service work;

        public WorkController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Work> works = await _context.Works.ToListAsync();
            return View(works);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Work work)
        {
           

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExists = await _context.Works.AnyAsync(c =>
            c.Name.ToLower().Trim() == work.Name.ToLower().Trim());


            if (isExists)
            {
                ModelState.AddModelError("Name", "Service name already exists");
                return View();
            }
            await _context.Works.AddAsync(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int Id)
        {
            Work? work = _context.Works.Find(Id);

            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        [HttpPost]
        public IActionResult Update(Work work)
        {

            Work? editedWork = _context.Works.Find(work.Id);
            if (editedWork == null)
            {
                return NotFound();
            }
            editedWork.Name = work.Name;
            _context.Works.Update(editedWork);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            Work? service = _context.Works.Find(Id);
            if (work == null)
            {
                return NotFound();
            }
            _context.Services.Remove(work);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
