using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendimientos
{
    public class IndexModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public IndexModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public List<Emprendimiento> Emprendimientos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Emprendimientos = await _context.Emprendimientos
                .Include(e => e.Emprendedor)
                .Include(e => e.Ciudad)
                .Include(e => e.ProductosServicios)
                .OrderByDescending(e => e.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var emprendimiento = await _context.Emprendimientos
                .Include(e => e.ProductosServicios)
                .Include(e => e.RedesSociales)
                .FirstOrDefaultAsync(e => e.EmprendimientoId == id);

            if (emprendimiento is null)
            {
                TempData["Error"] = "No se encontró el emprendimiento.";
                return RedirectToPage();
            }

            if (emprendimiento.ProductosServicios.Any())
            {
                TempData["Error"] = $"No se puede eliminar '{emprendimiento.NombreMarca}': tiene {emprendimiento.ProductosServicios.Count} producto(s)/servicio(s) asociado(s). Elimínalos primero.";
                return RedirectToPage();
            }

            // Las redes sociales sí se eliminan en cascada (no tienen dependientes propios)
            _context.RedesSociales.RemoveRange(emprendimiento.RedesSociales);
            _context.Emprendimientos.Remove(emprendimiento);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{emprendimiento.NombreMarca}' eliminado correctamente.";
            return RedirectToPage();
        }
    }
}