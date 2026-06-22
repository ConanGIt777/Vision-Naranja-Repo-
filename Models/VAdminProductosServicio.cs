using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Keyless]
public partial class VAdminProductosServicio
{
    public int ProductoServicioId { get; set; }

    [StringLength(200)]
    public string NombreProductoServicio { get; set; } = null!;

    [StringLength(8)]
    public string? TipoOferta { get; set; }

    [StringLength(100)]
    public string Categoria { get; set; } = null!;

    public int EmprendimientoId { get; set; }

    [StringLength(150)]
    public string NombreEmprendimiento { get; set; } = null!;

    public bool EmprendimientoActivo { get; set; }

    public int EmprendedorId { get; set; }

    [StringLength(150)]
    public string NombreEmprendedor { get; set; } = null!;

    public bool EmprendedorActivo { get; set; }

    [StringLength(100)]
    public string Ciudad { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }

    public bool Disponible { get; set; }

    public DateTime FechaRegistro { get; set; }
}
