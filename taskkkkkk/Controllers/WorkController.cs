using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    public class WorkController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public WorkController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = await _appDbContext.Sliders.ToListAsync(),
                Categories = await _appDbContext.Categories.Where(c => !c.IsDeleted).ToListAsync(),
                Services = await _appDbContext.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceImages)

                .OrderByDescending(s => s.Id)
                .Where(s => !s.IsDeleted)
                .Take(8)
                .ToListAsync()
            };
            return View(homeVM);
        }
    }
}
