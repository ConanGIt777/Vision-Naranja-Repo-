using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Productos
{
    public class EditModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public EditModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public ProductosServicio? Producto { get; set; }
        public List<Emprendimiento> Emprendimientos { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public string? Error { get; set; }

        private async Task CargarListasAsync()
        {
            Emprendimientos = await _context.Emprendimientos.Where(e => e.Activo).OrderBy(e => e.NombreMarca).ToListAsync();
            Categorias = await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task OnGetAsync(int id)
        {
            Producto = await _context.ProductosServicios
                .Include(p => p.MediaProductos)
                .FirstOrDefaultAsync(p => p.ProductoServicioId == id);
            await CargarListasAsync();
        }

        public async Task<IActionResult> OnPostGuardarAsync(
            int id, int emprendimientoId, int categoriaId, string nombre, string tipoOferta,
            string? descripcion, decimal? precio, bool disponible)
        {
            Producto = await _context.ProductosServicios
                .Include(p => p.MediaProductos)
                .FirstOrDefaultAsync(p => p.ProductoServicioId == id);
            await CargarListasAsync();

            if (Producto is null) return Page();

            // 1. Intentamos parsear el string a byte de forma segura
            bool esTipoOfertaValido = byte.TryParse(tipoOferta, out byte tipoOfertaByte);

            // 2. Validamos incluyendo que el tipo de oferta sea un número válido
            if (emprendimientoId == 0 || categoriaId == 0 || string.IsNullOrWhiteSpace(nombre) || !esTipoOfertaValido)
            {
                Error = "Emprendimiento, Categoría, Nombre y Tipo de Oferta son obligatorios.";
                return Page();
            }

            Producto.EmprendimientoId = emprendimientoId;
            Producto.CategoriaId = categoriaId;
            Producto.Nombre = nombre.Trim();
            Producto.TipoOferta = tipoOfertaByte; // <-- Asignamos el byte parseado correctamente
            Producto.Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
            Producto.Precio = precio;
            Producto.Disponible = disponible;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Cambios guardados correctamente.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostSubirImagenesAsync(int id, List<IFormFile> fotos)
        {
            var yaTienePortada = await _context.MediaProductos.AnyAsync(m => m.ProductoServicioId == id && m.EsPortada);
            var ordenActual = await _context.MediaProductos
                .Where(m => m.ProductoServicioId == id)
                .Select(m => (int?)m.Orden)
                .MaxAsync() ?? 0;

            if (fotos is null || fotos.Count == 0)
            {
                TempData["Error"] = "No se seleccionó ninguna foto.";
                return RedirectToPage(new { id });
            }

            foreach (var foto in fotos.Where(f => f.Length > 0))
            {
                using var ms = new MemoryStream();
                await foto.CopyToAsync(ms);

                ordenActual++;
                _context.MediaProductos.Add(new MediaProducto
                {
                    ProductoServicioId = id,
                    TipoMedia = "Imagen",
                    Datos = ms.ToArray(),
                    MimeType = foto.ContentType,
                    EsPortada = !yaTienePortada,
                    Orden = (byte)ordenActual,
                    FechaSubida = DateTime.Now
                });
                yaTienePortada = true;
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"{fotos.Count(f => f.Length > 0)} foto(s) subida(s) correctamente.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostMarcarPortadaAsync(int id, int mediaId)
        {
            var mediaActual = await _context.MediaProductos
                .Where(m => m.ProductoServicioId == id)
                .ToListAsync();

            foreach (var m in mediaActual)
                m.EsPortada = (m.MediaId == mediaId);

            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostEliminarImagenAsync(int id, int mediaId)
        {
            var media = await _context.MediaProductos.FindAsync(mediaId);
            if (media != null)
            {
                var eraPortada = media.EsPortada;
                _context.MediaProductos.Remove(media);
                await _context.SaveChangesAsync();

                if (eraPortada)
                {
                    var siguiente = await _context.MediaProductos
                        .Where(m => m.ProductoServicioId == id)
                        .OrderBy(m => m.Orden)
                        .FirstOrDefaultAsync();

                    if (siguiente != null)
                    {
                        siguiente.EsPortada = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToPage(new { id });
        }
    }
}