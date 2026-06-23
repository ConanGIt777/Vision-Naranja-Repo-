namespace VisionNaranja.Models.ViewModels
{
    public class EmprendimientoDetalleViewModel
    {
        public int EmprendimientoId { get; set; }
        public string NombreMarca { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? SitioWeb { get; set; }
        public string? MediosDePago { get; set; }
        public string? ZonaDeDespacho { get; set; }
        public string? TiempoDeEntrega { get; set; }
        public string? Certificaciones { get; set; }
        public string? EnlaceDriveFotos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        public int EmprendedorId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string? Telefono { get; set; }

        public int CiudadId { get; set; }
        public string Ciudad { get; set; } = null!;
        public string? Provincia { get; set; }
        public string Pais { get; set; } = null!;

        public List<RedSocialViewModel> RedesSociales { get; set; } = new();
        public List<ProductoServicioViewModel> Productos { get; set; } = new();
    }

    public class RedSocialViewModel
    {
        public int RedSocialId { get; set; }
        public string Plataforma { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public string? UrlPerfil { get; set; }
        public bool EsPrincipal { get; set; }
    }

    public class ProductoServicioViewModel
    {
        public int ProductoServicioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string TipoOferta { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public bool Disponible { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}