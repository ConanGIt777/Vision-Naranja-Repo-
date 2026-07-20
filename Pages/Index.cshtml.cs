using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Models;
using VisionNaranja.Services;

namespace VisionNaranja.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CatalogoService _catalogoService;

        public IndexModel(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public List<VCatalogo> Destacados { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var catalogo = await _catalogoService.ObtenerCatalogoAsync();

                Destacados = catalogo
                    .GroupBy(v => v.EmprendimientoId)
                    .Select(g => g.First())
                    .OrderByDescending(v => v.FechaRegistro)
                    .ToList();
            }
            catch
            {
                Destacados = new List<VCatalogo>();
            }
        }
    }
}