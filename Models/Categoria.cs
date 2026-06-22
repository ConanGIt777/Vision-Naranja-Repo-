using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("Nombre", Name = "UQ_Categorias_Nombre", IsUnique = true)]
public partial class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [InverseProperty("Categoria")]
    public virtual ICollection<ProductosServicio> ProductosServicios { get; set; } = new List<ProductosServicio>();
}
