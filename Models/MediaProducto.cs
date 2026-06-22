using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Table("MediaProducto")]
public partial class MediaProducto
{
    [Key]
    public int MediaId { get; set; }

    public int ProductoServicioId { get; set; }

    [StringLength(10)]
    public string TipoMedia { get; set; } = null!;

    public byte[]? Datos { get; set; }

    [StringLength(50)]
    public string? MimeType { get; set; }

    [StringLength(500)]
    public string? UrlExterna { get; set; }

    public bool EsPortada { get; set; }

    public byte Orden { get; set; }

    public DateTime FechaSubida { get; set; }

    [ForeignKey("ProductoServicioId")]
    [InverseProperty("MediaProductos")]
    public virtual ProductosServicio ProductoServicio { get; set; } = null!;
}
