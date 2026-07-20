using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendimientos
{
    public class EditModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public EditModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public Emprendimiento? Emprendimiento { get; set; }
        public List<Emprendedor> Emprendedores { get; set; } = new();
        public List<Ciudad> Ciudades { get; set; } = new();
        public string? Error { get; set; }

        private async Task CargarListasAsync()
        {
            Emprendedores = await _context.Emprendedores.Where(e => e.Activo).OrderBy(e => e.NombreCompleto).ToListAsync();
            Ciudades = await _context.Ciudades.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task OnGetAsync(int id)
        {
            Emprendimiento = await _context.Emprendimientos
                .Include(e => e.RedesSociales)
                .FirstOrDefaultAsync(e => e.EmprendimientoId == id);
            await CargarListasAsync();
        }

        public async Task<IActionResult> OnPostGuardarAsync(
            int id, int emprendedorId, int ciudadId, string nombreMarca, string descripcion,
            string? sitioWeb, string? mediosDePago, string? zonaDeDespacho,
            string? tiempoDeEntrega, string? certificaciones, bool activo, IFormFile? logo)
        {
            Emprendimiento = await _context.Emprendimientos
                .Include(e => e.RedesSociales)
                .FirstOrDefaultAsync(e => e.EmprendimientoId == id);

            await CargarListasAsync();

            if (Emprendimiento is null) return Page();

            if (emprendedorId == 0 || ciudadId == 0 || string.IsNullOrWhiteSpace(nombreMarca) || string.IsNullOrWhiteSpace(descripcion))
            {
                Error = "Emprendedor, Ciudad, Nombre de marca y Descripción son obligatorios.";
                return Page();
            }

            Emprendimiento.EmprendedorId = emprendedorId;
            Emprendimiento.CiudadId = ciudadId;
            Emprendimiento.NombreMarca = nombreMarca.Trim();
            Emprendimiento.Descripcion = descripcion.Trim();
            Emprendimiento.SitioWeb = string.IsNullOrWhiteSpace(sitioWeb) ? null : sitioWeb.Trim();
            Emprendimiento.MediosDePago = string.IsNullOrWhiteSpace(mediosDePago) ? null : mediosDePago.Trim();
            Emprendimiento.ZonaDeDespacho = string.IsNullOrWhiteSpace(zonaDeDespacho) ? null : zonaDeDespacho.Trim();
            Emprendimiento.TiempoDeEntrega = string.IsNullOrWhiteSpace(tiempoDeEntrega) ? null : tiempoDeEntrega.Trim();
            Emprendimiento.Certificaciones = string.IsNullOrWhiteSpace(certificaciones) ? null : certificaciones.Trim();
            Emprendimiento.Activo = activo;

            if (logo is { Length: > 0 })
            {
                using var ms = new MemoryStream();
                await logo.CopyToAsync(ms);
                Emprendimiento.Logo = ms.ToArray();
                Emprendimiento.LogoMime = logo.ContentType;
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Cambios guardados correctamente.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostAgregarRedAsync(int id, string plataforma, string usuario, string? urlPerfil)
        {
            if (!string.IsNullOrWhiteSpace(plataforma) && !string.IsNullOrWhiteSpace(usuario))
            {
                var yaHayPrincipal = await _context.RedesSociales.AnyAsync(r => r.EmprendimientoId == id);

                _context.RedesSociales.Add(new RedSocial
                {
                    EmprendimientoId = id,
                    Plataforma = plataforma.Trim(),
                    Usuario = usuario.Trim(),
                    UrlPerfil = string.IsNullOrWhiteSpace(urlPerfil) ? null : urlPerfil.Trim(),
                    EsPrincipal = !yaHayPrincipal
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostEliminarRedAsync(int id, int redSocialId)
        {
            var red = await _context.RedesSociales.FindAsync(redSocialId);
            if (red != null)
            {
                _context.RedesSociales.Remove(red);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }
    }
}