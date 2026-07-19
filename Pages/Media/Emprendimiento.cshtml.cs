using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Data;

namespace VisionNaranja.Pages.Media
{
    public class EmprendimientoModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public EmprendimientoModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var emprendimiento = await _context.Emprendimientos.FindAsync(id);

            if (emprendimiento?.Logo is null)
                return NotFound();

            return File(emprendimiento.Logo, emprendimiento.LogoMime ?? "image/png");
        }
    }
}