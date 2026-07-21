using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;
using VisionNaranja.Services;

namespace VisionNaranja.Pages.Catalogo
{
    public class ProductoModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public ProductoModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public ProductosServicio? Producto { get; set; }
        public string? ImagenUrl { get; set; }
        public List<string> ImagenesProducto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.ProductosServicios
                .Include(p => p.Categoria)
                .Include(p => p.Emprendimiento)
                    .ThenInclude(e => e.Ciudad)
                .FirstOrDefaultAsync(p => p.ProductoServicioId == id);

            if (Producto is null)
                return NotFound();

            // Obtener todas las imágenes del producto
            var mediasProducto = await _context.MediaProductos
                .Where(m => m.ProductoServicioId == id && m.TipoMedia != "Video")
                .OrderBy(m => m.Orden)
                .Select(m => m.MediaId)
                .ToListAsync();

            // Agregar portada primero si existe
            var portada = mediasProducto.FirstOrDefault();
            if (portada > 0)
            {
                ImagenUrl = $"/Media/Producto/{portada}";
                foreach (var mediaId in mediasProducto)
                {
                    ImagenesProducto.Add($"/Media/Producto/{mediaId}");
                }
            }
            else if (Producto.Emprendimiento.Logo != null)
            {
                ImagenUrl = $"/Media/Emprendimiento/{Producto.EmprendimientoId}";
            }
            else if (!string.IsNullOrWhiteSpace(Producto.Emprendimiento.EnlaceDriveFotos))
            {
                ImagenUrl = CatalogoService.ConvertirUrlDrive(Producto.Emprendimiento.EnlaceDriveFotos);
            }

            return Page();
        }
    }
}
