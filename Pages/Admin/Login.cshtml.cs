using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;

namespace VisionNaranja.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly VisionNaranjaContext _context;

        public LoginModel(VisionNaranjaContext context)
        {
            _context = context;
        }

        public string? Error { get; set; }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var admin = await _context.AdminUsuarios
                .FirstOrDefaultAsync(a => a.Username == username.Trim());

            if (admin is null)
            {
                Error = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var hasher = new PasswordHasher<AdminUsuario>();
            var resultado = hasher.VerifyHashedPassword(admin, admin.PasswordHash, password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                Error = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToPage("/Admin/Emprendedores/Index");
        }
    }
}