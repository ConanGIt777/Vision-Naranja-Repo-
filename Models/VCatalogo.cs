using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Keyless]
public partial class VCatalogo
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

    public int EmprendedorId { get; set; }

    [StringLength(150)]
    public string NombreEmprendedor { get; set; } = null!;

    public int CiudadId { get; set; }

    [StringLength(100)]
    public string Ciudad { get; set; } = null!;

    [StringLength(100)]
    public string? Provincia { get; set; }

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }

    [StringLength(255)]
    public string? MediosDePago { get; set; }

    [StringLength(255)]
    public string? ZonaDeDespacho { get; set; }

    [StringLength(100)]
    public string? TiempoDeEntrega { get; set; }

    [StringLength(255)]
    public string? Certificaciones { get; set; }

    [StringLength(255)]
    public string? SitioWeb { get; set; }

    [StringLength(150)]
    public string CorreoElectronico { get; set; } = null!;

    [StringLength(20)]
    public string? Telefono { get; set; }

    public DateTime FechaRegistro { get; set; }
}
