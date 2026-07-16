using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Models;
using Microsoft.Data.SqlClient;
using VisionNaranja.Models.ViewModels;

namespace VisionNaranja.Services
{
    public class CatalogoService
    {
        private readonly VisionNaranjaContext _context;

        public CatalogoService(VisionNaranjaContext context)
        {
            _context = context;
        }
        // Convierte URL de Drive "ver" → URL directa de imagen
        public static string? ConvertirUrlDrive(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;

        var match = System.Text.RegularExpressions.Regex.Match(
            url, @"/file/d/([a-zA-Z0-9_-]+)");
        if (match.Success)
            return $"https://drive.google.com/thumbnail?id={match.Groups[1].Value}&sz=w1000";

        var match2 = System.Text.RegularExpressions.Regex.Match(
            url, @"[?&]id=([a-zA-Z0-9_-]+)");
        if (match2.Success)
            return $"https://drive.google.com/thumbnail?id={match2.Groups[1].Value}&sz=w1000";

        return url;
    }
        // Lista todo el catálogo público, con filtros opcionales
        public async Task<List<VCatalogo>> ObtenerCatalogoAsync(
    string? categoria = null,
    int? ciudadId = null,
    string? tipoOferta = null,
    string? termino = null)
{
    var query = _context.VCatalogos.AsQueryable();

    if (!string.IsNullOrWhiteSpace(categoria))
        query = query.Where(v => v.Categoria == categoria);

    if (ciudadId.HasValue)
        query = query.Where(v => v.CiudadId == ciudadId.Value);

    if (!string.IsNullOrWhiteSpace(tipoOferta))
        query = query.Where(v => v.TipoOferta == tipoOferta);

    if (!string.IsNullOrWhiteSpace(termino))
        query = query.Where(v =>
            v.NombreProductoServicio.Contains(termino) ||
            v.NombreEmprendimiento.Contains(termino) ||
            v.Categoria.Contains(termino) ||
            v.Ciudad.Contains(termino));

    var items = await query
        .OrderBy(v => v.NombreEmprendimiento)
        .ToListAsync();

    if (items.Count == 0)
    {
        var emprendimientos = await _context.Emprendimientos
            .Where(e => e.Activo)
            .Include(e => e.Emprendedor)
            .Include(e => e.Ciudad)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync();

        var fallbackItems = new List<VCatalogo>();

        foreach (var emprendimiento in emprendimientos)
        {
            var productos = await _context.ProductosServicios
                .Where(p => p.EmprendimientoId == emprendimiento.EmprendimientoId && p.Disponible)
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            if (productos.Count == 0)
            {
                fallbackItems.Add(new VCatalogo
                {
                    ProductoServicioId = 0,
                    NombreProductoServicio = emprendimiento.NombreMarca,
                    TipoOferta = "Producto",
                    Categoria = "General",
                    EmprendimientoId = emprendimiento.EmprendimientoId,
                    NombreEmprendimiento = emprendimiento.NombreMarca,
                    EmprendedorId = emprendimiento.EmprendedorId,
                    NombreEmprendedor = emprendimiento.Emprendedor?.NombreCompleto ?? string.Empty,
                    CiudadId = emprendimiento.CiudadId,
                    Ciudad = emprendimiento.Ciudad?.Nombre ?? string.Empty,
                    Provincia = emprendimiento.Ciudad?.Provincia,
                    Descripcion = emprendimiento.Descripcion,
                    CorreoElectronico = emprendimiento.Emprendedor?.CorreoElectronico ?? string.Empty,
                    Telefono = emprendimiento.Emprendedor?.Telefono,
                    FechaRegistro = emprendimiento.FechaRegistro,
                    EnlaceDriveFotos = emprendimiento.EnlaceDriveFotos,
                    ImagenPortadaUrl = ConvertirUrlDrive(emprendimiento.EnlaceDriveFotos),
                });
                continue;
            }

            foreach (var producto in productos)
            {
                fallbackItems.Add(new VCatalogo
                {
                    ProductoServicioId = producto.ProductoServicioId,
                    NombreProductoServicio = producto.Nombre,
                    TipoOferta = producto.TipoOferta.ToString(),
                    Categoria = producto.Categoria?.Nombre ?? "General",
                    EmprendimientoId = emprendimiento.EmprendimientoId,
                    NombreEmprendimiento = emprendimiento.NombreMarca,
                    EmprendedorId = emprendimiento.EmprendedorId,
                    NombreEmprendedor = emprendimiento.Emprendedor?.NombreCompleto ?? string.Empty,
                    CiudadId = emprendimiento.CiudadId,
                    Ciudad = emprendimiento.Ciudad?.Nombre ?? string.Empty,
                    Provincia = emprendimiento.Ciudad?.Provincia,
                    Descripcion = emprendimiento.Descripcion,
                    CorreoElectronico = emprendimiento.Emprendedor?.CorreoElectronico ?? string.Empty,
                    Telefono = emprendimiento.Emprendedor?.Telefono,
                    FechaRegistro = emprendimiento.FechaRegistro,
                    EnlaceDriveFotos = emprendimiento.EnlaceDriveFotos,
                    ImagenPortadaUrl = ConvertirUrlDrive(emprendimiento.EnlaceDriveFotos)
                });
            }
        }

        items = fallbackItems
            .Where(v => !string.IsNullOrWhiteSpace(v.NombreProductoServicio))
            .OrderBy(v => v.NombreEmprendimiento)
            .ThenBy(v => v.NombreProductoServicio)
            .ToList();
    }

    return items;
}

