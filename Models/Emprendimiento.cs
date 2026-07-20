using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("CiudadId", Name = "IX_Emp_Ciudad")]
public partial class Emprendimiento
{
    [Key]
    public int EmprendimientoId { get; set; }

    public int EmprendedorId { get; set; }

    public int CiudadId { get; set; }

    [StringLength(150)]
    public string NombreMarca { get; set; } = null!;

    [StringLength(500)]
    public string Descripcion { get; set; } = null!;

    [StringLength(255)]
    public string? SitioWeb { get; set; }

    [StringLength(255)]
    public string? MediosDePago { get; set; }

    [StringLength(255)]
    public string? ZonaDeDespacho { get; set; }

    [StringLength(100)]
    public string? TiempoDeEntrega { get; set; }

    [StringLength(255)]
    public string? Certificaciones { get; set; }

    [StringLength(500)]
    public string? EnlaceDriveFotos { get; set; }

    public byte[]? Logo { get; set; }

    [StringLength(50)]
    public string? LogoMime { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    [ForeignKey("CiudadId")]
    [InverseProperty("Emprendimientos")]
    public virtual Ciudad Ciudad { get; set; } = null!;

    [ForeignKey("EmprendedorId")]
    [InverseProperty("Emprendimientos")]
    public virtual Emprendedor Emprendedor { get; set; } = null!;

    [InverseProperty("Emprendimiento")]
    public virtual ICollection<ProductosServicio> ProductosServicios { get; set; } = new List<ProductosServicio>();

    [InverseProperty("Emprendimiento")]
    public virtual ICollection<RedSocial> RedesSociales { get; set; } = new List<RedSocial>();
}
