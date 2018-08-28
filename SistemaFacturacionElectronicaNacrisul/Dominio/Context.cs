using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Dominio.Clases_Dominio;
using System.IO;

namespace Dominio
{
    public class Context : DbContext
    {


        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Detalle> Detalles { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<IndicadorFacturacion> Indicadores { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<SubGrupo> SubGrupos { get; set; }
        public DbSet<DatosEmisor> Emisor { get; set; }
        public DbSet<Parametros> Parametros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CodigosEmisor> CodigosEmisores { get; set; }
        public DbSet<CodigoRetencionPercepcion> CodigosPercepcionRetencion { get; set; }
        public DbSet<SaldosCliente> SaldosClientes { get; set; }
        public DbSet<CabezalRecibo> Recibos { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<StockProducto> StockProductos { get; set; }

        //Constructor
        public Context() : base("FacturacionElectronicaNacrisulProduccion") {
            ConfigureForSqlServer();
        }

        public static void ConfigureForSqlServer()
        {

            // Set base connection string
            //const string baseConnectionString =
            //    "Data Source=localhost\\SQLEXPRESS;user id=sa;password=123;MultipleActiveResultSets=True";
            StreamReader objReader = new StreamReader("c:\\cfe.txt");
            String sLine = "";
            String texto = "";
            int cont = 1;
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    if (cont == 1)
                    {
                        texto = sLine;
                    }
                }
                cont++;
            }
            objReader.Close();
            String[] linea2 = texto.Split(new char[] { ':' });
            string baseConnectionString = linea2[1].Trim();
             
            Database.DefaultConnectionFactory = new System.Data.Entity.Infrastructure.SqlConnectionFactory(baseConnectionString);
        }

        public Context(String baseDatos) : base(baseDatos) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Documento>().HasMany(p => p.detalle).WithMany().Map(mc =>
            {
                mc.ToTable("DocumentoDetalle");
                mc.MapLeftKey("IdDocumento");
                mc.MapRightKey("IdDetalle");
            });

            modelBuilder.Entity<Documento>().HasMany(p => p.documentosAsociados).WithMany().Map(mc =>
            {
                mc.ToTable("NCDocumento");
                mc.MapLeftKey("IdDocumento");
                mc.MapRightKey("IdDocumentoAsociado");
            });
        }

        public class Initializer : IDatabaseInitializer<Context>
        {
            public void InitializeDatabase(Context context)
            {
                //if (!context.Database.Exists())
                //{
                    context.Database.Delete();
                    context.Database.Create();

                                    //}
            }
        }

        
    }
}

