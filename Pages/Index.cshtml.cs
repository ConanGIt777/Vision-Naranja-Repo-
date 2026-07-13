using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;
using VisionNaranja.Services;

namespace VisionNaranja.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CatalogoService _catalogoService;
        private readonly VisionNaranjaContext _context;

        public IndexModel(CatalogoService catalogoService, VisionNaranjaContext context)
        {
            _catalogoService = catalogoService;
            _context = context;
        }

        public List<VCatalogo> Destacados { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var emprendimientos = await _context.Emprendimientos
                    .Include(e => e.Emprendedor)
                    .Where(e => e.Activo && e.Emprendedor != null && e.Emprendedor.Activo)
                    .OrderByDescending(e => e.FechaRegistro)
                    .Take(4)
                    .ToListAsync();

                Destacados = emprendimientos
                    .Select(e => new VCatalogo
                    {
                        EmprendimientoId = e.EmprendimientoId,
                        NombreProductoServicio = e.NombreMarca,
                        TipoOferta = "Producto",
                        Categoria = "Destacado",
                        NombreEmprendimiento = e.NombreMarca,
                        EmprendedorId = e.EmprendedorId,
                        NombreEmprendedor = e.Emprendedor?.NombreCompleto ?? "Emprendedor",
                        CiudadId = e.CiudadId,
                        Ciudad = "Sin asignar",
                        Provincia = null,
                        Descripcion = e.Descripcion,
                        Precio = null,
                        MediosDePago = e.MediosDePago,
                        ZonaDeDespacho = e.ZonaDeDespacho,
                        TiempoDeEntrega = e.TiempoDeEntrega,
                        Certificaciones = e.Certificaciones,
                        SitioWeb = e.SitioWeb,
                        CorreoElectronico = e.Emprendedor?.CorreoElectronico ?? string.Empty,
                        Telefono = e.Emprendedor?.Telefono,
                        FechaRegistro = e.FechaRegistro
                    })
                    .ToList();
            }
            catch
            {
                Destacados = new List<VCatalogo>();
            }
        }
    }
}