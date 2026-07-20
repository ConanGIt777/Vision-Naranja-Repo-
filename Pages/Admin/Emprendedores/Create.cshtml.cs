using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendedores
{
    public class CreateModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public CreateModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public string? Error { get; set; }

        public async Task<IActionResult> OnPostAsync(
            string nombreCompleto, string correoElectronico, string? telefono,
            IFormFile? foto, bool activo)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto) || string.IsNullOrWhiteSpace(correoElectronico))
            {
                Error = "Nombre y correo son obligatorios.";
                return Page();
            }

            var yaExiste = await _context.Emprendedores.AnyAsync(e => e.CorreoElectronico == correoElectronico.Trim());
            if (yaExiste)
            {
                Error = "Ya existe un emprendedor con ese correo.";
                return Page();
            }

            var emprendedor = new Emprendedor
            {
                NombreCompleto = nombreCompleto.Trim(),
                CorreoElectronico = correoElectronico.Trim(),
                Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                FechaRegistro = DateTime.Now,
                Activo = activo
            };

            if (foto is { Length: > 0 })
            {
                using var ms = new MemoryStream();
                await foto.CopyToAsync(ms);
                emprendedor.FotoPerfil = ms.ToArray();
                emprendedor.FotoPerfilMime = foto.ContentType;
            }

            _context.Emprendedores.Add(emprendedor);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{emprendedor.NombreCompleto}' creado correctamente.";
            return RedirectToPage("Index");
        }
    }
}