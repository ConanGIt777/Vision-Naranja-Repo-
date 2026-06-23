using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("EmprendimientoId", "EsPrincipal", Name = "IX_RS_Principal")]
[Index("EmprendimientoId", "Plataforma", Name = "UQ_RedesSociales_Plataforma", IsUnique = true)]
public partial class RedSocial
{
    [Key]
    public int RedSocialId { get; set; }

    public int EmprendimientoId { get; set; }

    [StringLength(50)]
    public string Plataforma { get; set; } = null!;

    [StringLength(150)]
    public string Usuario { get; set; } = null!;

    [StringLength(500)]
    public string? UrlPerfil { get; set; }

    public bool EsPrincipal { get; set; }

    [ForeignKey("EmprendimientoId")]
    [InverseProperty("RedesSociales")]
    public virtual Emprendimiento Emprendimiento { get; set; } = null!;
}
