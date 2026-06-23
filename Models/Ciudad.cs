using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionNaranja.Models;

[Index("Nombre", "Provincia", Name = "UQ_Ciudades_Nombre", IsUnique = true)]
public partial class Ciudad
{
    [Key]
    public int CiudadId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string? Provincia { get; set; }

    [StringLength(100)]
    public string Pais { get; set; } = null!;

    [InverseProperty("Ciudad")]
    public virtual ICollection<Emprendimiento> Emprendimientos { get; set; } = new List<Emprendimiento>();
}
