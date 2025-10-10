using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ArenasProyect3Web.Models
{
    public partial class BD_VENTAS_2Context : DbContext
    {
        public BD_VENTAS_2Context()
        {
        }

        public BD_VENTAS_2Context(DbContextOptions<BD_VENTAS_2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Actum> Acta { get; set; } = null!;
        public virtual DbSet<Almacen> Almacens { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<AreaGeneral> AreaGenerals { get; set; } = null!;
        public virtual DbSet<Auditora> Auditoras { get; set; } = null!;
        public virtual DbSet<AuditoraGeneral> AuditoraGenerals { get; set; } = null!;
        public virtual DbSet<Banco> Bancos { get; set; } = null!;
        public virtual DbSet<Bonificacion> Bonificacions { get; set; } = null!;
        public virtual DbSet<Brochure> Brochures { get; set; } = null!;
        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<CentroCosto> CentroCostos { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<CondicionPago> CondicionPagos { get; set; } = null!;
        public virtual DbSet<Correlativo> Correlativos { get; set; } = null!;
        public virtual DbSet<Cotizacion> Cotizacions { get; set; } = null!;
        public virtual DbSet<DatosAnexosBienesSujetoPercepcion> DatosAnexosBienesSujetoPercepcions { get; set; } = null!;
        public virtual DbSet<DatosAnexosClienteCindicion> DatosAnexosClienteCindicions { get; set; } = null!;
        public virtual DbSet<DatosAnexosClienteContacto> DatosAnexosClienteContactos { get; set; } = null!;
        public virtual DbSet<DatosAnexosClienteSucursal> DatosAnexosClienteSucursals { get; set; } = null!;
        public virtual DbSet<DatosAnexosClienteUnidad> DatosAnexosClienteUnidads { get; set; } = null!;
        public virtual DbSet<DatosAnexosOrigen> DatosAnexosOrigens { get; set; } = null!;
        public virtual DbSet<DatosAnexosProductoImportacion> DatosAnexosProductoImportacions { get; set; } = null!;
        public virtual DbSet<DatosAnexosProductoStockUbicacion> DatosAnexosProductoStockUbicacions { get; set; } = null!;
        public virtual DbSet<DatosAnexosProductoSunat> DatosAnexosProductoSunats { get; set; } = null!;
        public virtual DbSet<DatosAnexosProveedorContacto> DatosAnexosProveedorContactos { get; set; } = null!;
        public virtual DbSet<DatosAnexosProveedorCuentaProducto> DatosAnexosProveedorCuentaProductos { get; set; } = null!;
        public virtual DbSet<DatosAnexosProveedorCuentasBancaria> DatosAnexosProveedorCuentasBancarias { get; set; } = null!;
        public virtual DbSet<DatosAnexosProveedorSucursal> DatosAnexosProveedorSucursals { get; set; } = null!;
        public virtual DbSet<DatosAnexosTerminosCompra> DatosAnexosTerminosCompras { get; set; } = null!;
        public virtual DbSet<DatosAnexosTipoExistencium> DatosAnexosTipoExistencia { get; set; } = null!;
        public virtual DbSet<DefinicionFormulacione> DefinicionFormulaciones { get; set; } = null!;
        public virtual DbSet<DescripcionCaracteristica> DescripcionCaracteristicas { get; set; } = null!;
        public virtual DbSet<DescripcionDiametro> DescripcionDiametros { get; set; } = null!;
        public virtual DbSet<DescripcionDiseñoAcabado> DescripcionDiseñoAcabados { get; set; } = null!;
        public virtual DbSet<DescripcionEspesore> DescripcionEspesores { get; set; } = null!;
        public virtual DbSet<DescripcionForma> DescripcionFormas { get; set; } = null!;
        public virtual DbSet<DescripcionMedida> DescripcionMedidas { get; set; } = null!;
        public virtual DbSet<DescripcionNtipo> DescripcionNtipos { get; set; } = null!;
        public virtual DbSet<DescripcionVarios0> DescripcionVarios0s { get; set; } = null!;
        public virtual DbSet<DetalleCantidadCalidad> DetalleCantidadCalidads { get; set; } = null!;
        public virtual DbSet<DetalleCantidadOp> DetalleCantidadOps { get; set; } = null!;
        public virtual DbSet<DetalleCantidadOt> DetalleCantidadOts { get; set; } = null!;
        public virtual DbSet<DetalleCantidadesCalidad> DetalleCantidadesCalidads { get; set; } = null!;
        public virtual DbSet<DetalleCantidadesOp> DetalleCantidadesOps { get; set; } = null!;
        public virtual DbSet<DetalleCantidadesOt> DetalleCantidadesOts { get; set; } = null!;
        public virtual DbSet<DetalleClienteLiquidacionVentum> DetalleClienteLiquidacionVenta { get; set; } = null!;
        public virtual DbSet<DetalleClienteRequerimientoVentum> DetalleClienteRequerimientoVenta { get; set; } = null!;
        public virtual DbSet<DetalleCotizacion> DetalleCotizacions { get; set; } = null!;
        public virtual DbSet<DetalleLiquidacionVentum> DetalleLiquidacionVenta { get; set; } = null!;
        public virtual DbSet<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = null!;
        public virtual DbSet<DetallePedido> DetallePedidos { get; set; } = null!;
        public virtual DbSet<DetalleRegistroCantidad> DetalleRegistroCantidads { get; set; } = null!;
        public virtual DbSet<DetalleRequerimientoSimple> DetalleRequerimientoSimples { get; set; } = null!;
        public virtual DbSet<DetalleRequerimientoVentum> DetalleRequerimientoVenta { get; set; } = null!;
        public virtual DbSet<DetalleVendedorLiquidacionVentum> DetalleVendedorLiquidacionVenta { get; set; } = null!;
        public virtual DbSet<DetalleVendedorRequerimientoVentum> DetalleVendedorRequerimientoVenta { get; set; } = null!;
        public virtual DbSet<Diferencial> Diferencials { get; set; } = null!;
        public virtual DbSet<EquipoArea> EquipoAreas { get; set; } = null!;
        public virtual DbSet<EstadoSistema> EstadoSistemas { get; set; } = null!;
        public virtual DbSet<EstadoSistemaInicio> EstadoSistemaInicios { get; set; } = null!;
        public virtual DbSet<FormaPago> FormaPagos { get; set; } = null!;
        public virtual DbSet<Formulacion> Formulacions { get; set; } = null!;
        public virtual DbSet<FormulacionActividadesProducto> FormulacionActividadesProductos { get; set; } = null!;
        public virtual DbSet<FormulacionActividadesSemiProducido> FormulacionActividadesSemiProducidos { get; set; } = null!;
        public virtual DbSet<FormulacionMateriale> FormulacionMateriales { get; set; } = null!;
        public virtual DbSet<Kardex> Kardices { get; set; } = null!;
        public virtual DbSet<KardexEntradaAlmacen> KardexEntradaAlmacens { get; set; } = null!;
        public virtual DbSet<KardexEntradaAlmacenDetalle> KardexEntradaAlmacenDetalles { get; set; } = null!;
        public virtual DbSet<KardexSalidaAlmacen> KardexSalidaAlmacens { get; set; } = null!;
        public virtual DbSet<KardexSalidaAlmacenDetalle> KardexSalidaAlmacenDetalles { get; set; } = null!;
        public virtual DbSet<Linea> Lineas { get; set; } = null!;
        public virtual DbSet<LineaTrabajo> LineaTrabajos { get; set; } = null!;
        public virtual DbSet<LineaXoperacion> LineaXoperacions { get; set; } = null!;
        public virtual DbSet<LineaXoperacionXmaquinarium> LineaXoperacionXmaquinaria { get; set; } = null!;
        public virtual DbSet<LiquidacionVentum> LiquidacionVenta { get; set; } = null!;
        public virtual DbSet<ListarOpcantidade> ListarOpcantidades { get; set; } = null!;
        public virtual DbSet<ListarOtcantidade> ListarOtcantidades { get; set; } = null!;
        public virtual DbSet<ListarProductosRequerimientoGeneral> ListarProductosRequerimientoGenerals { get; set; } = null!;
        public virtual DbSet<Local> Locals { get; set; } = null!;
        public virtual DbSet<Maquinaria> Maquinarias { get; set; } = null!;
        public virtual DbSet<Medidum> Medida { get; set; } = null!;
        public virtual DbSet<Modelo> Modelos { get; set; } = null!;
        public virtual DbSet<ModeloXcamposPredeterminado> ModeloXcamposPredeterminados { get; set; } = null!;
        public virtual DbSet<ModeloXcamposPredeterminadosDetalle> ModeloXcamposPredeterminadosDetalles { get; set; } = null!;
        public virtual DbSet<ModeloXoperacion> ModeloXoperacions { get; set; } = null!;
        public virtual DbSet<ModeloXoperacionXmaquinarium> ModeloXoperacionXmaquinaria { get; set; } = null!;
        public virtual DbSet<MostrarOrdenCompraItemsGeneralLogistica> MostrarOrdenCompraItemsGeneralLogisticas { get; set; } = null!;
        public virtual DbSet<MostrarRequerimientoGeneralLosgistica> MostrarRequerimientoGeneralLosgisticas { get; set; } = null!;
        public virtual DbSet<MostrarRequerimientoItemsGeneralLosgistica> MostrarRequerimientoItemsGeneralLosgisticas { get; set; } = null!;
        public virtual DbSet<Operacione> Operaciones { get; set; } = null!;
        public virtual DbSet<OrdenCompra> OrdenCompras { get; set; } = null!;
        public virtual DbSet<OrdenProduccion> OrdenProduccions { get; set; } = null!;
        public virtual DbSet<OrdenServicio> OrdenServicios { get; set; } = null!;
        public virtual DbSet<PausaActiva> PausaActivas { get; set; } = null!;
        public virtual DbSet<Pedido> Pedidos { get; set; } = null!;
        public virtual DbSet<Perfil> Perfils { get; set; } = null!;
        public virtual DbSet<PlanoProducto> PlanoProductos { get; set; } = null!;
        public virtual DbSet<PlanoXproducto> PlanoXproductos { get; set; } = null!;
        public virtual DbSet<Prioridade> Prioridades { get; set; } = null!;
        public virtual DbSet<ProcesoSistema> ProcesoSistemas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<ProductoXcamposSeleccionadosDetalle> ProductoXcamposSeleccionadosDetalles { get; set; } = null!;
        public virtual DbSet<ProductosXcamposSeleccionado> ProductosXcamposSeleccionados { get; set; } = null!;
        public virtual DbSet<Proveedore> Proveedores { get; set; } = null!;
        public virtual DbSet<ReporteOp> ReporteOps { get; set; } = null!;
        public virtual DbSet<ReporteOpCalidad> ReporteOpCalidads { get; set; } = null!;
        public virtual DbSet<ReporteOt> ReporteOts { get; set; } = null!;
        public virtual DbSet<ReporteProductosMaterialesFormulacion> ReporteProductosMaterialesFormulacions { get; set; } = null!;
        public virtual DbSet<ReporteProduto> ReporteProdutos { get; set; } = null!;
        public virtual DbSet<ReporteProdutosDetallePedido> ReporteProdutosDetallePedidos { get; set; } = null!;
        public virtual DbSet<ReporteProdutosDetallePedidoDashboard> ReporteProdutosDetallePedidoDashboards { get; set; } = null!;
        public virtual DbSet<ReporteSemiProductosMaterialesFormulacion> ReporteSemiProductosMaterialesFormulacions { get; set; } = null!;
        public virtual DbSet<RequerimientoSimple> RequerimientoSimples { get; set; } = null!;
        public virtual DbSet<RequerimientoVentum> RequerimientoVenta { get; set; } = null!;
        public virtual DbSet<Responsable> Responsables { get; set; } = null!;
        public virtual DbSet<SalidaNoConforme> SalidaNoConformes { get; set; } = null!;
        public virtual DbSet<Sede> Sedes { get; set; } = null!;
        public virtual DbSet<SistemaMensajerium> SistemaMensajeria { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;
        public virtual DbSet<TablaLicencia> TablaLicencias { get; set; } = null!;
        public virtual DbSet<TipoAccion> TipoAccions { get; set; } = null!;
        public virtual DbSet<TipoAlmacenEntradaSalidaAlmacen> TipoAlmacenEntradaSalidaAlmacens { get; set; } = null!;
        public virtual DbSet<TipoCambio> TipoCambios { get; set; } = null!;
        public virtual DbSet<TipoCliente> TipoClientes { get; set; } = null!;
        public virtual DbSet<TipoCuentum> TipoCuenta { get; set; } = null!;
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; } = null!;
        public virtual DbSet<TipoFormulacion> TipoFormulacions { get; set; } = null!;
        public virtual DbSet<TipoGrupo> TipoGrupos { get; set; } = null!;
        public virtual DbSet<TipoMoneda> TipoMonedas { get; set; } = null!;
        public virtual DbSet<TipoMovimientosEntradaSalidaAlmacen> TipoMovimientosEntradaSalidaAlmacens { get; set; } = null!;
        public virtual DbSet<TipoNotaIngreso> TipoNotaIngresos { get; set; } = null!;
        public virtual DbSet<TipoOperacionPro> TipoOperacionPros { get; set; } = null!;
        public virtual DbSet<TipoOrdenCompra> TipoOrdenCompras { get; set; } = null!;
        public virtual DbSet<TipoProveedor> TipoProveedors { get; set; } = null!;
        public virtual DbSet<TipoRequerimientoGeneral> TipoRequerimientoGenerals { get; set; } = null!;
        public virtual DbSet<TipoRetencion> TipoRetencions { get; set; } = null!;
        public virtual DbSet<Tipomercaderia> Tipomercaderias { get; set; } = null!;
        public virtual DbSet<Tipooperacion> Tipooperacions { get; set; } = null!;
        public virtual DbSet<TiposCaracteristica> TiposCaracteristicas { get; set; } = null!;
        public virtual DbSet<TiposDiametro> TiposDiametros { get; set; } = null!;
        public virtual DbSet<TiposDiseñoAcabado> TiposDiseñoAcabados { get; set; } = null!;
        public virtual DbSet<TiposEspesore> TiposEspesores { get; set; } = null!;
        public virtual DbSet<TiposForma> TiposFormas { get; set; } = null!;
        public virtual DbSet<TiposMedida> TiposMedidas { get; set; } = null!;
        public virtual DbSet<TiposNtipo> TiposNtipos { get; set; } = null!;
        public virtual DbSet<TiposVariosO> TiposVariosOs { get; set; } = null!;
        public virtual DbSet<Transferencium> Transferencia { get; set; } = null!;
        public virtual DbSet<UbicacionDepartamento> UbicacionDepartamentos { get; set; } = null!;
        public virtual DbSet<UbicacionDistrito> UbicacionDistritos { get; set; } = null!;
        public virtual DbSet<UbicacionPai> UbicacionPais { get; set; } = null!;
        public virtual DbSet<UbicacionProvincium> UbicacionProvincia { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Vehiculo> Vehiculos { get; set; } = null!;
        public virtual DbSet<Zona> Zonas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=192.168.1.154; database=BD_VENTAS_2; User Id=sa; Password=Arenas.2020!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actum>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Asistente1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Asistente2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Asistente3)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CargoCliente1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CargoCliente2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CargoCliente3)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContactoCliente1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContactoCliente2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContactoCliente3)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoCliente1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoCliente2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoCliente3)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActa).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaTermino).HasColumnType("datetime");

                entity.Property(e => e.TelefonoCliente1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoCliente2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoCliente3)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.HasKey(e => e.IdAlmacen);

                entity.ToTable("Almacen");

                entity.Property(e => e.Concepto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.IdArea);

                entity.ToTable("Area");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AreaGeneral>(entity =>
            {
                entity.HasKey(e => e.IdArea);

                entity.ToTable("AreaGeneral");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Auditora>(entity =>
            {
                entity.HasKey(e => e.IdAuditora);

                entity.ToTable("Auditora");

                entity.Property(e => e.Accion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAccion).HasColumnType("datetime");

                entity.Property(e => e.Mantenimiento)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Maquina)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuarioSesion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuditoraGeneral>(entity =>
            {
                entity.HasKey(e => e.IdAuditoraGeneral);

                entity.ToTable("AuditoraGeneral");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAccion).HasColumnType("datetime");

                entity.Property(e => e.Mantenimiento)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Maquina)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuarioSesion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Banco>(entity =>
            {
                entity.HasKey(e => e.IdBanco);

                entity.Property(e => e.Anotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bonificacion>(entity =>
            {
                entity.HasKey(e => e.IdBonificacion);

                entity.ToTable("Bonificacion");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Brochure>(entity =>
            {
                entity.HasKey(e => e.IdBrochures);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ruta).IsUnicode(false);
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.IdCargo);

                entity.ToTable("Cargo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CentroCosto>(entity =>
            {
                entity.HasKey(e => e.IdCentroCostos);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDistrito)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProvincia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Correo1)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Correo2)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Dni)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ldolares)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("LDolares");

                entity.Property(e => e.Lsoles)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("LSoles");

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OtroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Referencia)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ruc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoFijo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ubigeo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdGrupo)
                    .HasConstraintName("FK_Clientes_TipoGrupo");

                entity.HasOne(d => d.IdRetencionNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdRetencion)
                    .HasConstraintName("FK_Clientes_TipoRetencion");

                entity.HasOne(d => d.IdTipoClienteNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdTipoCliente)
                    .HasConstraintName("FK_Clientes_TipoClientes");

                entity.HasOne(d => d.IdTipoDocumentoNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdTipoDocumento)
                    .HasConstraintName("FK_Clientes_TipoDocumentos");

                entity.HasOne(d => d.IdTipoMonedaNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdTipoMoneda)
                    .HasConstraintName("FK_Clientes_TipoMonedas");
            });

            modelBuilder.Entity<CondicionPago>(entity =>
            {
                entity.HasKey(e => e.IdCondicionPago);

                entity.ToTable("CondicionPago");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Correlativo>(entity =>
            {
                entity.HasKey(e => e.IdCorrelativo);

                entity.ToTable("CORRELATIVO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(e => e.IdCotizacion);

                entity.ToTable("Cotizacion");

                entity.Property(e => e.CodigoCotizacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Exonerado).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FechaEmision).HasColumnType("datetime");

                entity.Property(e => e.FechaValidez).HasColumnType("datetime");

                entity.Property(e => e.Grarantia)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Igv).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Inafecta).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LugarEntrega)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MensajeAnulacion).IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Referencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaBrochureFinal).IsUnicode(false);

                entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TiempoEntrega)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalDescuento).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdAlmacenNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdAlmacen)
                    .HasConstraintName("FK_Cotizacion_Almacen");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_Cotizacion_Clientes");

                entity.HasOne(d => d.IdCondicionPagoNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdCondicionPago)
                    .HasConstraintName("FK_Cotizacion_CondicionPago");

                entity.HasOne(d => d.IdContactoNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdContacto)
                    .HasConstraintName("FK_Cotizacion_DatosAnexosCliente_Contacto");

                entity.HasOne(d => d.IdFormaPagoNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdFormaPago)
                    .HasConstraintName("FK_Cotizacion_FormaPago");

                entity.HasOne(d => d.IdModedaNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdModeda)
                    .HasConstraintName("FK_Cotizacion_TipoMonedas");

                entity.HasOne(d => d.IdResponsableNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdResponsable)
                    .HasConstraintName("FK_Cotizacion_Usuarios");

                entity.HasOne(d => d.IdUnidadNavigation)
                    .WithMany(p => p.Cotizacions)
                    .HasForeignKey(d => d.IdUnidad)
                    .HasConstraintName("FK_Cotizacion_DatosAnexosCliente_Unidad");
            });

            modelBuilder.Entity<DatosAnexosBienesSujetoPercepcion>(entity =>
            {
                entity.HasKey(e => e.IdBienesSujetoPercepcion);

                entity.ToTable("DatosAnexos_BienesSujetoPercepcion");

                entity.Property(e => e.CodigoBienesSujetoPercepcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatosAnexosClienteCindicion>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosClienteCondicion);

                entity.ToTable("DatosAnexosCliente_Cindicion");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.DatosAnexosClienteCindicions)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_DatosAnexosCliente_Cindicion_Clientes");

                entity.HasOne(d => d.IdCondicionPagoNavigation)
                    .WithMany(p => p.DatosAnexosClienteCindicions)
                    .HasForeignKey(d => d.IdCondicionPago)
                    .HasConstraintName("FK_DatosAnexosCliente_Cindicion_CondicionPago");

                entity.HasOne(d => d.IdFormaPagoNavigation)
                    .WithMany(p => p.DatosAnexosClienteCindicions)
                    .HasForeignKey(d => d.IdFormaPago)
                    .HasConstraintName("FK_DatosAnexosCliente_Cindicion_FormaPago");
            });

            modelBuilder.Entity<DatosAnexosClienteContacto>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosClienteContacto);

                entity.ToTable("DatosAnexosCliente_Contacto");

                entity.Property(e => e.Anexo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.DatosAnexosClienteContactos)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_DatosAnexosCliente_Contacto_Area");

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithMany(p => p.DatosAnexosClienteContactos)
                    .HasForeignKey(d => d.IdCargo)
                    .HasConstraintName("FK_DatosAnexosCliente_Contacto_Cargo");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.DatosAnexosClienteContactos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_DatosAnexosCliente_Contacto_Clientes");

                entity.HasOne(d => d.IdUnidadClienteNavigation)
                    .WithMany(p => p.DatosAnexosClienteContactos)
                    .HasForeignKey(d => d.IdUnidadCliente)
                    .HasConstraintName("FK_DatosAnexosCliente_Contacto_DatosAnexosCliente_Unidad");
            });

            modelBuilder.Entity<DatosAnexosClienteSucursal>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosClienteSucursal);

                entity.ToTable("DatosAnexosCliente_Sucursal");

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDistrito)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProvincia)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.DatosAnexosClienteSucursals)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_DatosAnexosCliente_Sucursal_Clientes");
            });

            modelBuilder.Entity<DatosAnexosClienteUnidad>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosClienteUnidad);

                entity.ToTable("DatosAnexosCliente_Unidad");

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Latitud).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Longitud).HasColumnType("decimal(18, 6)");

                entity.HasOne(d => d.IdResponsableNavigation)
                    .WithMany(p => p.DatosAnexosClienteUnidads)
                    .HasForeignKey(d => d.IdResponsable)
                    .HasConstraintName("FK_DatosAnexosCliente_Unidad_Usuarios");

                entity.HasOne(d => d.IdZonaNavigation)
                    .WithMany(p => p.DatosAnexosClienteUnidads)
                    .HasForeignKey(d => d.IdZona)
                    .HasConstraintName("FK_DatosAnexosCliente_Unidad_Zona");
            });

            modelBuilder.Entity<DatosAnexosOrigen>(entity =>
            {
                entity.HasKey(e => e.IdOrigen);

                entity.ToTable("DatosAnexos_Origen");

                entity.Property(e => e.CodigoOrigen)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatosAnexosProductoImportacion>(entity =>
            {
                entity.HasKey(e => e.IdImportacion);

                entity.ToTable("DatosAnexosProducto_Importacion");

                entity.Property(e => e.Contenedor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Medidas)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PesoContenedor).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.DatosAnexosProductoImportacions)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_DatosAnexosProducto_Importacion_PRODUCTOS");

                entity.HasOne(d => d.IdOrigenNavigation)
                    .WithMany(p => p.DatosAnexosProductoImportacions)
                    .HasForeignKey(d => d.IdOrigen)
                    .HasConstraintName("FK_DatosAnexosProducto_Importacion_DatosAnexos_Origen");

                entity.HasOne(d => d.IdTerminosCompraNavigation)
                    .WithMany(p => p.DatosAnexosProductoImportacions)
                    .HasForeignKey(d => d.IdTerminosCompra)
                    .HasConstraintName("FK_DatosAnexosProducto_Importacion_DatosAnexos_TerminosCompra");
            });

            modelBuilder.Entity<DatosAnexosProductoStockUbicacion>(entity =>
            {
                entity.HasKey(e => e.IdStockUbicacion);

                entity.ToTable("DatosAnexosProducto_StockUbicacion");

                entity.Property(e => e.AfectoIgv).HasColumnName("AfectoIGV");

                entity.Property(e => e.Maximo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Minimo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Peso).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.DatosAnexosProductoStockUbicacions)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_DatosAnexosProducto_StockUbicacion_PRODUCTOS");
            });

            modelBuilder.Entity<DatosAnexosProductoSunat>(entity =>
            {
                entity.HasKey(e => e.IdSunat);

                entity.ToTable("DatosAnexosProducto_Sunat");

                entity.Property(e => e.CodigoUnspcs)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CodigoUNSPCS");

                entity.Property(e => e.PorcentajeDetraccion).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PorcentajeIsc)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PorcentajeISC");

                entity.Property(e => e.PorcentajePercepcion).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SujetoIsc).HasColumnName("SujetoISC");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.DatosAnexosProductoSunats)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_DatosAnexosProducto_Sunat_PRODUCTOS");

                entity.HasOne(d => d.IdBienesSujetoPercepcionNavigation)
                    .WithMany(p => p.DatosAnexosProductoSunats)
                    .HasForeignKey(d => d.IdBienesSujetoPercepcion)
                    .HasConstraintName("FK_DatosAnexosProducto_Sunat_DatosAnexos_BienesSujetoPercepcion");

                entity.HasOne(d => d.IdTipoExistenciaNavigation)
                    .WithMany(p => p.DatosAnexosProductoSunats)
                    .HasForeignKey(d => d.IdTipoExistencia)
                    .HasConstraintName("FK_DatosAnexosProducto_Sunat_DatosAnexos_TipoExistencia");
            });

            modelBuilder.Entity<DatosAnexosProveedorContacto>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosProveedorContacto);

                entity.ToTable("DatosAnexosProveedor_Contacto");

                entity.Property(e => e.Correo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatosAnexosProveedorCuentaProducto>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosProveedorCuentaProducto);

                entity.ToTable("DatosAnexosProveedor_CuentaProducto");
            });

            modelBuilder.Entity<DatosAnexosProveedorCuentasBancaria>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosProveedorCuentaBancaria);

                entity.ToTable("DatosAnexosProveedor_CuentasBancarias");

                entity.Property(e => e.Cci)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CCI");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NumeroCUenta");

                entity.Property(e => e.TipoBanco)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatosAnexosProveedorSucursal>(entity =>
            {
                entity.HasKey(e => e.IdDatosAnexosProveedorSucursal);

                entity.ToTable("DatosAnexosProveedor_Sucursal");

                entity.Property(e => e.LugarEntrega)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreSucursal)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NOmbreSucursal");
            });

            modelBuilder.Entity<DatosAnexosTerminosCompra>(entity =>
            {
                entity.HasKey(e => e.IdTerminosCompra);

                entity.ToTable("DatosAnexos_TerminosCompra");

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoTerminosCompra)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatosAnexosTipoExistencium>(entity =>
            {
                entity.HasKey(e => e.IdTipoExistencia);

                entity.ToTable("DatosAnexos_TipoExistencia");

                entity.Property(e => e.CodigoTipoExistencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DefinicionFormulacione>(entity =>
            {
                entity.HasKey(e => e.IdDefinicionFormulaciones);

                entity.Property(e => e.CodigoDefinicion)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DescripcionCaracteristica>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionCaracteristicas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTipoNn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DescripcionTipoNN");

                entity.Property(e => e.IdTipoNn).HasColumnName("IdTipoNN");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionCaracteristicas)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionCaracteristicas_MODELOS");

                entity.HasOne(d => d.IdTipoCaracteristicasNavigation)
                    .WithMany(p => p.DescripcionCaracteristicas)
                    .HasForeignKey(d => d.IdTipoCaracteristicas)
                    .HasConstraintName("FK_DescripcionCaracteristicas_TiposCaracteristicas");
            });

            modelBuilder.Entity<DescripcionDiametro>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionDiametros);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionDiametros)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionDiametros_MODELOS");

                entity.HasOne(d => d.IdTipoDiametrosNavigation)
                    .WithMany(p => p.DescripcionDiametros)
                    .HasForeignKey(d => d.IdTipoDiametros)
                    .HasConstraintName("FK_DescripcionDiametros_TiposDiametros");
            });

            modelBuilder.Entity<DescripcionDiseñoAcabado>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionDiseñoAcabado);

                entity.ToTable("DescripcionDiseñoAcabado");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTipoNn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DescripcionTipoNN");

                entity.Property(e => e.IdTipoNn).HasColumnName("IdTipoNN");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionDiseñoAcabados)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionDiseñoAcabado_MODELOS");

                entity.HasOne(d => d.IdTipoDiseñoAcabadoNavigation)
                    .WithMany(p => p.DescripcionDiseñoAcabados)
                    .HasForeignKey(d => d.IdTipoDiseñoAcabado)
                    .HasConstraintName("FK_DescripcionDiseñoAcabado_TiposDiseñoAcabado");
            });

            modelBuilder.Entity<DescripcionEspesore>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionEspesores);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionEspesores)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionEspesores_MODELOS");

                entity.HasOne(d => d.IdTipoEspesoresNavigation)
                    .WithMany(p => p.DescripcionEspesores)
                    .HasForeignKey(d => d.IdTipoEspesores)
                    .HasConstraintName("FK_DescripcionEspesores_TiposEspesores");
            });

            modelBuilder.Entity<DescripcionForma>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionFormas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionFormas)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionFormas_MODELOS");

                entity.HasOne(d => d.IdTipoFormasNavigation)
                    .WithMany(p => p.DescripcionFormas)
                    .HasForeignKey(d => d.IdTipoFormas)
                    .HasConstraintName("FK_DescripcionFormas_TiposFormas");
            });

            modelBuilder.Entity<DescripcionMedida>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionMedidas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionMedida)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionMedidas_MODELOS");

                entity.HasOne(d => d.IdTipoMedidasNavigation)
                    .WithMany(p => p.DescripcionMedida)
                    .HasForeignKey(d => d.IdTipoMedidas)
                    .HasConstraintName("FK_DescripcionMedidas_TiposMedidas");
            });

            modelBuilder.Entity<DescripcionNtipo>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionNtipos);

                entity.ToTable("DescripcionNTipos");

                entity.Property(e => e.IdDescripcionNtipos).HasColumnName("IdDescripcionNTipos");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTipoNn)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DescripcionTipoNN");

                entity.Property(e => e.IdTipoNn).HasColumnName("IdTipoNN");

                entity.Property(e => e.IdTipoNtipos).HasColumnName("IdTipoNTipos");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionNtipos)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionNTipos_MODELOS");

                entity.HasOne(d => d.IdTipoNtiposNavigation)
                    .WithMany(p => p.DescripcionNtipos)
                    .HasForeignKey(d => d.IdTipoNtipos)
                    .HasConstraintName("FK_DescripcionNTipos_TiposNTipos");
            });

            modelBuilder.Entity<DescripcionVarios0>(entity =>
            {
                entity.HasKey(e => e.IdDescripcionVarios0);

                entity.ToTable("DescripcionVarios0");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTipoNn)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DescripcionTipoNN");

                entity.Property(e => e.IdTipoNn).HasColumnName("IdTipoNN");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.DescripcionVarios0s)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_DescripcionVarios0_MODELOS");

                entity.HasOne(d => d.IdTipoVarios0Navigation)
                    .WithMany(p => p.DescripcionVarios0s)
                    .HasForeignKey(d => d.IdTipoVarios0)
                    .HasConstraintName("FK_DescripcionVarios0_TiposVariosO");
            });

            modelBuilder.Entity<DetalleCantidadCalidad>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DetalleCantidadCalidad");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CodigoOpC).HasColumnName("CODIGO OP C");

                entity.Property(e => e.ÚltimaFechaDeIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("ÚLTIMA FECHA DE INGRESO");
            });

            modelBuilder.Entity<DetalleCantidadOp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DetalleCantidadOP");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CodigoOp).HasColumnName("CODIGO OP");

                entity.Property(e => e.ÚltimaFechaDeIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("ÚLTIMA FECHA DE INGRESO");
            });

            modelBuilder.Entity<DetalleCantidadOt>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DetalleCantidadOT");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CodigoOt).HasColumnName("CODIGO OT");
            });

            modelBuilder.Entity<DetalleCantidadesCalidad>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCantidadCalidad);

                entity.ToTable("DetalleCantidadesCalidad");

                entity.Property(e => e.EstadoAd).HasColumnName("EstadoAD");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Obserbaciones).IsUnicode(false);

                entity.Property(e => e.PesoReal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PesoTeorico).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<DetalleCantidadesOp>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCantidadOrdenProduccion);

                entity.ToTable("DetalleCantidadesOP");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<DetalleCantidadesOt>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCantidadOrdenServicio);

                entity.ToTable("DetalleCantidadesOT");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<DetalleClienteLiquidacionVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleClienteLiquidacionVenta);

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaTermino).HasColumnType("datetime");
            });

            modelBuilder.Entity<DetalleClienteRequerimientoVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleClienteRequerimientoVenta);

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdCliente).HasColumnName("IdCLiente");
            });

            modelBuilder.Entity<DetalleCotizacion>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCotizacion);

                entity.ToTable("DetalleCotizacion");

                entity.Property(e => e.CodigoCliente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionCliente)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrecioUnidad).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Ta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.DetalleCotizacions)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_DetalleCotizacion_PRODUCTOS");

                entity.HasOne(d => d.IdBonificacionNavigation)
                    .WithMany(p => p.DetalleCotizacions)
                    .HasForeignKey(d => d.IdBonificacion)
                    .HasConstraintName("FK_DetalleCotizacion_Bonificacion");

                entity.HasOne(d => d.IdCotizacionNavigation)
                    .WithMany(p => p.DetalleCotizacions)
                    .HasForeignKey(d => d.IdCotizacion)
                    .HasConstraintName("FK_DetalleCotizacion_Cotizacion");
            });

            modelBuilder.Entity<DetalleLiquidacionVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleLiquidacionVenta);

                entity.Property(e => e.Conbustible)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaLiquidacion).HasColumnType("datetime");

                entity.Property(e => e.Hospedaje)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Movilidad)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Otros)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Peaje)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Subtotal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Viatico)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetalleOrdenCompra>(entity =>
            {
                entity.HasKey(e => e.IdDetalleOrdenCompra);

                entity.ToTable("DetalleOrdenCompra");

                entity.Property(e => e.IdDetalleOrdenCompra).HasColumnName("IdDetalleOrdenCOmpra");

                entity.Property(e => e.Cantidad)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionProductoProveedor).IsUnicode(false);

                entity.Property(e => e.Descuento)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEntregaReal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEstimada)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Precio)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.HasKey(e => e.IdDetallePedido);

                entity.ToTable("DetallePedido");

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FechaEntrega).HasColumnType("datetime");

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<DetalleRegistroCantidad>(entity =>
            {
                entity.HasKey(e => e.IdRegistroCantidades);

                entity.ToTable("DetalleRegistroCantidad");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<DetalleRequerimientoSimple>(entity =>
            {
                entity.HasKey(e => e.IdDetalleRequerimientoSimple);

                entity.ToTable("DetalleRequerimientoSimple");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CantidadRetirada).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CantidadTotal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Stock).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<DetalleRequerimientoVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleRequerimientoVenta);

                entity.Property(e => e.Combustible)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRequerimeinto).HasColumnType("datetime");

                entity.Property(e => e.Hospedaje)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Movilidad)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Otros)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Peaje)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SubTotal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Viatico)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRequerimientoVentaNavigation)
                    .WithMany(p => p.DetalleRequerimientoVenta)
                    .HasForeignKey(d => d.IdRequerimientoVenta)
                    .HasConstraintName("FK_DetalleRequerimientoVenta_RequerimientoVenta");
            });

            modelBuilder.Entity<DetalleVendedorLiquidacionVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVnededorLiquidacionVenta);
            });

            modelBuilder.Entity<DetalleVendedorRequerimientoVentum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVendedorRequerimientoVenta);
            });

            modelBuilder.Entity<Diferencial>(entity =>
            {
                entity.HasKey(e => e.IdDiferencial);

                entity.ToTable("DIFERENCIAL");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EquipoArea>(entity =>
            {
                entity.HasKey(e => e.IdEquipoArea);

                entity.ToTable("EquipoArea");

                entity.Property(e => e.Consumo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionEquipoArea)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IdCliente).HasColumnName("IdCLiente");

                entity.Property(e => e.MontoPromedioAnual)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MontoPromedioVenta)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Precio)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoSistema>(entity =>
            {
                entity.HasKey(e => e.IdEstadoSistema);

                entity.ToTable("EstadoSistema");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.EstadoSistema1)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("EstadoSistema");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Maquina)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioDispositivo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioSistema)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoSistemaInicio>(entity =>
            {
                entity.HasKey(e => e.IdEstadoSistemaInicio);

                entity.ToTable("EstadoSistemaInicio");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAparicion).HasColumnType("datetime");

                entity.Property(e => e.FechaInstalacionSsitema)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NuevasFuncionesNovedades).IsUnicode(false);

                entity.Property(e => e.VersionSsitema)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormaPago>(entity =>
            {
                entity.HasKey(e => e.IdFormaPago);

                entity.ToTable("FormaPago");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Formulacion>(entity =>
            {
                entity.HasKey(e => e.IdFormulacion);

                entity.ToTable("Formulacion");

                entity.Property(e => e.Cif).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NamePlanoSeguridad).IsUnicode(false);

                entity.Property(e => e.NamePlanoTecnico).IsUnicode(false);

                entity.Property(e => e.PlanoSeguridad)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoTecnico)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RelacionProXsemi).HasColumnName("RelacionProXSemi");
            });

            modelBuilder.Entity<FormulacionActividadesProducto>(entity =>
            {
                entity.HasKey(e => e.IdActividadFormulacionProducto);

                entity.ToTable("FormulacionActividadesProducto");

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cpersonal)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CPersonal");

                entity.Property(e => e.Ctotal)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CTotal");

                entity.Property(e => e.IdLom).HasColumnName("IdLOM");

                entity.Property(e => e.Tcosto).HasColumnName("TCosto");

                entity.Property(e => e.Thoras).HasColumnName("THoras");

                entity.Property(e => e.Toperacion)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("TOperacion");

                entity.Property(e => e.Tpor).HasColumnName("TPor");

                entity.Property(e => e.Tsetup)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("TSetup");

                entity.HasOne(d => d.IdCoorelativoNavigation)
                    .WithMany(p => p.FormulacionActividadesProductos)
                    .HasForeignKey(d => d.IdCoorelativo)
                    .HasConstraintName("FK_FormulacionActividadesProducto_CORRELATIVO");

                entity.HasOne(d => d.IdLomNavigation)
                    .WithMany(p => p.FormulacionActividadesProductos)
                    .HasForeignKey(d => d.IdLom)
                    .HasConstraintName("FK_FormulacionActividadesProducto_LineaXOperacionXMaquinaria");

                entity.HasOne(d => d.IdTipoOperacionNavigation)
                    .WithMany(p => p.FormulacionActividadesProductos)
                    .HasForeignKey(d => d.IdTipoOperacion)
                    .HasConstraintName("FK_FormulacionActividadesProducto_TIPOOPERACION");
            });

            modelBuilder.Entity<FormulacionActividadesSemiProducido>(entity =>
            {
                entity.HasKey(e => e.IdActividadFormulacionSemiProducido);

                entity.ToTable("FormulacionActividadesSemiProducido");

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cpersonal)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CPersonal");

                entity.Property(e => e.Ctotal)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CTotal");

                entity.Property(e => e.IdMom).HasColumnName("IdMOM");

                entity.Property(e => e.Tcosto).HasColumnName("TCosto");

                entity.Property(e => e.Thoras).HasColumnName("THoras");

                entity.Property(e => e.Toperacion)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("TOperacion");

                entity.Property(e => e.Tpor).HasColumnName("TPor");

                entity.Property(e => e.Tsetup)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("TSetup");
            });

            modelBuilder.Entity<FormulacionMateriale>(entity =>
            {
                entity.HasKey(e => e.IdMaterialOperacion);

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CantidadProducto).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CodigoFormulacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoMaterial)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Kardex>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("KARDEX");

                entity.Property(e => e.Almacen)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("ALMACEN");

                entity.Property(e => e.AlmacenGeneral)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("ALMACEN GENERAL");

                entity.Property(e => e.CTotalDólares)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL DÓLARES");

                entity.Property(e => e.CTotalEntradaDólares)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL ENTRADA DÓLARES");

                entity.Property(e => e.CTotalEntradaSoles)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL ENTRADA SOLES");

                entity.Property(e => e.CTotalSalidaDólares)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL SALIDA DÓLARES");

                entity.Property(e => e.CTotalSalidaSoles)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL SALIDA SOLES");

                entity.Property(e => e.CTotalSoles)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("C. TOTAL SOLES");

                entity.Property(e => e.Código)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO");

                entity.Property(e => e.Entradas)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("ENTRADAS");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.Guía)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GUÍA");

                entity.Property(e => e.PStockDólares)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("P. STOCK DÓLARES");

                entity.Property(e => e.PStockSoles)
                    .HasColumnType("decimal(10, 4)")
                    .HasColumnName("P. STOCK SOLES");

                entity.Property(e => e.PrecioUnitEntradaDólares)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("PRECIO UNIT. ENTRADA DÓLARES");

                entity.Property(e => e.PrecioUnitEntradaSoles)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("PRECIO UNIT. ENTRADA SOLES");

                entity.Property(e => e.PrecioUnitSalidaDólares)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("PRECIO UNIT. SALIDA DÓLARES");

                entity.Property(e => e.PrecioUnitSalidaSoles)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("PRECIO UNIT. SALIDA SOLES");

                entity.Property(e => e.Producto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO");

                entity.Property(e => e.Salida)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("SALIDA");
            });

            modelBuilder.Entity<KardexEntradaAlmacen>(entity =>
            {
                entity.HasKey(e => e.IdEntradaAlmacen);

                entity.ToTable("Kardex_EntradaAlmacen");

                entity.Property(e => e.CodigoEntradaAlmacen)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEntrada).HasColumnType("datetime");

                entity.Property(e => e.FechaGuia).HasColumnType("datetime");

                entity.Property(e => e.FechaOrden).HasColumnType("datetime");

                entity.Property(e => e.NumeroDoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroGuia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroOrden)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.PdfdocuemntoAdjunto)
                    .IsUnicode(false)
                    .HasColumnName("PDFDocuemntoAdjunto");
            });

            modelBuilder.Entity<KardexEntradaAlmacenDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalleEntradaAlmacen);

                entity.ToTable("Kardex_EntradaAlmacenDetalles");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PrecioTotalDolares).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioTotalSoles).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioUnitarioDolares).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioUnitarioSoles).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<KardexSalidaAlmacen>(entity =>
            {
                entity.HasKey(e => e.IdSalidaAlmacen);

                entity.ToTable("Kardex_SalidaAlmacen");

                entity.Property(e => e.CentroCostos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoSalidaAlmacen)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNs).HasColumnName("EstadoNS");

                entity.Property(e => e.FechaOrden).HasColumnType("datetime");

                entity.Property(e => e.FechaRequerimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaSalida).HasColumnType("datetime");

                entity.Property(e => e.NumeroOrden)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRequerimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);
            });

            modelBuilder.Entity<KardexSalidaAlmacenDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalleSalidaAlmacen);

                entity.ToTable("Kardex_SalidaAlmacenDetalles");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PrecioTotalDolares).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioTotalSoles).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioUnitarioDolares).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecioUnitarioSoles).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<Linea>(entity =>
            {
                entity.HasKey(e => e.IdLinea);

                entity.ToTable("LINEAS");

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTipMerNavigation)
                    .WithMany(p => p.Lineas)
                    .HasForeignKey(d => d.IdTipMer)
                    .HasConstraintName("FK_LINEAS_TIPOMERCADERIAS");
            });

            modelBuilder.Entity<LineaTrabajo>(entity =>
            {
                entity.HasKey(e => e.IdLineaTrabajo);

                entity.ToTable("LineaTrabajo");

                entity.Property(e => e.AccionesDescripcion).IsUnicode(false);

                entity.Property(e => e.AntecedentesDescripcion).IsUnicode(false);

                entity.Property(e => e.DesarrolloDescripcion).IsUnicode(false);

                entity.Property(e => e.FechaAcciones).HasColumnType("datetime");

                entity.Property(e => e.GastoLinea).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Imagen1).IsUnicode(false);

                entity.Property(e => e.Imagen2).IsUnicode(false);

                entity.Property(e => e.Imagen3).IsUnicode(false);

                entity.Property(e => e.ResultadoDescripcion).IsUnicode(false);
            });

            modelBuilder.Entity<LineaXoperacion>(entity =>
            {
                entity.HasKey(e => e.IdLineaXoperacion);

                entity.ToTable("LineaXOperacion");

                entity.Property(e => e.IdLineaXoperacion).HasColumnName("IdLineaXOperacion");

                entity.HasOne(d => d.IdLineaNavigation)
                    .WithMany(p => p.LineaXoperacions)
                    .HasForeignKey(d => d.IdLinea)
                    .HasConstraintName("FK_LineaXOperacion_LINEAS");

                entity.HasOne(d => d.IdOperacionNavigation)
                    .WithMany(p => p.LineaXoperacions)
                    .HasForeignKey(d => d.IdOperacion)
                    .HasConstraintName("FK_LineaXOperacion_OPERACIONES");
            });

            modelBuilder.Entity<LineaXoperacionXmaquinarium>(entity =>
            {
                entity.HasKey(e => e.IdLineaXoperacioXmaquinaria);

                entity.ToTable("LineaXOperacionXMaquinaria");

                entity.Property(e => e.IdLineaXoperacioXmaquinaria).HasColumnName("IdLineaXOperacioXMaquinaria");

                entity.HasOne(d => d.IdLineaNavigation)
                    .WithMany(p => p.LineaXoperacionXmaquinaria)
                    .HasForeignKey(d => d.IdLinea)
                    .HasConstraintName("FK_LineaXOperacionXMaquinaria_LINEAS");

                entity.HasOne(d => d.IdMaquinariaNavigation)
                    .WithMany(p => p.LineaXoperacionXmaquinaria)
                    .HasForeignKey(d => d.IdMaquinaria)
                    .HasConstraintName("FK_LineaXOperacionXMaquinaria_MAQUINARIAS");

                entity.HasOne(d => d.IdOperacionNavigation)
                    .WithMany(p => p.LineaXoperacionXmaquinaria)
                    .HasForeignKey(d => d.IdOperacion)
                    .HasConstraintName("FK_LineaXOperacionXMaquinaria_OPERACIONES");
            });

            modelBuilder.Entity<LiquidacionVentum>(entity =>
            {
                entity.HasKey(e => e.IdLiquidacion);

                entity.Property(e => e.IdLiquidacion).ValueGeneratedNever();

                entity.Property(e => e.Adelanto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaLiquidacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTermino).HasColumnType("datetime");

                entity.Property(e => e.ItinerarioViaje)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MotivoVisita)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Saldo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdJefaturaNavigation)
                    .WithMany(p => p.LiquidacionVentumIdJefaturaNavigations)
                    .HasForeignKey(d => d.IdJefatura)
                    .HasConstraintName("FK_LiquidacionVenta_Usuarios1");

                entity.HasOne(d => d.IdRequerimeintoNavigation)
                    .WithMany(p => p.LiquidacionVenta)
                    .HasForeignKey(d => d.IdRequerimeinto)
                    .HasConstraintName("FK_LiquidacionVenta_RequerimientoVenta");

                entity.HasOne(d => d.IdTipoMonedaNavigation)
                    .WithMany(p => p.LiquidacionVenta)
                    .HasForeignKey(d => d.IdTipoMoneda)
                    .HasConstraintName("FK_LiquidacionVenta_TipoMonedas");

                entity.HasOne(d => d.IdVehiculoNavigation)
                    .WithMany(p => p.LiquidacionVenta)
                    .HasForeignKey(d => d.IdVehiculo)
                    .HasConstraintName("FK_LiquidacionVenta_Vehiculos");

                entity.HasOne(d => d.IdVendedorNavigation)
                    .WithMany(p => p.LiquidacionVentumIdVendedorNavigations)
                    .HasForeignKey(d => d.IdVendedor)
                    .HasConstraintName("FK_LiquidacionVenta_Usuarios");
            });

            modelBuilder.Entity<ListarOpcantidade>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ListarOPCantidades");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO MATERNO");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO PATERNO");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CantidadRealizada).HasColumnName("CANTIDAD REALIZADA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(1003)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COLOR");

                entity.Property(e => e.DescripciónDelProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN DEL PRODUCTO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.EstadoDeOc).HasColumnName("ESTADO DE OC");

                entity.Property(e => e.FechaDeEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE ENTREGA");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE INICIO");

                entity.Property(e => e.FechaProduccion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA PRODUCCION");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.NOp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OP");

                entity.Property(e => e.NPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("N°. PEDIDO");

                entity.Property(e => e.Oc)
                    .IsUnicode(false)
                    .HasColumnName("OC");

                entity.Property(e => e.Pl)
                    .IsUnicode(false)
                    .HasColumnName("PL");

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PRIMER NOMBRE");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO NOMBRE");

                entity.Property(e => e.Unidad)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");
            });

            modelBuilder.Entity<ListarOtcantidade>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ListarOTCantidades");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CantidadRealizada).HasColumnName("CANTIDAD REALIZADA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(1003)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COLOR");

                entity.Property(e => e.DescripciónDelSubProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN DEL SUB-PRODUCTO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.FechaDeEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE ENTREGA");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE INICIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NOp).HasColumnName("N°. OP");

                entity.Property(e => e.NOt)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OT");
            });

            modelBuilder.Entity<ListarProductosRequerimientoGeneral>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ListarProductosRequerimientoGeneral");

                entity.Property(e => e.CantidadMinima)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CANTIDAD MINIMA");

                entity.Property(e => e.Código)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO");

                entity.Property(e => e.CódigoBss)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO BSS");

                entity.Property(e => e.CódigoInterno).HasColumnName("CÓDIGO INTERNO");

                entity.Property(e => e.CódigoLínea).HasColumnName("CÓDIGO LÍNEA");

                entity.Property(e => e.CódigoMercaderiaCuenta).HasColumnName("CÓDIGO MERCADERIA/CUENTA");

                entity.Property(e => e.CódigoModelo).HasColumnName("CÓDIGO MODELO");

                entity.Property(e => e.Proceso).HasColumnName("PROCESO");

                entity.Property(e => e.Producto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO");

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("STOCK");

                entity.Property(e => e.TipoDeMedida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIPO DE MEDIDA");

                entity.Property(e => e.VCritico).HasColumnName("V_CRITICO");
            });

            modelBuilder.Entity<Local>(entity =>
            {
                entity.HasKey(e => e.IdLocal);

                entity.ToTable("Local");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Maquinaria>(entity =>
            {
                entity.HasKey(e => e.IdMaquinarias);

                entity.ToTable("MAQUINARIAS");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Medidum>(entity =>
            {
                entity.HasKey(e => e.IdMedida);

                entity.ToTable("MEDIDA");

                entity.Property(e => e.IdMedida)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.HasKey(e => e.IdModelo);

                entity.ToTable("MODELOS");

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdLineaNavigation)
                    .WithMany(p => p.Modelos)
                    .HasForeignKey(d => d.IdLinea)
                    .HasConstraintName("FK_MODELOS_LINEAS");
            });

            modelBuilder.Entity<ModeloXcamposPredeterminado>(entity =>
            {
                entity.HasKey(e => e.IdModeloXcamposPredeterminado);

                entity.ToTable("ModeloXCamposPredeterminados");

                entity.Property(e => e.IdModeloXcamposPredeterminado).HasColumnName("IdModeloXCamposPredeterminado");

                entity.Property(e => e.CampNtipos1).HasColumnName("CampNTipos1");

                entity.Property(e => e.CampNtipos2).HasColumnName("CampNTipos2");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.ModeloXcamposPredeterminados)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_ModeloXCamposPredeterminados_MODELOS");
            });

            modelBuilder.Entity<ModeloXcamposPredeterminadosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdModeloXcamposPredeterminadosDetalle);

                entity.ToTable("ModeloXCamposPredeterminadosDetalle");

                entity.Property(e => e.IdModeloXcamposPredeterminadosDetalle).HasColumnName("IdModeloXCamposPredeterminadosDetalle");

                entity.Property(e => e.CampoGeneral)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoCaracteristicas4).HasColumnName("IdTIpoCaracteristicas4");

                entity.Property(e => e.IdTipoNtipos1).HasColumnName("IdTipoNTipos1");

                entity.Property(e => e.IdTipoNtipos2).HasColumnName("IdTipoNTipos2");

                entity.Property(e => e.IdTipoNtipos3).HasColumnName("IdTipoNTipos3");

                entity.Property(e => e.IdTipoNtipos4).HasColumnName("IdTipoNTipos4");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.ModeloXcamposPredeterminadosDetalles)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_ModeloXCamposPredeterminadosDetalle_MODELOS");
            });

            modelBuilder.Entity<ModeloXoperacion>(entity =>
            {
                entity.HasKey(e => e.IdModeloXoperacion);

                entity.ToTable("ModeloXOperacion");

                entity.Property(e => e.IdModeloXoperacion).HasColumnName("IdModeloXOperacion");
            });

            modelBuilder.Entity<ModeloXoperacionXmaquinarium>(entity =>
            {
                entity.HasKey(e => e.IdModeloXoperacionXmaquinaria);

                entity.ToTable("ModeloXOperacionXMaquinaria");

                entity.Property(e => e.IdModeloXoperacionXmaquinaria).HasColumnName("IdModeloXOperacionXMaquinaria");
            });

            modelBuilder.Entity<MostrarOrdenCompraItemsGeneralLogistica>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MostrarOrdenCompraItemsGeneralLogistica");

                entity.Property(e => e.CDelProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("C. DEL PRODUCTO");

                entity.Property(e => e.CantidadTotal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CANTIDAD TOTAL");

                entity.Property(e => e.Código).HasColumnName("CÓDIGO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.Producto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO");

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("STOCK");

                entity.Property(e => e.TipoDeMedida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIPO DE MEDIDA");
            });

            modelBuilder.Entity<MostrarRequerimientoGeneralLosgistica>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MostrarRequerimientoGeneralLosgistica");

                entity.Property(e => e.CantidadMinima)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CANTIDAD MINIMA");

                entity.Property(e => e.Código)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO");

                entity.Property(e => e.CódigoInterno).HasColumnName("CÓDIGO INTERNO");

                entity.Property(e => e.CódigoLínea).HasColumnName("CÓDIGO LÍNEA");

                entity.Property(e => e.CódigoMercaderiaCuenta).HasColumnName("CÓDIGO MERCADERIA/CUENTA");

                entity.Property(e => e.CódigoModelo).HasColumnName("CÓDIGO MODELO");

                entity.Property(e => e.CódigoRequerimeinto).HasColumnName("CÓDIGO REQUERIMEINTO");

                entity.Property(e => e.Proceso).HasColumnName("PROCESO");

                entity.Property(e => e.Producto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO");

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("STOCK");

                entity.Property(e => e.TipoDeMedida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIPO DE MEDIDA");

                entity.Property(e => e.VCritico).HasColumnName("V_CRITICO");
            });

            modelBuilder.Entity<MostrarRequerimientoItemsGeneralLosgistica>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MostrarRequerimientoItemsGeneralLosgistica");

                entity.Property(e => e.CDelProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("C. DEL PRODUCTO");

                entity.Property(e => e.CantidadRetirada)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CANTIDAD RETIRADA");

                entity.Property(e => e.CantidadTotal)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CANTIDAD TOTAL");

                entity.Property(e => e.Código).HasColumnName("CÓDIGO");

                entity.Property(e => e.EstadoAtendido)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO ATENDIDO");

                entity.Property(e => e.Producto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTO");

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("STOCK");

                entity.Property(e => e.TipoDeMedida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIPO DE MEDIDA");
            });

            modelBuilder.Entity<Operacione>(entity =>
            {
                entity.HasKey(e => e.IdOperaciones);

                entity.ToTable("OPERACIONES");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdenCompra>(entity =>
            {
                entity.HasKey(e => e.IdOrdenCompra);

                entity.ToTable("OrdenCompra");

                entity.Property(e => e.Autorizado)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoCotizacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOrdenCompra)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoOc).HasColumnName("EstadoOC");

                entity.Property(e => e.FechaEstimada).HasColumnType("datetime");

                entity.Property(e => e.FechaOrdenCompra).HasColumnType("datetime");

                entity.Property(e => e.FechaRequerimeintoMasProximo).HasColumnType("datetime");

                entity.Property(e => e.FechaRequerimientoMasAntiguo).HasColumnType("datetime");

                entity.Property(e => e.FileCotizacion).IsUnicode(false);

                entity.Property(e => e.Flete)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Generado)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Igv)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IGV");

                entity.Property(e => e.MensajeAnulacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.SubTotal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdenProduccion>(entity =>
            {
                entity.HasKey(e => e.IdOrdenProduccion);

                entity.ToTable("OrdenProduccion");

                entity.Property(e => e.CodigoBss)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoBssSemi)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoCliente)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoClienteSemi)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOrdenProduccion)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProductoSemi)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoSis)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoSisSemi)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ColorSemi)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionProductoSemi)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoOc).HasColumnName("EstadoOC");

                entity.Property(e => e.EstadoOp).HasColumnName("EstadoOP");

                entity.Property(e => e.FechaEntrega).HasColumnType("datetime");

                entity.Property(e => e.FechaEntregaRepro1).HasColumnType("datetime");

                entity.Property(e => e.FechaEntregaRepro2).HasColumnType("datetime");

                entity.Property(e => e.FechaEntregaRepro3).HasColumnType("datetime");

                entity.Property(e => e.FechaIncial).HasColumnType("datetime");

                entity.Property(e => e.FechaProduccion).HasColumnType("datetime");

                entity.Property(e => e.LugarEntrega)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoProducto).IsUnicode(false);

                entity.Property(e => e.PlanoProductoSemi).IsUnicode(false);
            });

            modelBuilder.Entity<OrdenServicio>(entity =>
            {
                entity.HasKey(e => e.IdOrdenServicio);

                entity.ToTable("OrdenServicio");

                entity.Property(e => e.CodigoBss)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CodigoBSS");

                entity.Property(e => e.CodigoOrdenServicio)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoOs).HasColumnName("EstadoOS");

                entity.Property(e => e.FechaEmtrega).HasColumnType("datetime");

                entity.Property(e => e.FechaInicial).HasColumnType("datetime");

                entity.Property(e => e.IdGeneraUsuario).HasColumnName("IdGenera_Usuario");

                entity.Property(e => e.Obserbaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoProducto).IsUnicode(false);

                entity.Property(e => e.UsuarioGenera)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PausaActiva>(entity =>
            {
                entity.HasKey(e => e.IdPausaActiva);

                entity.ToTable("PausaActiva");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido);

                entity.ToTable("Pedido");

                entity.Property(e => e.CodigoPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DetallePedido).IsUnicode(false);

                entity.Property(e => e.Direccion).IsUnicode(false);

                entity.Property(e => e.Exonerado).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FechaEmision).HasColumnType("datetime");

                entity.Property(e => e.FechaEntrega).HasColumnType("datetime");

                entity.Property(e => e.Igv)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("IGV");

                entity.Property(e => e.Inafecta).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LugarEntrega).IsUnicode(false);

                entity.Property(e => e.MensajeAnulacion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.OrdenCompra)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Peso).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.RutaOrdenCompraPdf)
                    .IsUnicode(false)
                    .HasColumnName("RutaOrdenCompraPDF");

                entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalDescuento).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.ToTable("Perfil");

                entity.Property(e => e.Alias)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil1)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Perfil");
            });

            modelBuilder.Entity<PlanoProducto>(entity =>
            {
                entity.HasKey(e => e.IdPlano);

                entity.ToTable("PlanoProducto");

                entity.Property(e => e.Name)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NameReferences)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RealDoc)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlanoXproducto>(entity =>
            {
                entity.HasKey(e => e.IdPlanoXproducto);

                entity.ToTable("PlanoXProducto");

                entity.Property(e => e.IdPlanoXproducto).HasColumnName("IdPlanoXProducto");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.PlanoXproductos)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_PlanoXProducto_PRODUCTOS");

                entity.HasOne(d => d.IdPlanoNavigation)
                    .WithMany(p => p.PlanoXproductos)
                    .HasForeignKey(d => d.IdPlano)
                    .HasConstraintName("FK_PlanoXProducto_PlanoProducto");
            });

            modelBuilder.Entity<Prioridade>(entity =>
            {
                entity.HasKey(e => e.IdPrioridad);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProcesoSistema>(entity =>
            {
                entity.HasKey(e => e.IdProceso);

                entity.ToTable("ProcesoSistema");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdArt)
                    .HasName("PK_PRODUTOS");

                entity.ToTable("PRODUCTOS");

                entity.Property(e => e.CantidadMinima).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Codcom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoGenerado)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Detalle)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.IdMedida)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RutaImagen).IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VCritico).HasColumnName("V_Critico");

                entity.HasOne(d => d.IdDiferencialNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdDiferencial)
                    .HasConstraintName("FK_PRODUCTOS_DIFERENCIAL");

                entity.HasOne(d => d.IdLineaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdLinea)
                    .HasConstraintName("FK_PRODUCTOS_LINEAS");

                entity.HasOne(d => d.IdMedidaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMedida)
                    .HasConstraintName("FK_PRODUCTOS_MEDIDA");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdModelo)
                    .HasConstraintName("FK_PRODUCTOS_MODELOS");

                entity.HasOne(d => d.IdTipoMercaderiasNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdTipoMercaderias)
                    .HasConstraintName("FK_PRODUCTOS_TIPOMERCADERIAS");
            });

            modelBuilder.Entity<ProductoXcamposSeleccionadosDetalle>(entity =>
            {
                entity.HasKey(e => e.IdPoductoXcamposSeleccionadoDetalle);

                entity.ToTable("ProductoXCamposSeleccionadosDetalle");

                entity.Property(e => e.IdPoductoXcamposSeleccionadoDetalle).HasColumnName("IdPoductoXCamposSeleccionadoDetalle");

                entity.Property(e => e.CampoGeneral)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IdDescripcionNtipos1).HasColumnName("IdDescripcionNTipos1");

                entity.Property(e => e.IdDescripcionNtipos2).HasColumnName("IdDescripcionNTipos2");

                entity.Property(e => e.IdDescripcionNtipos3).HasColumnName("IdDescripcionNTipos3");

                entity.Property(e => e.IdDescripcionNtipos4).HasColumnName("IdDescripcionNTipos4");

                entity.Property(e => e.IdTipoNtipos1).HasColumnName("IdTipoNTipos1");

                entity.Property(e => e.IdTipoNtipos2).HasColumnName("IdTipoNTipos2");

                entity.Property(e => e.IdTipoNtipos3).HasColumnName("IdTipoNTipos3");

                entity.Property(e => e.IdTipoNtipos4).HasColumnName("IdTipoNTipos4");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.ProductoXcamposSeleccionadosDetalles)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_ProductoXCamposSeleccionadosDetalle_PRODUCTOS");
            });

            modelBuilder.Entity<ProductosXcamposSeleccionado>(entity =>
            {
                entity.HasKey(e => e.IdProductoXcamposSeleccionados);

                entity.ToTable("ProductosXCamposSeleccionados");

                entity.Property(e => e.IdProductoXcamposSeleccionados).HasColumnName("IdProductoXCamposSeleccionados");

                entity.Property(e => e.CampNtipos1).HasColumnName("CampNTipos1");

                entity.Property(e => e.CampNtipos2).HasColumnName("CampNTipos2");

                entity.HasOne(d => d.IdArtNavigation)
                    .WithMany(p => p.ProductosXcamposSeleccionados)
                    .HasForeignKey(d => d.IdArt)
                    .HasConstraintName("FK_ProductosXCamposSeleccionados_PRODUCTOS");
            });

            modelBuilder.Entity<Proveedore>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDistrito)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProvincia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Dni)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DNI");

                entity.Property(e => e.IdTipoCliente).HasColumnName("IdTipoCLiente");

                entity.Property(e => e.Ldolares).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Lsoles).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NombreProveedor)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Otros)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OTROS");

                entity.Property(e => e.PaginaWeb)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ruc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RUC");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReporteOp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_OP");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO MATERNO");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO PATERNO");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CantidadRealizada).HasColumnName("CANTIDAD REALIZADA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(1003)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COLOR");

                entity.Property(e => e.DescripciónDelProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN DEL PRODUCTO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.EstadoDeOc).HasColumnName("ESTADO DE OC");

                entity.Property(e => e.FechaDeEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE ENTREGA");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE INICIO");

                entity.Property(e => e.FechaProduccion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA PRODUCCION");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.NOp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OP");

                entity.Property(e => e.NPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("N°. PEDIDO");

                entity.Property(e => e.Oc)
                    .IsUnicode(false)
                    .HasColumnName("OC");

                entity.Property(e => e.Pl)
                    .IsUnicode(false)
                    .HasColumnName("PL");

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PRIMER NOMBRE");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO NOMBRE");

                entity.Property(e => e.Unidad)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");
            });

            modelBuilder.Entity<ReporteOpCalidad>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_OP_Calidad");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO MATERNO");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO PATERNO");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CantidadInspeccionada).HasColumnName("CANTIDAD INSPECCIONADA");

                entity.Property(e => e.CantidadRealizada).HasColumnName("CANTIDAD REALIZADA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(1003)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COLOR");

                entity.Property(e => e.DescripciónDelProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN DEL PRODUCTO");

                entity.Property(e => e.EstadoCalidad)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO CALIDAD");

                entity.Property(e => e.EstadoDeOc).HasColumnName("ESTADO DE OC");

                entity.Property(e => e.EstadoOp)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO OP");

                entity.Property(e => e.FechaDeEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE ENTREGA");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE INICIO");

                entity.Property(e => e.FechaProduccion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA PRODUCCION");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.NOp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OP");

                entity.Property(e => e.NPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("N°. PEDIDO");

                entity.Property(e => e.Oc)
                    .IsUnicode(false)
                    .HasColumnName("OC");

                entity.Property(e => e.Pl)
                    .IsUnicode(false)
                    .HasColumnName("PL");

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PRIMER NOMBRE");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO NOMBRE");

                entity.Property(e => e.Unidad)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");
            });

            modelBuilder.Entity<ReporteOt>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_OT");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CantidadRealizada).HasColumnName("CANTIDAD REALIZADA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(1003)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("COLOR");

                entity.Property(e => e.DescripciónDelSubProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN DEL SUB-PRODUCTO");

                entity.Property(e => e.Estado)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO");

                entity.Property(e => e.FechaDeEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE ENTREGA");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA DE INICIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NOp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OP");

                entity.Property(e => e.NOt)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("N°. OT");

                entity.Property(e => e.PlanoProducto)
                    .IsUnicode(false)
                    .HasColumnName("PLANO PRODUCTO");

                entity.Property(e => e.PlanoSemiproducido)
                    .IsUnicode(false)
                    .HasColumnName("PLANO SEMIPRODUCIDO");
            });

            modelBuilder.Entity<ReporteProductosMaterialesFormulacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_Productos_Materiales_Formulacion");

                entity.Property(e => e.Cantidad)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CANTIDAD");

                entity.Property(e => e.CodBss)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("COD. BSS");

                entity.Property(e => e.CodSistema)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COD. SISTEMA");

                entity.Property(e => e.Codformulacion)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CODFORMULACION");

                entity.Property(e => e.DescripciónProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN PRODUCTO");

                entity.Property(e => e.Idmaterialactividad).HasColumnName("IDMATERIALACTIVIDAD");

                entity.Property(e => e.Idofrmualcion).HasColumnName("IDOFRMUALCION");

                entity.Property(e => e.Idproduc).HasColumnName("IDPRODUC");

                entity.Property(e => e.Medida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEDIDA");
            });

            modelBuilder.Entity<ReporteProduto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_Produtos");

                entity.Property(e => e.CantidadMinima)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CANTIDAD MINIMA");

                entity.Property(e => e.Código)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO");

                entity.Property(e => e.CódigoBss)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CÓDIGO BSS");

                entity.Property(e => e.CódigoInterno).HasColumnName("CÓDIGO INTERNO");

                entity.Property(e => e.CódigoLínea).HasColumnName("CÓDIGO LÍNEA");

                entity.Property(e => e.CódigoMercaderiaCuenta).HasColumnName("CÓDIGO MERCADERIA/CUENTA");

                entity.Property(e => e.CódigoModelo).HasColumnName("CÓDIGO MODELO");

                entity.Property(e => e.Descripción)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN");

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.Línea)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("LÍNEA");

                entity.Property(e => e.Medida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEDIDA");

                entity.Property(e => e.Modelo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("MODELO");

                entity.Property(e => e.Proceso).HasColumnName("PROCESO");

                entity.Property(e => e.VCritico).HasColumnName("V_CRITICO");
            });

            modelBuilder.Entity<ReporteProdutosDetallePedido>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_Produtos_DetallePedido");

                entity.Property(e => e.CPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("C. PEDIDO");

                entity.Property(e => e.CantPedido).HasColumnName("CANT. PEDIDO");

                entity.Property(e => e.CantidadTotalItems).HasColumnName("CANTIDAD TOTAL ITEMS");

                entity.Property(e => e.CodigoCliente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoForm)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO FORM");

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescripciónProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN PRODUCTO");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA ENTREGA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idart).HasColumnName("IDART");

                entity.Property(e => e.Idpedido).HasColumnName("IDPEDIDO");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.MProducto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("M. PRODUCTO");

                entity.Property(e => e.NumeroItem).HasColumnName("NUMERO ITEM");

                entity.Property(e => e.PlProducto)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PL PRODUCTO");

                entity.Property(e => e.PlSemiProducido)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PL SEMI PRODUCIDO");
            });

            modelBuilder.Entity<ReporteProdutosDetallePedidoDashboard>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_Produtos_DetallePedido_Dashboard");

                entity.Property(e => e.CPedido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("C. PEDIDO");

                entity.Property(e => e.CantPedido).HasColumnName("CANT. PEDIDO");

                entity.Property(e => e.CantidadTotalItems).HasColumnName("CANTIDAD TOTAL ITEMS");

                entity.Property(e => e.CodigoCliente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoForm)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO FORM");

                entity.Property(e => e.CodigoProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescripciónProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN PRODUCTO");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("date")
                    .HasColumnName("FECHA ENTREGA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idart).HasColumnName("IDART");

                entity.Property(e => e.Idpedido).HasColumnName("IDPEDIDO");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.MProducto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("M. PRODUCTO");

                entity.Property(e => e.NumeroItem).HasColumnName("NUMERO ITEM");

                entity.Property(e => e.PlProducto)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PL PRODUCTO");

                entity.Property(e => e.PlSemiProducido)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PL SEMI PRODUCIDO");
            });

            modelBuilder.Entity<ReporteSemiProductosMaterialesFormulacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Reporte_SemiProductos_Materiales_Formulacion");

                entity.Property(e => e.Cantidad)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("CANTIDAD");

                entity.Property(e => e.CodBss)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("COD. BSS");

                entity.Property(e => e.CodSistema)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COD. SISTEMA");

                entity.Property(e => e.Codformulacion)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CODFORMULACION");

                entity.Property(e => e.DescripciónProducto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIÓN PRODUCTO");

                entity.Property(e => e.Idmaterialactividad).HasColumnName("IDMATERIALACTIVIDAD");

                entity.Property(e => e.Idofrmualcion).HasColumnName("IDOFRMUALCION");

                entity.Property(e => e.Idproduc).HasColumnName("IDPRODUC");

                entity.Property(e => e.Medida)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEDIDA");
            });

            modelBuilder.Entity<RequerimientoSimple>(entity =>
            {
                entity.HasKey(e => e.IdRequerimientoSimple);

                entity.ToTable("RequerimientoSimple");

                entity.Property(e => e.AliasCargoJefatura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoRequerimientoSimple)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.DesJefatura)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoOc).HasColumnName("EstadoOC");

                entity.Property(e => e.FechaRequerida).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitada).HasColumnType("datetime");

                entity.Property(e => e.IdOp).HasColumnName("IdOP");

                entity.Property(e => e.IdOt).HasColumnName("IdOT");

                entity.Property(e => e.MensajeAnulacion).IsUnicode(false);

                entity.Property(e => e.Obervaciones).IsUnicode(false);
            });

            modelBuilder.Entity<RequerimientoVentum>(entity =>
            {
                entity.HasKey(e => e.IdRequerimientoVenta);

                entity.Property(e => e.IdRequerimientoVenta).ValueGeneratedNever();

                entity.Property(e => e.AliasCargoComercial)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.AliasCargoJefatura)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaRequerimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaTermino).HasColumnType("datetime");

                entity.Property(e => e.ItinerarioViaje)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MensajeAnulado)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MensajeAtrasado)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MensajeFueraFecha)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MotivoVisita)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdJefaturaNavigation)
                    .WithMany(p => p.RequerimientoVentumIdJefaturaNavigations)
                    .HasForeignKey(d => d.IdJefatura)
                    .HasConstraintName("FK_RequerimientoVenta_Usuarios1");

                entity.HasOne(d => d.IdTipoMonedaNavigation)
                    .WithMany(p => p.RequerimientoVenta)
                    .HasForeignKey(d => d.IdTipoMoneda)
                    .HasConstraintName("FK_RequerimientoVenta_TipoMonedas");

                entity.HasOne(d => d.IdVehiculoNavigation)
                    .WithMany(p => p.RequerimientoVenta)
                    .HasForeignKey(d => d.IdVehiculo)
                    .HasConstraintName("FK_RequerimientoVenta_Vehiculos");

                entity.HasOne(d => d.IdVendedorNavigation)
                    .WithMany(p => p.RequerimientoVentumIdVendedorNavigations)
                    .HasForeignKey(d => d.IdVendedor)
                    .HasConstraintName("FK_RequerimientoVenta_Usuarios");
            });

            modelBuilder.Entity<Responsable>(entity =>
            {
                entity.HasKey(e => e.IdResponsable);

                entity.ToTable("Responsable");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalidaNoConforme>(entity =>
            {
                entity.HasKey(e => e.IdSnc)
                    .HasName("PK_Calidad_SalidaNoConforme");

                entity.ToTable("SalidaNoConforme");

                entity.Property(e => e.IdSnc).HasColumnName("IdSNC");

                entity.Property(e => e.CausaConformidad).IsUnicode(false);

                entity.Property(e => e.CkCorrecion).HasColumnName("ckCorrecion");

                entity.Property(e => e.CkLiberacion).HasColumnName("ckLiberacion");

                entity.Property(e => e.CkReclasificacion).HasColumnName("ckReclasificacion");

                entity.Property(e => e.CkReproceso).HasColumnName("ckReproceso");

                entity.Property(e => e.DescripcionAccionesTomadas).IsUnicode(false);

                entity.Property(e => e.DescripcionOtros)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionSnc)
                    .IsUnicode(false)
                    .HasColumnName("DescripcionSNC");

                entity.Property(e => e.FechaHallazgo).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroPro).HasColumnType("datetime");

                entity.Property(e => e.Finaliza).HasColumnType("datetime");

                entity.Property(e => e.IdOp).HasColumnName("IdOP");

                entity.Property(e => e.Imagen1).IsUnicode(false);

                entity.Property(e => e.Imagen2).IsUnicode(false);

                entity.Property(e => e.Imagen3).IsUnicode(false);

                entity.Property(e => e.Inicio).HasColumnType("datetime");

                entity.Property(e => e.OportunidadMejora).IsUnicode(false);

                entity.Property(e => e.SkDestruccion).HasColumnName("skDestruccion");

                entity.Property(e => e.SkOtros).HasColumnName("skOtros");

                entity.Property(e => e.SkRecuperacion).HasColumnName("skRecuperacion");
            });

            modelBuilder.Entity<Sede>(entity =>
            {
                entity.HasKey(e => e.IdSede);

                entity.ToTable("Sede");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SistemaMensajerium>(entity =>
            {
                entity.HasKey(e => e.IdSistemaNotificaciones);

                entity.Property(e => e.ArchivoAdjunto).IsUnicode(false);

                entity.Property(e => e.AsuntoMensaje)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");

                entity.Property(e => e.ImagenAdjunta).IsUnicode(false);

                entity.Property(e => e.MensajeMen).IsUnicode(false);

                entity.Property(e => e.TituloMensaje)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("STOCKs");

                entity.Property(e => e.Código).HasColumnName("CÓDIGO");

                entity.Property(e => e.Stock1)
                    .HasColumnType("decimal(38, 3)")
                    .HasColumnName("STOCK");
            });

            modelBuilder.Entity<TablaLicencia>(entity =>
            {
                entity.HasKey(e => e.IdLicencia);

                entity.Property(e => e.Anotaciones)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Maquina)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificador)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalAsignado)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Placa)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoAccion>(entity =>
            {
                entity.HasKey(e => e.IdTipoAccion);

                entity.ToTable("TipoAccion");

                entity.Property(e => e.Accion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoAlmacenEntradaSalidaAlmacen>(entity =>
            {
                entity.HasKey(e => e.IdTipoAlmacenEntrada)
                    .HasName("PK_TipoAlmacenEntradaAlmacen");

                entity.ToTable("TipoAlmacenEntradaSalidaAlmacen");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoCambio>(entity =>
            {
                entity.HasKey(e => e.IdTipoCambio);

                entity.ToTable("TipoCambio");

                entity.Property(e => e.FechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.Maquina)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCompra)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoVenta)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoCliente>(entity =>
            {
                entity.HasKey(e => e.IdTipoClientes);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoCuentum>(entity =>
            {
                entity.HasKey(e => e.IdTipoCuenta);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.HasKey(e => e.IdTipoDocumento);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoFormulacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoFormulacion);

                entity.ToTable("TipoFormulacion");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoGrupo>(entity =>
            {
                entity.HasKey(e => e.IdTipoGrupo);

                entity.ToTable("TipoGrupo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoMoneda>(entity =>
            {
                entity.HasKey(e => e.IdTipoMonedas);

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoMovimientosEntradaSalidaAlmacen>(entity =>
            {
                entity.HasKey(e => e.IdTipoMovimientoEntradaAlmacen)
                    .HasName("PK_TipoMovimientosEntradaAlmacen");

                entity.ToTable("TipoMovimientosEntradaSalidaAlmacen");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EntradaSalida)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoNotaIngreso>(entity =>
            {
                entity.HasKey(e => e.IdTipoNotaIngreso);

                entity.ToTable("TipoNotaIngreso");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoOperacionPro>(entity =>
            {
                entity.HasKey(e => e.IdTipoOperacionPro);

                entity.ToTable("TipoOperacionPro");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoOrdenCompra>(entity =>
            {
                entity.HasKey(e => e.IdTipoOrdenCompra);

                entity.ToTable("TipoOrdenCompra");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoProveedor>(entity =>
            {
                entity.HasKey(e => e.IdTipoProveedor);

                entity.ToTable("TipoProveedor");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoRequerimientoGeneral>(entity =>
            {
                entity.HasKey(e => e.IdTipoRequerimiento)
                    .HasName("PK_TipoRequerimiento");

                entity.ToTable("TipoRequerimientoGeneral");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoRetencion>(entity =>
            {
                entity.HasKey(e => e.IdTipoRetencion);

                entity.ToTable("TipoRetencion");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipomercaderia>(entity =>
            {
                entity.HasKey(e => e.IdTipoMercaderias);

                entity.ToTable("TIPOMERCADERIAS");

                entity.Property(e => e.Abreviatura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodSunet)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Desciripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tipooperacion>(entity =>
            {
                entity.HasKey(e => e.IdTipoOperacion);

                entity.ToTable("TIPOOPERACION");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposCaracteristica>(entity =>
            {
                entity.HasKey(e => e.IdTipoCaracteristicas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposDiametro>(entity =>
            {
                entity.HasKey(e => e.IdTipoDiametros);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposDiseñoAcabado>(entity =>
            {
                entity.HasKey(e => e.IdTipoDiseñoAcabado);

                entity.ToTable("TiposDiseñoAcabado");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposEspesore>(entity =>
            {
                entity.HasKey(e => e.IdTipoEspesores);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposForma>(entity =>
            {
                entity.HasKey(e => e.IdTipoFormas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposMedida>(entity =>
            {
                entity.HasKey(e => e.IdTipoMedidas);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposNtipo>(entity =>
            {
                entity.HasKey(e => e.IdTipoNtipos);

                entity.ToTable("TiposNTipos");

                entity.Property(e => e.IdTipoNtipos).HasColumnName("IdTipoNTipos");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposVariosO>(entity =>
            {
                entity.HasKey(e => e.IdTipoVariosO);

                entity.ToTable("TiposVariosO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Magnitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transferencium>(entity =>
            {
                entity.HasKey(e => e.IdTransferencia);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UbicacionDepartamento>(entity =>
            {
                entity.HasKey(e => e.IdDepartamento);

                entity.ToTable("UbicacionDepartamento");

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UbicacionDistrito>(entity =>
            {
                entity.HasKey(e => e.IdDistrito);

                entity.ToTable("UbicacionDistrito");

                entity.Property(e => e.CodigoDistrito)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProvincia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UbicacionPai>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UbicacionProvincium>(entity =>
            {
                entity.HasKey(e => e.IdProvincia);

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoProvincia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuarios);

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoParterno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Area).IsUnicode(false);

                entity.Property(e => e.Documento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Icono).HasColumnType("image");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreIcono).IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaFirma).IsUnicode(false);

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.IdVehiculo);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Zona>(entity =>
            {
                entity.HasKey(e => e.IdZona);

                entity.ToTable("Zona");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
