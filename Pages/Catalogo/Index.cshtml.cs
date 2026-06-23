using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Data;
using VisionNaranja.Models;
using VisionNaranja.Services;

namespace VisionNaranja.Pages.Catalogo
{
    public class IndexModel : PageModel
    {
        private readonly CatalogoService _catalogoService;

        public IndexModel(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public List<VCatalogo> Items { get; set; } = new();
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
            Categorias = await _catalogoService.ObtenerCategoriasAsync();
            Ciudades = await _catalogoService.ObtenerCiudadesAsync();

            Items = await _catalogoService.ObtenerCatalogoAsync(
                Categoria, CiudadId, TipoOferta, Termino);
        }
    }
}