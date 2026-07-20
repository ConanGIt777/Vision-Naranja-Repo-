using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;

namespace VisionNaranja.Pages.Admin.Productos
{
    public class ProductoListItem
    {
        public int ProductoServicioId { get; set; }
        public string Nombre { get; set; } = null!;
        public byte TipoOferta { get; set; }
        public string TipoOfertaTexto => TipoOferta == 1 ? "Producto" : "Servicio";
        public decimal? Precio { get; set; }
        public bool Disponible { get; set; }
        public VisionNaranja.Models.Emprendimiento? Emprendimiento { get; set; }
        public VisionNaranja.Models.Categoria? Categoria { get; set; }
        public int? PortadaMediaId { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public IndexModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public List<ProductoListItem> Productos { get; set; } = new();

        public async Task OnGetAsync()
        {
            var productos = await _context.ProductosServicios
                .Include(p => p.Emprendimiento)
                .Include(p => p.Categoria)
                .Include(p => p.MediaProductos)
                .OrderByDescending(p => p.FechaRegistro)
                .ToListAsync();

            Productos = productos.Select(p => new ProductoListItem
            {
                ProductoServicioId = p.ProductoServicioId,
                Nombre = p.Nombre,
                
                // Traducimos el número byte a su texto correspondiente de manera limpia
                TipoOferta = p.TipoOferta,
                
                Precio = p.Precio,
                Disponible = p.Disponible,
                Emprendimiento = p.Emprendimiento,
                Categoria = p.Categoria,
                PortadaMediaId = p.MediaProductos.FirstOrDefault(m => m.EsPortada)?.MediaId
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var producto = await _context.ProductosServicios
                .Include(p => p.MediaProductos)
                .FirstOrDefaultAsync(p => p.ProductoServicioId == id);

            if (producto is null)
            {
                TempData["Error"] = "No se encontró el producto.";
                return RedirectToPage();
            }

            _context.MediaProductos.RemoveRange(producto.MediaProductos);
            _context.ProductosServicios.Remove(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{producto.Nombre}' eliminado correctamente.";
            return RedirectToPage();
        }
    }
}