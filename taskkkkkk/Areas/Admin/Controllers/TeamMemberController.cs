using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModel;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context;


        public TeamMemberController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> teamMembers = await _context.TeamMembers.ToListAsync();
            return View(teamMembers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExists = await _context.TeamMembers.AnyAsync(c =>
            c.Name.ToLower().Trim() == teamMember.Name.ToLower().Trim());


            if (isExists)
            {
                ModelState.AddModelError("Name", "Team member name already exists");
                return View();
            }
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);

            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        [HttpPost]
        public IActionResult Update(TeamMember teamMember)
        {
            TeamMember? editedTeamMember = _context.TeamMembers.Find(teamMember.Id);
            if (editedTeamMember == null)
            {
                return NotFound();
            }
            editedTeamMember.Name = teamMember.Name;
            editedTeamMember.Image = teamMember.Image;
            editedTeamMember.Profession = teamMember.Profession;
            _context.TeamMembers.Update(editedTeamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);
            if (teamMember == null)
            {
                return NotFound();
            }
            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
