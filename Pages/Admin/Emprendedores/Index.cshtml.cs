using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendedores
{
    public class IndexModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public IndexModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public List<Emprendedor> Emprendedores { get; set; } = new();

        public async Task OnGetAsync()
        {
            Emprendedores = await _context.Emprendedores
                .Include(e => e.Emprendimientos)
                .OrderByDescending(e => e.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var emprendedor = await _context.Emprendedores
                .Include(e => e.Emprendimientos)
                .FirstOrDefaultAsync(e => e.EmprendedorId == id);

            if (emprendedor is null)
            {
                TempData["Error"] = "No se encontró el emprendedor.";
                return RedirectToPage();
            }

            if (emprendedor.Emprendimientos.Any())
            {
                TempData["Error"] = $"No se puede eliminar a '{emprendedor.NombreCompleto}': tiene {emprendedor.Emprendimientos.Count} emprendimiento(s) asociado(s). Elimínalos primero.";
                return RedirectToPage();
            }

            _context.Emprendedores.Remove(emprendedor);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{emprendedor.NombreCompleto}' eliminado correctamente.";
            return RedirectToPage();
        }
    }
}