using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DomiciliosApp.Models;

public partial class DomiciliosContext : DbContext
{
    public DomiciliosContext(DbContextOptions<DomiciliosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<cliente> clientes { get; set; }

    public virtual DbSet<cliente_direccion> cliente_direccions { get; set; }

    public virtual DbSet<direccion> direccions { get; set; }

    public virtual DbSet<domiciliario> domiciliarios { get; set; }

    public virtual DbSet<establecimiento> establecimientos { get; set; }

    public virtual DbSet<estadodomiciliario> estadodomiciliarios { get; set; }

    public virtual DbSet<estadosolicitud> estadosolicituds { get; set; }

    public virtual DbSet<incidente> incidentes { get; set; }

    public virtual DbSet<mediodetransporte> mediodetransportes { get; set; }

    public virtual DbSet<metodopago> metodopagos { get; set; }

    public virtual DbSet<solicitud> solicituds { get; set; }

    public virtual DbSet<tipoestablecimiento> tipoestablecimientos { get; set; }

    public virtual DbSet<tipoincidente> tipoincidentes { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

    public virtual DbSet<log_estado_solicitud> log_estado_solicituds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<cliente>(entity =>
        {
            entity.HasKey(e => e.id_cliente).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.Property(e => e.id_cliente).ValueGeneratedNever();

            entity.HasOne(d => d.id_clienteNavigation).WithOne(p => p.cliente)
                .HasForeignKey<cliente>(d => d.id_cliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_ibfk_1");
        });

        modelBuilder.Entity<cliente_direccion>(entity =>
        {
            entity.HasKey(e => new { e.id_cliente, e.id_direccion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("cliente_direccion");

            entity.HasIndex(e => e.id_direccion, "id_direccion");

            entity.HasOne(d => d.id_clienteNavigation).WithMany(p => p.cliente_direccions)
                .HasForeignKey(d => d.id_cliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_direccion_ibfk_1");

            entity.HasOne(d => d.id_direccionNavigation).WithMany(p => p.cliente_direccions)
                .HasForeignKey(d => d.id_direccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_direccion_ibfk_2");
        });

        modelBuilder.Entity<direccion>(entity =>
        {
            entity.HasKey(e => e.id_direccion).HasName("PRIMARY");

            entity.ToTable("direccion");

            entity.Property(e => e.barrio).HasMaxLength(255);
            entity.Property(e => e.comuna).HasMaxLength(255);
            entity.Property(e => e.referencia).HasColumnType("text");
            entity.Property(e => e.ubicacion).HasMaxLength(500);
        });

        modelBuilder.Entity<domiciliario>(entity =>
        {
            entity.HasKey(e => e.id_domiciliario).HasName("PRIMARY");

            entity.ToTable("domiciliario");

            entity.HasIndex(e => e.codigo, "codigo").IsUnique();

            entity.HasIndex(e => e.id_estado, "id_estado");

            entity.HasIndex(e => e.id_transporte, "id_transporte");

            entity.Property(e => e.id_domiciliario).ValueGeneratedNever();

            entity.HasOne(d => d.id_domiciliarioNavigation).WithOne(p => p.domiciliario)
                .HasForeignKey<domiciliario>(d => d.id_domiciliario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("domiciliario_ibfk_1");

            entity.HasOne(d => d.id_estadoNavigation).WithMany(p => p.domiciliarios)
                .HasForeignKey(d => d.id_estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("domiciliario_ibfk_3");

            entity.HasOne(d => d.id_transporteNavigation).WithMany(p => p.domiciliarios)
                .HasForeignKey(d => d.id_transporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("domiciliario_ibfk_2");
        });

        modelBuilder.Entity<establecimiento>(entity =>
        {
            entity.HasKey(e => e.id_establecimiento).HasName("PRIMARY");

            entity.ToTable("establecimiento");

            entity.HasIndex(e => e.codigo, "codigo").IsUnique();

            entity.HasIndex(e => e.id_direccion, "id_direccion");

            entity.HasIndex(e => e.id_tipoEstablecimiento, "id_tipoEstablecimiento");

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.telefono).HasMaxLength(100);

            entity.HasOne(d => d.id_direccionNavigation).WithMany(p => p.establecimientos)
                .HasForeignKey(d => d.id_direccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("establecimiento_ibfk_2");

            entity.HasOne(d => d.id_tipoEstablecimientoNavigation).WithMany(p => p.establecimientos)
                .HasForeignKey(d => d.id_tipoEstablecimiento)
                .HasConstraintName("establecimiento_ibfk_1");
        });

        modelBuilder.Entity<estadodomiciliario>(entity =>
        {
            entity.HasKey(e => e.id_estado).HasName("PRIMARY");

            entity.ToTable("estadodomiciliario");

            entity.Property(e => e.nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<estadosolicitud>(entity =>
        {
            entity.HasKey(e => e.id_estado).HasName("PRIMARY");

            entity.ToTable("estadosolicitud");

            entity.Property(e => e.nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<incidente>(entity =>
        {
            entity.HasKey(e => e.id_incidente).HasName("PRIMARY");

            entity.ToTable("incidente");

            entity.HasIndex(e => e.id_solicitud, "id_solicitud");

            entity.HasIndex(e => e.tipoIncidente, "tipoIncidente");

            entity.Property(e => e.descripcion).HasMaxLength(100);
            entity.Property(e => e.hora).HasColumnType("time");

            entity.HasOne(d => d.id_solicitudNavigation).WithMany(p => p.incidentes)
                .HasForeignKey(d => d.id_solicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("incidente_ibfk_2");

            entity.HasOne(d => d.tipoIncidenteNavigation).WithMany(p => p.incidentes)
                .HasForeignKey(d => d.tipoIncidente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("incidente_ibfk_1");
        });

        modelBuilder.Entity<mediodetransporte>(entity =>
        {
            entity.HasKey(e => e.id_transporte).HasName("PRIMARY");

            entity.ToTable("mediodetransporte");

            entity.Property(e => e.nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<metodopago>(entity =>
        {
            entity.HasKey(e => e.id_metodoPago).HasName("PRIMARY");

            entity.ToTable("metodopago");

            entity.Property(e => e.nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<solicitud>(entity =>
        {
            entity.HasKey(e => e.id_solicitud).HasName("PRIMARY");

            entity.ToTable("solicitud");

            entity.HasIndex(e => new { e.id_cliente, e.id_direccionEntrega }, "id_cliente");

            entity.HasIndex(e => e.id_direccionRecogidaExterna, "id_direccionRecogidaExterna");

            entity.HasIndex(e => e.id_domiciliario, "id_domiciliario");

            entity.HasIndex(e => e.id_establecimientoRecogida, "id_establecimientoRecogida");

            entity.HasIndex(e => e.id_estado, "id_estado");

            entity.HasIndex(e => e.id_metodoPago, "id_metodoPago");

            entity.Property(e => e.descripcion).HasColumnType("text");
            entity.Property(e => e.horaAsignacion).HasColumnType("time");
            entity.Property(e => e.horaCreacion).HasColumnType("time");
            entity.Property(e => e.montoTotal).HasPrecision(10, 2);

            entity.HasOne(d => d.id_clienteNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_cliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_ibfk_3");

            entity.HasOne(d => d.id_direccionRecogidaExternaNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_direccionRecogidaExterna)
                .HasConstraintName("solicitud_ibfk_7");

            entity.HasOne(d => d.id_domiciliarioNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_domiciliario)
                .HasConstraintName("solicitud_ibfk_4");

            entity.HasOne(d => d.id_establecimientoRecogidaNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_establecimientoRecogida)
                .HasConstraintName("solicitud_ibfk_6");

            entity.HasOne(d => d.id_estadoNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_ibfk_1");

            entity.HasOne(d => d.id_metodoPagoNavigation).WithMany(p => p.solicituds)
                .HasForeignKey(d => d.id_metodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_ibfk_2");

            entity.HasOne(d => d.cliente_direccion).WithMany(p => p.solicituds)
                .HasForeignKey(d => new { d.id_cliente, d.id_direccionEntrega })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_ibfk_5");
        });

        modelBuilder.Entity<tipoestablecimiento>(entity =>
        {
            entity.HasKey(e => e.id_tipoEstablecimiento).HasName("PRIMARY");

            entity.ToTable("tipoestablecimiento");

            entity.Property(e => e.nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<tipoincidente>(entity =>
        {
            entity.HasKey(e => e.id_tipoIncidente).HasName("PRIMARY");

            entity.ToTable("tipoincidente");

            entity.Property(e => e.nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.numeroId, "numeroId").IsUnique();

            entity.Property(e => e.correo).HasMaxLength(255);
            entity.Property(e => e.nombre_completo).HasMaxLength(255);
            entity.Property(e => e.numeroId).HasMaxLength(20);
            entity.Property(e => e.telefono).HasMaxLength(10);
        });

        modelBuilder.Entity<log_estado_solicitud>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("log_estado_solicitud");

            entity.Property(e => e.id_solicitud).HasColumnName("id_solicitud");
            entity.Property(e => e.estado_anterior).HasColumnName("estado_anterior");
            entity.Property(e => e.estado_nuevo).HasColumnName("estado_nuevo");
            entity.Property(e => e.fecha_cambio).HasColumnName("fecha_cambio");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}