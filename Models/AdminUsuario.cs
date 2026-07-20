using System.ComponentModel.DataAnnotations;

namespace VisionNaranja.Models;

public class AdminUsuario
{
    [Key]
    public int AdminUsuarioId { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = null!;

    [Required, StringLength(500)]
    public string PasswordHash { get; set; } = null!;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}