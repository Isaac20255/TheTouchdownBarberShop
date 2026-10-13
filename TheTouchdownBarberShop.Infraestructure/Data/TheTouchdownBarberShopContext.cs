using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Data;

public partial class TheTouchdownBarberShopContext : DbContext
{
    public TheTouchdownBarberShopContext(DbContextOptions<TheTouchdownBarberShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriasProductos> CategoriasProductos { get; set; }

    public virtual DbSet<CategoriasServicios> CategoriasServicios { get; set; }

    public virtual DbSet<CitaPreguntas> CitaPreguntas { get; set; }

    public virtual DbSet<CitaServicios> CitaServicios { get; set; }

    public virtual DbSet<Citas> Citas { get; set; }

    public virtual DbSet<FacturaProductos> FacturaProductos { get; set; }

    public virtual DbSet<FacturaServicios> FacturaServicios { get; set; }

    public virtual DbSet<Facturas> Facturas { get; set; }

    public virtual DbSet<HistorialCitas> HistorialCitas { get; set; }

    public virtual DbSet<HorariosBarberos> HorariosBarberos { get; set; }

    public virtual DbSet<MetodosPago> MetodosPago { get; set; }

    public virtual DbSet<PaqueteServicios> PaqueteServicios { get; set; }

    public virtual DbSet<Paquetes> Paquetes { get; set; }

    public virtual DbSet<Productos> Productos { get; set; }

    public virtual DbSet<Resenas> Resenas { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Servicios> Servicios { get; set; }

    public virtual DbSet<Sucursales> Sucursales { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriasProductos>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto).HasName("PK__categori__87643501489E1653");

            entity.ToTable("categorias_productos");

            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC688E87C18").IsUnique();

            entity.Property(e => e.IdCategoriaProducto).HasColumnName("id_categoria_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<CategoriasServicios>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CD54BC5ACD28DA35");

            entity.ToTable("categorias_servicios");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Obligatoria).HasColumnName("obligatoria");
        });

        modelBuilder.Entity<CitaPreguntas>(entity =>
        {
            entity.HasKey(e => e.IdCitaPregunta).HasName("PK__cita_pre__99A8E5B5C4BA9D75");

            entity.ToTable("cita_preguntas");

            entity.Property(e => e.IdCitaPregunta).HasColumnName("id_cita_pregunta");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.Pregunta)
                .HasMaxLength(250)
                .HasColumnName("pregunta");
            entity.Property(e => e.Respuesta)
                .HasMaxLength(500)
                .HasColumnName("respuesta");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.CitaPreguntas)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("FK_cita_preguntas_citas");
        });

        modelBuilder.Entity<CitaServicios>(entity =>
        {
            entity.HasKey(e => e.IdCitaServicio).HasName("PK__cita_ser__9F00D22962A3994F");

            entity.ToTable("cita_servicios");

            entity.HasIndex(e => new { e.IdCita, e.IdServicio }, "UQ_cita_servicios").IsUnique();

            entity.Property(e => e.IdCitaServicio).HasColumnName("id_cita_servicio");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.PrecioAplicado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_aplicado");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.CitaServicios)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("FK_cita_servicios_citas");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.CitaServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cita_servicios_servicios");
        });

        modelBuilder.Entity<Citas>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__citas__6AEC3C0912C2ACB1");

            entity.ToTable("citas");

            entity.HasIndex(e => new { e.IdBarbero, e.FechaHora }, "UX_citas_barbero_fecha")
                .IsUnique()
                .HasFilter("([id_barbero] IS NOT NULL AND [estado]<>'Cancelada')");

            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaHora).HasColumnName("fecha_hora");
            entity.Property(e => e.IdBarbero).HasColumnName("id_barbero");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdBarberoNavigation).WithMany(p => p.CitasIdBarberoNavigation)
                .HasForeignKey(d => d.IdBarbero)
                .HasConstraintName("FK_citas_barbero");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.CitasIdClienteNavigation)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_citas_cliente");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.Citas)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK_citas_paquete");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Citas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_citas_sucursales");
        });

        modelBuilder.Entity<FacturaProductos>(entity =>
        {
            entity.HasKey(e => e.IdFacturaProducto).HasName("PK__factura___2019D081DAA6F37D");

            entity.ToTable("factura_productos");

            entity.Property(e => e.IdFacturaProducto).HasColumnName("id_factura_producto");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.FacturaProductos)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK_factura_productos_facturas");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.FacturaProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_factura_productos_productos");
        });

        modelBuilder.Entity<FacturaServicios>(entity =>
        {
            entity.HasKey(e => e.IdFacturaServicio).HasName("PK__factura___2E73B1F4B5055334");

            entity.ToTable("factura_servicios");

            entity.Property(e => e.IdFacturaServicio).HasColumnName("id_factura_servicio");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.FacturaServicios)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK_factura_servicios_facturas");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.FacturaServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_factura_servicios_servicios");
        });

        modelBuilder.Entity<Facturas>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__facturas__6C08ED538502C287");

            entity.ToTable("facturas");

            entity.HasIndex(e => e.IdCita, "UX_facturas_cita")
                .IsUnique()
                .HasFilter("([id_cita] IS NOT NULL)");

            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_emision");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Impuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuesto");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdCitaNavigation).WithOne(p => p.Facturas)
                .HasForeignKey<Facturas>(d => d.IdCita)
                .HasConstraintName("FK_facturas_citas");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_cliente");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_metodos_pago");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_sucursales");
        });

        modelBuilder.Entity<HistorialCitas>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__historia__76E6C5025BB234DE");

            entity.ToTable("historial_citas");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.EstadoAnterior)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_anterior");
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_nuevo");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.IdUsuarioCambio).HasColumnName("id_usuario_cambio");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.HistorialCitas)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("FK_historial_citas");

            entity.HasOne(d => d.IdUsuarioCambioNavigation).WithMany(p => p.HistorialCitas)
                .HasForeignKey(d => d.IdUsuarioCambio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_historial_usuarios");
        });

        modelBuilder.Entity<HorariosBarberos>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__horarios__C5836D695C172579");

            entity.ToTable("horarios_barberos");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(15)
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdBarbero).HasColumnName("id_barbero");

            entity.HasOne(d => d.IdBarberoNavigation).WithMany(p => p.HorariosBarberos)
                .HasForeignKey(d => d.IdBarbero)
                .HasConstraintName("FK_horarios_usuarios");
        });

        modelBuilder.Entity<MetodosPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__metodos___85BE0EBCB60FFFE8");

            entity.ToTable("metodos_pago");

            entity.HasIndex(e => e.Nombre, "UQ__metodos___72AFBCC65536CC69").IsUnique();

            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<PaqueteServicios>(entity =>
        {
            entity.HasKey(e => e.IdPaqueteServicio).HasName("PK__paquete___B1C5550F38771763");

            entity.ToTable("paquete_servicios");

            entity.HasIndex(e => new { e.IdPaquete, e.IdServicio }, "UQ_paquete_servicios").IsUnique();

            entity.Property(e => e.IdPaqueteServicio).HasColumnName("id_paquete_servicio");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK_paquete_servicios_paquetes");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_paquete_servicios_servicios");
        });

        modelBuilder.Entity<Paquetes>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__paquetes__609C3BCBD0090C65");

            entity.ToTable("paquetes");

            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsPersonalizable).HasColumnName("es_personalizable");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_total");
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__FF341C0D7A4211A3");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoriaProducto).HasColumnName("id_categoria_producto");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .HasColumnName("imagen");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .HasColumnName("marca");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Presentacion)
                .HasMaxLength(100)
                .HasColumnName("presentacion");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoriaProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_productos_categorias");
        });

        modelBuilder.Entity<Resenas>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("PK__resenas__06CD9363612C412C");

            entity.ToTable("resenas");

            entity.HasIndex(e => e.IdCita, "UQ__resenas__6AEC3C0842CFCFB7").IsUnique();

            entity.Property(e => e.IdResena).HasColumnName("id_resena");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .HasColumnName("comentario");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

            entity.HasOne(d => d.IdCitaNavigation).WithOne(p => p.Resenas)
                .HasForeignKey<Resenas>(d => d.IdCita)
                .HasConstraintName("FK_resenas_citas");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_resenas_cliente");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles__6ABCB5E0AB02341F");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "UQ__roles__72AFBCC620BCCC8C").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Servicios>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__servicio__6FD07FDC37CCB76F");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.DuracionMinutos)
                .HasDefaultValue(30)
                .HasColumnName("duracion_minutos");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_servicios_categorias");
        });

        modelBuilder.Entity<Sucursales>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PK__sucursal__4C75801382178F73");

            entity.ToTable("sucursales");

            entity.HasIndex(e => e.Nombre, "UQ__sucursal__72AFBCC67A8B15BF").IsUnique();

            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04ADBC9DB1ED");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "UQ__usuarios__2A586E0BED67AAF3").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuarios_roles");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("FK_usuarios_sucursales");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
