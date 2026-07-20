using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Productos
{
    public class CreateModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public CreateModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public List<Emprendimiento> Emprendimientos { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            Emprendimientos = await _context.Emprendimientos.Where(e => e.Activo).OrderBy(e => e.NombreMarca).ToListAsync();
            Categorias = await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(
            int emprendimientoId, int categoriaId, string nombre, string tipoOferta,
            string? descripcion, decimal? precio, bool disponible)
        {
            await OnGetAsync();

            // 1. Intentamos parsear el string a byte de forma segura
            bool esTipoOfertaValido = byte.TryParse(tipoOferta, out byte tipoOfertaByte);

            // 2. Validamos los campos obligatorios y que la conversión haya sido exitosa
            if (emprendimientoId == 0 || categoriaId == 0 || string.IsNullOrWhiteSpace(nombre) || !esTipoOfertaValido)
            {
                Error = "Emprendimiento, Categoría, Nombre y Tipo de Oferta válido son obligatorios.";
                return Page();
            }

            var producto = new ProductosServicio
            {
                EmprendimientoId = emprendimientoId,
                CategoriaId = categoriaId,
                Nombre = nombre.Trim(),
                TipoOferta = tipoOfertaByte, // <-- Asignamos la variable byte ya parseada
                Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                Precio = precio,
                Disponible = disponible,
                FechaRegistro = DateTime.Now
            };

            _context.ProductosServicios.Add(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{producto.Nombre}' creado. Ahora puedes subir sus fotos.";
            return RedirectToPage("Edit", new { id = producto.ProductoServicioId });
        }
    }
}