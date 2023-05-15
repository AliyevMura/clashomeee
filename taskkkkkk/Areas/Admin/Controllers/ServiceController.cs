using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModel;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

    
namespace WebFrontToBack.Areas.Admin.Controllers
{
    
    public class ServiceController : Controller
    {
       
        private readonly AppDbContext _context;

        public Task<List<Category>> Categories { get; private set; }
        public Service Service { get; private set; }

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Service> services = await _context.Services.ToListAsync();
            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Service service = new Service();
            ServiceVM vm = new ServiceVM();
            {
                Categories =_context.Categories.Where(c => c.IsDeleted==false).ToListAsync();
                Service = service;
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceVM service)
        {
            
            //service.Services.Name;

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExists = await _context.Services.AnyAsync(c =>
            c.Name.ToLower().Trim() == service.Services.Name.ToLower().Trim());
           

            if (isExists)
            {
                ModelState.AddModelError("Name", "Service name already exists");
                return View();
            }
            await _context.Services.AddAsync(service.Services);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int Id)
        {
            Service? service = _context.Services.Find(Id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult Update(Service service)
        {
            
            Service? editedService = _context.Services.Find(service.Id);
            if (editedService == null)
            {
                return NotFound();
            }
            editedService.Name = service.Name;
            _context.Services.Update(editedService);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            Service? service = _context.Services.Find(Id);
            if (service == null)
            {
                return NotFound();
            }
            _context.Services.Remove(service);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
