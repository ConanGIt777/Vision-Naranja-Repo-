using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;
using VisionNaranja.Services;

namespace VisionNaranja.Pages.Catalogo
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

        public List<VCatalogo> Items { get; set; } = new();
        public List<VCatalogo> Destacados { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public List<Ciudad> Ciudades { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Categoria { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CiudadId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TipoOferta { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Termino { get; set; }

        public async Task OnGetAsync()
        {
            var logosPorEmprendimiento = await _context.Emprendimientos
                .Where(e => e.Logo != null)
                .Select(e => e.EmprendimientoId)
                .ToListAsync();

            Categorias = await _catalogoService.ObtenerCategoriasAsync();
            Ciudades = await _catalogoService.ObtenerCiudadesAsync();

            Items = await _catalogoService.ObtenerCatalogoAsync(
                Categoria, CiudadId, TipoOferta, Termino);

            Destacados = Items
                .GroupBy(v => v.EmprendimientoId)
                .Select(g => g.First())
                .OrderByDescending(v => v.FechaRegistro)
                .Take(4)
                .ToList();

            foreach (var item in Destacados)
            {
                if (logosPorEmprendimiento.Contains(item.EmprendimientoId))
                    item.ImagenPortadaUrl = $"/Media/Emprendimiento/{item.EmprendimientoId}";
            }
        }
    }
}