using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("CategoriaId", "TipoOferta", Name = "IX_PS_Categoria")]
[Index("EmprendimientoId", Name = "IX_PS_Emprendimiento")]
[Index("TipoOferta", "Disponible", Name = "IX_PS_TipoOferta")]
public partial class ProductosServicio
{
    [Key]
    public int ProductoServicioId { get; set; }

    public int EmprendimientoId { get; set; }

    public int CategoriaId { get; set; }

    [StringLength(200)]
    public string Nombre { get; set; } = null!;

    public byte TipoOferta { get; set; }

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }

    public bool Disponible { get; set; }

    public DateTime FechaRegistro { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("ProductosServicios")]
    public virtual Categoria Categoria { get; set; } = null!;

    [ForeignKey("EmprendimientoId")]
    [InverseProperty("ProductosServicios")]
    public virtual Emprendimiento Emprendimiento { get; set; } = null!;

    [InverseProperty("ProductoServicio")]
    public virtual ICollection<MediaProducto> MediaProductos { get; set; } = new List<MediaProducto>();
}
