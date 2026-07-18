using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin.Emprendedores
{
    public class EditModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public EditModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public Emprendedor? Emprendedor { get; set; }
        public string? Error { get; set; }

        public async Task OnGetAsync(int id)
        {
            Emprendedor = await _context.Emprendedores.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(
            int id, string nombreCompleto, string correoElectronico, string? telefono,
            IFormFile? foto, bool activo)
        {
            Emprendedor = await _context.Emprendedores.FindAsync(id);
            if (Emprendedor is null) return Page();

            if (string.IsNullOrWhiteSpace(nombreCompleto) || string.IsNullOrWhiteSpace(correoElectronico))
            {
                Error = "Nombre y correo son obligatorios.";
                return Page();
            }

            var correoDuplicado = await _context.Emprendedores
                .AnyAsync(e => e.CorreoElectronico == correoElectronico.Trim() && e.EmprendedorId != id);
            if (correoDuplicado)
            {
                Error = "Ya existe otro emprendedor con ese correo.";
                return Page();
            }

            Emprendedor.NombreCompleto = nombreCompleto.Trim();
            Emprendedor.CorreoElectronico = correoElectronico.Trim();
            Emprendedor.Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim();
            Emprendedor.Activo = activo;

            if (foto is { Length: > 0 })
            {
                using var ms = new MemoryStream();
                await foto.CopyToAsync(ms);
                Emprendedor.FotoPerfil = ms.ToArray();
                Emprendedor.FotoPerfilMime = foto.ContentType;
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"'{Emprendedor.NombreCompleto}' actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}