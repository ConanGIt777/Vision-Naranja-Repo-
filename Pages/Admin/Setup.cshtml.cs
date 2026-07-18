using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin
{
    public class SetupModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public SetupModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public bool YaExisteAdmin { get; set; }
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            YaExisteAdmin = await _context.AdminUsuarios.AnyAsync();
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            if (await _context.AdminUsuarios.AnyAsync())
            {
                YaExisteAdmin = true;
                return Page();
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                Error = "Usuario requerido y contraseña de al menos 8 caracteres.";
                return Page();
            }

            var hasher = new PasswordHasher<AdminUsuario>();
            var admin = new AdminUsuario { Username = username.Trim() };
            admin.PasswordHash = hasher.HashPassword(admin, password);

            _context.AdminUsuarios.Add(admin);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Login");
        }
    }
}