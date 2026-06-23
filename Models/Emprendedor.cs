using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("CorreoElectronico", Name = "UQ_Emprendedores_Correo", IsUnique = true)]
public partial class Emprendedor
{
    [Key]
    public int EmprendedorId { get; set; }

    [StringLength(150)]
    public string NombreCompleto { get; set; } = null!;

    [StringLength(150)]
    public string CorreoElectronico { get; set; } = null!;

    [StringLength(20)]
    public string? Telefono { get; set; }

    public byte[]? FotoPerfil { get; set; }

    [StringLength(50)]
    public string? FotoPerfilMime { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    [InverseProperty("Emprendedor")]
    public virtual ICollection<Emprendimiento> Emprendimientos { get; set; } = new List<Emprendimiento>();
}
