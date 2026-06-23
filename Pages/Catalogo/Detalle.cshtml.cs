using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionNaranja.Models.ViewModels;
using VisionNaranja.Services;
using Microsoft.AspNetCore.Mvc;

namespace VisionNaranja.Pages.Catalogo
{
    public class DetalleModel : PageModel
    {
        private readonly CatalogoService _catalogoService;

        public DetalleModel(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public EmprendimientoDetalleViewModel? Emprendimiento { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Emprendimiento = await _catalogoService.ObtenerDetalleEmprendimientoAsync(id);

            if (Emprendimiento is null)
                return NotFound();

            return Page();
        }
    }
}