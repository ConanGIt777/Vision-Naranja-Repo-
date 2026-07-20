using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Data;

namespace VisionNaranja.Pages.Media
{
    public class EmprendedorModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public EmprendedorModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var emprendedor = await _context.Emprendedores.FindAsync(id);

            if (emprendedor?.FotoPerfil is null)
                return NotFound();

            return File(emprendedor.FotoPerfil, emprendedor.FotoPerfilMime ?? "image/jpeg");
        }
    }
}