using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Data;

namespace VisionNaranja.Pages.Media
{
    public class ProductoModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public ProductoModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // "id" es el MediaId (clave única de cada imagen individual), no el ProductoServicioId
            var media = await _context.MediaProductos.FindAsync(id);

            if (media?.Datos is null)
                return NotFound();

            return File(media.Datos, media.MimeType ?? "image/jpeg");
        }
    }
}