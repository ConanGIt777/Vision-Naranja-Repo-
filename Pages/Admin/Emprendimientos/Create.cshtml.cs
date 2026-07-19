using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendimientos
{
    public class CreateModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public CreateModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public List<Emprendedor> Emprendedores { get; set; } = new();
        public List<Ciudad> Ciudades { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            Emprendedores = await _context.Emprendedores.Where(e => e.Activo).OrderBy(e => e.NombreCompleto).ToListAsync();
            Ciudades = await _context.Ciudades.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(
            int emprendedorId, int ciudadId, string nombreMarca, string descripcion,
            string? sitioWeb, string? mediosDePago, string? zonaDeDespacho,
            string? tiempoDeEntrega, string? certificaciones, bool activo, IFormFile? logo)
        {
            await OnGetAsync(); // recargar dropdowns por si hay que re-mostrar el form

            if (emprendedorId == 0 || ciudadId == 0 || string.IsNullOrWhiteSpace(nombreMarca) || string.IsNullOrWhiteSpace(descripcion))
            {
                Error = "Emprendedor, Ciudad, Nombre de marca y Descripción son obligatorios.";
                return Page();
            }

            var emprendimiento = new Emprendimiento
            {
                EmprendedorId = emprendedorId,
                CiudadId = ciudadId,
                NombreMarca = nombreMarca.Trim(),
                Descripcion = descripcion.Trim(),
                SitioWeb = string.IsNullOrWhiteSpace(sitioWeb) ? null : sitioWeb.Trim(),
                MediosDePago = string.IsNullOrWhiteSpace(mediosDePago) ? null : mediosDePago.Trim(),
                ZonaDeDespacho = string.IsNullOrWhiteSpace(zonaDeDespacho) ? null : zonaDeDespacho.Trim(),
                TiempoDeEntrega = string.IsNullOrWhiteSpace(tiempoDeEntrega) ? null : tiempoDeEntrega.Trim(),
                Certificaciones = string.IsNullOrWhiteSpace(certificaciones) ? null : certificaciones.Trim(),
                FechaRegistro = DateTime.Now,
                Activo = activo
            };

            if (logo is { Length: > 0 })
            {
                using var ms = new MemoryStream();
                await logo.CopyToAsync(ms);
                emprendimiento.Logo = ms.ToArray();
                emprendimiento.LogoMime = logo.ContentType;
            }

            _context.Emprendimientos.Add(emprendimiento);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{emprendimiento.NombreMarca}' creado correctamente.";
            return RedirectToPage("Edit", new { id = emprendimiento.EmprendimientoId });
        }
    }
}