using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VisionNaranja.Models;

namespace VisionNaranja.Data;

public partial class VisionNaranjaContext : DbContext
{
    public VisionNaranjaContext()
    {
    }

    public VisionNaranjaContext(DbContextOptions<VisionNaranjaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Ciudade> Ciudades { get; set; }

    public virtual DbSet<Emprendedore> Emprendedores { get; set; }

    public virtual DbSet<Emprendimiento> Emprendimientos { get; set; }

    public virtual DbSet<MediaProducto> MediaProductos { get; set; }

    public virtual DbSet<ProductosServicio> ProductosServicios { get; set; }

    public virtual DbSet<RedesSociale> RedesSociales { get; set; }

    public virtual DbSet<VAdminProductosServicio> VAdminProductosServicios { get; set; }

    public virtual DbSet<VCatalogo> VCatalogos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-J22FVAB;Database=VisionNaranja;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AI");

        modelBuilder.Entity<Ciudade>(entity =>
        {
            entity.Property(e => e.Pais).HasDefaultValue("Ecuador", "DF_Ciudades_Pais");
        });

        modelBuilder.Entity<Emprendedore>(entity =>
        {
            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Emprendedores_Activo");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())", "DF_Emprendedores_Fecha");
        });

        modelBuilder.Entity<Emprendimiento>(entity =>
        {
            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Emprendimientos_Activo");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())", "DF_Emprendimientos_Fecha");

            entity.HasOne(d => d.Ciudad).WithMany(p => p.Emprendimientos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Emprendimientos_Ciudad");

            entity.HasOne(d => d.Emprendedor).WithMany(p => p.Emprendimientos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Emprendimientos_Emprendedor");
        });

        modelBuilder.Entity<MediaProducto>(entity =>
        {
            entity.Property(e => e.FechaSubida).HasDefaultValueSql("(sysdatetime())", "DF_Media_Fecha");
            entity.Property(e => e.Orden).HasDefaultValue((byte)1, "DF_Media_Orden");

            entity.HasOne(d => d.ProductoServicio).WithMany(p => p.MediaProductos).HasConstraintName("FK_Media_Producto");
        });

        modelBuilder.Entity<ProductosServicio>(entity =>
        {
            entity.Property(e => e.Disponible).HasDefaultValue(true, "DF_PS_Disponible");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())", "DF_PS_Fecha");

            entity.HasOne(d => d.Categoria).WithMany(p => p.ProductosServicios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PS_Categoria");

            entity.HasOne(d => d.Emprendimiento).WithMany(p => p.ProductosServicios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PS_Emprendimiento");
        });

        modelBuilder.Entity<RedesSociale>(entity =>
        {
            entity.HasOne(d => d.Emprendimiento).WithMany(p => p.RedesSociales).HasConstraintName("FK_RedesSociales_Emprendimiento");
        });

        modelBuilder.Entity<VAdminProductosServicio>(entity =>
        {
            entity.ToView("vAdminProductosServicios");
        });

        modelBuilder.Entity<VCatalogo>(entity =>
        {
            entity.ToView("vCatalogo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