        // Listas para llenar los <select> de filtros (categorías y ciudades)
        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task<List<Ciudad>> ObtenerCiudadesAsync()
        {
            return await _context.Ciudades.OrderBy(c => c.Nombre).ToListAsync();
        }
        public async Task<EmprendimientoDetalleViewModel?> ObtenerDetalleEmprendimientoAsync(int emprendimientoId)
{
    EmprendimientoDetalleViewModel? detalle = null;

    var connection = _context.Database.GetDbConnection();
    await using var command = connection.CreateCommand();
    command.CommandText = "sp_ObtenerEmprendimiento";
    command.CommandType = System.Data.CommandType.StoredProcedure;
    command.Parameters.Add(new SqlParameter("@EmprendimientoId", emprendimientoId));

    var abrioConexion = false;
    if (connection.State != System.Data.ConnectionState.Open)
    {
        await connection.OpenAsync();
        abrioConexion = true;
    }

    try
    {
        await using var reader = await command.ExecuteReaderAsync();

        // Result set 1: datos del emprendimiento
        if (await reader.ReadAsync())
        {
            detalle = new EmprendimientoDetalleViewModel
            {
                EmprendimientoId = reader.GetInt32(reader.GetOrdinal("EmprendimientoId")),
                NombreMarca = reader.GetString(reader.GetOrdinal("NombreMarca")),
                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                SitioWeb = reader["SitioWeb"] as string,
                MediosDePago = reader["MediosDePago"] as string,
                ZonaDeDespacho = reader["ZonaDeDespacho"] as string,
                TiempoDeEntrega = reader["TiempoDeEntrega"] as string,
                Certificaciones = reader["Certificaciones"] as string,
                EnlaceDriveFotos = ConvertirUrlDrive(reader["EnlaceDriveFotos"] as string),
                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                EmprendedorId = reader.GetInt32(reader.GetOrdinal("EmprendedorId")),
                NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                CorreoElectronico = reader.GetString(reader.GetOrdinal("CorreoElectronico")),
                Telefono = reader["Telefono"] as string,
                CiudadId = reader.GetInt32(reader.GetOrdinal("CiudadId")),
                Ciudad = reader.GetString(reader.GetOrdinal("Ciudad")),
                Provincia = reader["Provincia"] as string,
                Pais = reader.GetString(reader.GetOrdinal("Pais"))
            };
        }

        if (detalle is null)
            return null; // No existe ese EmprendimientoId

        // Result set 2: redes sociales
        await reader.NextResultAsync();
        while (await reader.ReadAsync())
        {
            detalle.RedesSociales.Add(new RedSocialViewModel
            {
                RedSocialId = reader.GetInt32(reader.GetOrdinal("RedSocialId")),
                Plataforma = reader.GetString(reader.GetOrdinal("Plataforma")),
                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                UrlPerfil = reader["UrlPerfil"] as string,
                EsPrincipal = reader.GetBoolean(reader.GetOrdinal("EsPrincipal"))
            });
        }

        // Result set 3: productos y servicios
        await reader.NextResultAsync();
        while (await reader.ReadAsync())
        {
            detalle.Productos.Add(new ProductoServicioViewModel
            {
                ProductoServicioId = reader.GetInt32(reader.GetOrdinal("ProductoServicioId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                TipoOferta = reader.GetString(reader.GetOrdinal("TipoOferta")),
                Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
                Descripcion = reader["Descripcion"] as string,
                Precio = reader["Precio"] as decimal?,
                Disponible = reader.GetBoolean(reader.GetOrdinal("Disponible")),
                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
            });
        }
    }
    finally
    {
        if (abrioConexion)
            await connection.CloseAsync();
    }

    return detalle;
}
    }
    
}

