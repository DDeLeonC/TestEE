using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Dominio.Clases_Dominio;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Data.Objects.SqlClient;
using System.IO;

namespace Dominio
{
    partial class Sistema
    {
        #region Cliente

        public String GuardarCliente(Cliente Cliente)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Cliente cli = baseDatos.Clientes.FirstOrDefault(de => de.nroDoc.Trim().ToUpper().Equals(Cliente.nroDoc.Trim().ToUpper()));

                    if (cli != null && !Cliente.tipoDocumento.Equals("RUT"))
                    {
                        return "Error: Existe un cliente con el mismo número de documento";
                    }
                    else
                    {
                        Cliente.Activo = true;
                        Cliente.Pais = baseDatos.Paises.FirstOrDefault(pa=> pa.IdPais == Cliente.IdPais);
                        Cliente.Vendedor = baseDatos.Vendedores.FirstOrDefault(pa => pa.IdVendedor == Cliente.IdVendedor);
                        Cliente.Zona = baseDatos.Zonas.FirstOrDefault(pa => pa.IdZona == Cliente.IdZona);
                        baseDatos.Clientes.Add(Cliente);
                        baseDatos.SaveChanges();
                        Cliente clie = baseDatos.Clientes.FirstOrDefault(de => de.nroDoc.Trim().ToUpper().Equals(Cliente.nroDoc.Trim().ToUpper()));
                        if (clie != null) {
                            SaldosCliente saldo = new SaldosCliente();
                            saldo.Año = DateTime.Now.Year;
                            saldo.Cliente = clie;
                            saldo.IdCliente = clie.IdCliente;
                            saldo.Mes = DateTime.Now.Month;
                            saldo.Debe = 0;
                            saldo.Haber = 0;
                            baseDatos.SaldosClientes.Add(saldo);
                        }
                        return "El cliente se guardó correctamente";
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return "Error al guardar el cliente";
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Clientes != null)
                    {
                        baseDatos.Clientes.Remove(Cliente);
                    }
                    return "Error al guardar el cliente";
                }
            }
        }

        public List<Cliente> ObtenerClientes()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Clientes.Include("Pais").Where(ej => ej.Activo == true).OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarCliente(Cliente Cliente)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Cliente cli = baseDatos.Clientes.FirstOrDefault(cl => cl.IdCliente == Cliente.IdCliente);
                    if (cli != null)
                    {
                        Cliente cli2 = baseDatos.Clientes.FirstOrDefault(de => de.nroDoc.Trim().ToUpper().Equals(Cliente.nroDoc.Trim().ToUpper()) && de.IdCliente != Cliente.IdCliente);
                        if (cli2 != null && !Cliente.tipoDocumento.Equals("RUT"))
                        {
                            return "Error: Existe un cliente con el mismo documento";
                        }
                        else
                        {

                            cli.Ciudad = Cliente.Ciudad;
                            cli.CodigoPostal = Cliente.CodigoPostal;
                            cli.Direccion = Cliente.Direccion;
                            cli.Nombre = Cliente.Nombre;
                            cli.nroDoc = Cliente.nroDoc;
                            cli.tipoDocumento = Cliente.tipoDocumento;
                            cli.IdPais = Cliente.IdPais;
                            cli.IdVendedor = Cliente.IdVendedor;
                            cli.IdZona = Cliente.IdZona;
                            cli.Mail = Cliente.Mail;
                            cli.Tel = Cliente.Tel;
                            cli.Pais = baseDatos.Paises.FirstOrDefault(pa => pa.IdPais == cli.IdPais);
                            cli.Vendedor = baseDatos.Vendedores.FirstOrDefault(pa => pa.IdVendedor == cli.IdVendedor);
                            cli.Zona = baseDatos.Zonas.FirstOrDefault(pa => pa.IdZona == cli.IdZona);
                            baseDatos.SaveChanges();
                            return "El cliente se modificó correctamente";


                        }


                    }
                    else
                    {
                        return "Error al modificar el cliente";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el cliente";
            }
        }

        public String EliminarCliente(Cliente Cliente)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Cliente cli = baseDatos.Clientes.FirstOrDefault(de => de.IdCliente == Cliente.IdCliente);
                    if (cli != null)
                    {
                        cli.Activo = false;
                        baseDatos.SaveChanges();
                        return "El cliente se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el cliente";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el cliente";
            }
        }

        public List<Cliente> BuscarClientes(String Nombre, bool activo, String rut, int? idVendedor)
        {
            
            List<Cliente> result = new List<Cliente>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Clientes.Include("Pais").Include("Zona").Include("Vendedor").Where(pro => pro.IdCliente > 0);
                    query = query.Include("Pais").Include("Zona").Include("Vendedor").Where(prop => prop.Activo == activo && prop.rut.Equals(rut));

                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Include("Pais").Include("Zona").Include("Vendedor").Where(fun => fun.Nombre.ToUpper().StartsWith(Nombre.ToUpper()));
                    }
                    if (idVendedor != null)
                    {
                        query = query.Include("Pais").Include("Zona").Include("Vendedor").Where(doc => doc.IdVendedor == idVendedor);
                    }


                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Cliente BuscarClienteId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Clientes.Include("Pais").Include("Zona").Include("Vendedor").FirstOrDefault(prop => prop.IdCliente == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region Grupo

        public String GuardarGrupo(Grupo Grupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Grupo gru = baseDatos.Grupos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Grupo.Codigo.Trim().ToUpper()));

                    if (gru != null)
                    {
                        return "Error: Existe un grupo con el mismo código";
                    }
                    else
                    {
                        Grupo gru2 = baseDatos.Grupos.FirstOrDefault(de => de.Descripcion.Trim().ToUpper().Equals(Grupo.Descripcion.Trim().ToUpper()));
                        if (gru2 != null)
                        {
                            return "Error: Existe un grupo con la misma descripción";
                        }
                        else
                        {
                            Grupo.Activo = true;
                            baseDatos.Grupos.Add(Grupo);
                            baseDatos.SaveChanges();
                            return "El grupo se guardó correctamente";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Grupos != null)
                    {
                        baseDatos.Grupos.Remove(Grupo);
                    }
                    return "Error al guardar el grupo";
                }
            }
        }

        public List<Grupo> ObtenerGrupos(String rut)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Grupos.Where(ej => ej.Activo == true && ej.rut.Equals(rut)).OrderBy(ej => ej.Descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarGrupo(Grupo Grupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Grupo gru = baseDatos.Grupos.FirstOrDefault(cl => cl.IdGrupo == Grupo.IdGrupo);
                    if (gru != null)
                    {
                        Grupo gru2 = baseDatos.Grupos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Grupo.Codigo.Trim().ToUpper()) && de.IdGrupo != Grupo.IdGrupo);
                        if (gru2 != null)
                        {
                            return "Error: Existe un grupo con el mismo código";
                        }
                        else
                        {
                            Grupo gru3 = baseDatos.Grupos.FirstOrDefault(de => de.Descripcion.Trim().ToUpper().Equals(Grupo.Descripcion.Trim().ToUpper()) && de.IdGrupo != Grupo.IdGrupo);
                            if (gru3 != null)
                            {
                                return "Error: Existe un grupo con la misma descripción";
                            }
                            else
                            {
                                gru.Codigo = Grupo.Codigo;
                                gru.Descripcion = Grupo.Descripcion;

                                baseDatos.SaveChanges();
                                return "El grupo se modificó correctamente";
                            }

                        }


                    }
                    else
                    {
                        return "Error al modificar el grupo";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el grupo";
            }
        }

        public String EliminarGrupo(Grupo Grupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Grupo gru = baseDatos.Grupos.FirstOrDefault(de => de.IdGrupo == Grupo.IdGrupo);
                    if (gru != null)
                    {
                        gru.Activo = false;
                        baseDatos.SaveChanges();
                        return "El grupo se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el grupo";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el grupo";
            }
        }

        public List<Grupo> BuscarGrupos(String Nombre, String codigo, bool activo, String rut)
        {
            List<Grupo> result = new List<Grupo>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Grupos.Where(pro => pro.IdGrupo > 0);
                    query = query.Where(prop => prop.Activo == activo && prop.rut.Equals(rut));

                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Where(fun => fun.Descripcion.ToUpper().StartsWith(Nombre.ToUpper()));
                    }

                    if (!String.IsNullOrEmpty(codigo))
                    {
                        query = query.Where(fun => fun.Codigo.ToUpper().StartsWith(codigo.ToUpper()));
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Grupo BuscarGrupoId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Grupos.FirstOrDefault(prop => prop.IdGrupo == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region SubGrupo

        public String GuardarSubGrupo(SubGrupo SubGrupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    SubGrupo gru = baseDatos.SubGrupos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(SubGrupo.Codigo.Trim().ToUpper()));

                    if (gru != null)
                    {
                        return "Error: Existe un SubGrupo con el mismo código";
                    }
                    else
                    {
                        SubGrupo gru2 = baseDatos.SubGrupos.FirstOrDefault(de => de.Descripcion.Trim().ToUpper().Equals(SubGrupo.Descripcion.Trim().ToUpper()));
                        if (gru2 != null)
                        {
                            return "Error: Existe un SubGrupo con la misma descripción";
                        }
                        else
                        {
                            SubGrupo.Activo = true;
                            SubGrupo.Grupo = baseDatos.Grupos.FirstOrDefault(pa => pa.IdGrupo == SubGrupo.IdGrupo);
                            baseDatos.SubGrupos.Add(SubGrupo);
                            baseDatos.SaveChanges();
                            return "El SubGrupo se guardó correctamente";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.SubGrupos != null)
                    {
                        baseDatos.SubGrupos.Remove(SubGrupo);
                    }
                    return "Error al guardar el SubGrupo";
                }
            }
        }

        public List<SubGrupo> ObtenerSubGrupos(String rut)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.SubGrupos.Include("Grupo").Where(ej => ej.Activo == true && ej.rut.Equals(rut)).OrderBy(ej => ej.Descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarSubGrupo(SubGrupo SubGrupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    SubGrupo gru = baseDatos.SubGrupos.FirstOrDefault(cl => cl.IdSubGrupo == SubGrupo.IdSubGrupo);
                    if (gru != null)
                    {
                        SubGrupo gru2 = baseDatos.SubGrupos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(SubGrupo.Codigo.Trim().ToUpper()) && de.IdSubGrupo != SubGrupo.IdSubGrupo);
                        if (gru2 != null)
                        {
                            return "Error: Existe un Subgrupo con el mismo código";
                        }
                        else
                        {
                            SubGrupo gru3 = baseDatos.SubGrupos.FirstOrDefault(de => de.Descripcion.Trim().ToUpper().Equals(SubGrupo.Descripcion.Trim().ToUpper()) && de.IdSubGrupo != SubGrupo.IdSubGrupo);
                            if (gru3 != null)
                            {
                                return "Error: Existe un Subgrupo con la misma descripción";
                            }
                            else
                            {
                                gru.Codigo = SubGrupo.Codigo;
                                gru.Descripcion = SubGrupo.Descripcion;
                                gru.IdGrupo = SubGrupo.IdGrupo;
                                gru.Grupo = baseDatos.Grupos.FirstOrDefault(pa => pa.IdGrupo == SubGrupo.IdGrupo);
                                baseDatos.SaveChanges();
                                return "El Subgrupo se modificó correctamente";
                            }

                        }


                    }
                    else
                    {
                        return "Error al modificar el Subgrupo";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el Subgrupo";
            }
        }

        public String EliminarSubGrupo(SubGrupo SubGrupo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    SubGrupo gru = baseDatos.SubGrupos.FirstOrDefault(de => de.IdSubGrupo == SubGrupo.IdSubGrupo);
                    if (gru != null)
                    {
                        gru.Activo = false;
                        baseDatos.SaveChanges();
                        return "El Subgrupo se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el Subgrupo";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el Subgrupo";
            }
        }

        public SubGrupo BuscarSubGrupoId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.SubGrupos.Include("Grupo").FirstOrDefault(prop => prop.IdSubGrupo == id);

                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public List<SubGrupo> BuscarSubGrupos(String Nombre, String codigo, int? idGrupo, bool activo, String rut)
        {
            List<SubGrupo> result = new List<SubGrupo>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.SubGrupos.Include("Grupo").Where(pro => pro.IdSubGrupo > 0);
                    query = query.Include("Grupo").Where(prop => prop.Activo == activo && prop.rut.Equals(rut));

                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Include("Grupo").Where(fun => fun.Descripcion.ToUpper().StartsWith(Nombre.ToUpper()));
                    }
                    if (idGrupo != null)
                    {
                        query = query.Include("Grupo").Where(fun => fun.IdGrupo == idGrupo);
                    }

                    if (!String.IsNullOrEmpty(codigo))
                    {
                        query = query.Include("Grupo").Where(fun => fun.Codigo.ToUpper().StartsWith(codigo.ToUpper()));
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<SubGrupo> ObtenerSubGruposGrupo(int idGrupo, String rut)
        {
            using (var baseDatos = new Context())
            {
                List<SubGrupo> result = new List<SubGrupo>();
                result = baseDatos.SubGrupos.Include("Grupo").Where(fun => fun.IdGrupo == idGrupo && fun.rut.Equals(rut)).ToList();
                return result;
            }
        }
        #endregion

        #region Producto

        public String GuardarProducto(Producto Producto)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Producto pro = baseDatos.Productos.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Producto.Nombre.Trim().ToUpper()));

                    if (pro != null)
                    {
                        return "Error: Existe un producto con el mismo nombre";
                    }
                    else
                    {
                        Producto pro2 = baseDatos.Productos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Producto.Codigo.Trim().ToUpper()));

                        if (pro2 != null)
                        {
                            return "Error: Existe un producto con el mismo codigo";
                        }
                        else
                        {
                            Producto.Activo = true;
                            Producto.SubGrupo = baseDatos.SubGrupos.FirstOrDefault(pa => pa.IdSubGrupo == Producto.IdSubGrupo);
                            Producto.indicador = baseDatos.Indicadores.FirstOrDefault(pa => pa.IdIndicador == Producto.IdIndicador);
                            baseDatos.Productos.Add(Producto);
                            baseDatos.SaveChanges();


                            //CREO STOCK
                            Producto prod3 = baseDatos.Productos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Producto.Codigo.Trim().ToUpper()));
                            if (prod3 != null) {
                                StockProducto stock = new StockProducto();
                                stock.Producto = prod3;
                                stock.Cantidad = 0;
                                stock.IdProducto = prod3.IdProducto;
                                stock.Kilos = 0;
                                
                            }
                            return "El producto se guardó correctamente";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Productos != null)
                    {
                        baseDatos.Productos.Remove(Producto);
                    }
                    return "Error al guardar el producto";
                }
            }
        }

        public List<Producto> ObtenerProductos(String rut)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Productos.Include("indicador").Where(ej => ej.Activo == true && ej.rut.Equals(rut)).OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarProducto(Producto Producto)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Producto pro = baseDatos.Productos.FirstOrDefault(cl => cl.IdProducto == Producto.IdProducto);
                    if (pro != null)
                    {
                        Producto pro2 = baseDatos.Productos.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Producto.Nombre.Trim().ToUpper()) && de.IdProducto != Producto.IdProducto);
                        if (pro2 != null)
                        {
                            return "Error: Existe un producto con el mismo nombre";
                        }
                        else
                        {
                            Producto pro3 = baseDatos.Productos.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Producto.Codigo.Trim().ToUpper()) && de.IdProducto != Producto.IdProducto);
                            if (pro2 != null)
                            {
                                return "Error: Existe un producto con el mismo codigo";
                            }
                            else
                            {
                                pro.Codigo = Producto.Codigo;
                                pro.Descripcion = Producto.Descripcion;
                                pro.IdIndicador = Producto.IdIndicador;
                                pro.IdSubGrupo = Producto.IdSubGrupo;
                                pro.Nombre = Producto.Nombre;
                                pro.Precio = Producto.Precio;
                                pro.unidadMedida = Producto.unidadMedida;
                                pro.SubGrupo = baseDatos.SubGrupos.FirstOrDefault(pa => pa.IdSubGrupo == Producto.IdSubGrupo);
                                pro.indicador = baseDatos.Indicadores.FirstOrDefault(pa => pa.IdIndicador == Producto.IdIndicador);
                                baseDatos.SaveChanges();
                                return "El producto se modificó correctamente";
                            }

                        }


                    }
                    else
                    {
                        return "Error al modificar el producto";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el producto";
            }
        }

        public String EliminarProducto(Producto Producto)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Producto pro = baseDatos.Productos.FirstOrDefault(de => de.IdProducto == Producto.IdProducto);
                    if (pro != null)
                    {
                        pro.Activo = false;
                        baseDatos.SaveChanges();
                        return "El producto se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el producto";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el producto";
            }
        }

        public List<Producto> BuscarProductos(String Nombre, String codigo, int? idSubGrupo, bool activo, String rut)
        {
            List<Producto> result = new List<Producto>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Productos.Include("SubGrupo").Include("indicador").Where(pro => pro.IdProducto > 0);
                    query = query.Include("SubGrupo").Include("indicador").Where(prop => prop.Activo == activo && prop.rut.Equals(rut));

                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.Nombre.ToUpper().StartsWith(Nombre.ToUpper()));
                    }
                    if (!String.IsNullOrEmpty(codigo))
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.Codigo.ToUpper().StartsWith(codigo.ToUpper()));
                    }
                    if (idSubGrupo != null)
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.IdSubGrupo == idSubGrupo);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Producto> SearchProducts(String texto, bool activo, String rut, int IdSubGrupo, int IdGrupo)
        {
            List<Producto> result = new List<Producto>();


            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Productos.Include("SubGrupo").Include("indicador").Where(pro => pro.IdProducto > 0);
                    query = query.Include("SubGrupo").Include("indicador").Where(prop => prop.Activo == activo && prop.rut.Equals(rut));

                    if (!String.IsNullOrEmpty(texto))
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.Nombre.ToUpper().StartsWith(texto.ToUpper()) || fun.Nombre.ToUpper().Contains(texto.ToUpper()) || fun.Codigo.Equals(texto));
                    }
                    if (IdSubGrupo != 0)
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.IdSubGrupo == IdSubGrupo);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Producto> BuscarProductosSubGrupo(int idSubGrupo, String rut)
        {
            List<Producto> result = new List<Producto>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Productos.Include("SubGrupo").Include("indicador").Where(pro => pro.IdProducto > 0);
                    query = query.Include("SubGrupo").Include("indicador").Where(prop => prop.Activo == true && prop.rut.Equals(rut));

                    if (idSubGrupo != null)
                    {
                        query = query.Include("SubGrupo").Include("indicador").Where(fun => fun.IdSubGrupo == idSubGrupo);
                    }


                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Producto BuscarProductoId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Productos.Include("indicador").Include("SubGrupo").Include("SubGrupo.Grupo").FirstOrDefault(prop => prop.IdProducto == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region Vendedor

        public String GuardarVendedor(Vendedor Vendedor)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Vendedor vend = baseDatos.Vendedores.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Vendedor.Codigo.Trim().ToUpper()));

                    if (vend != null)
                    {
                        return "Error: Existe un vendedor con el mismo código";
                    }
                    else
                    {
                        Vendedor vend2 = baseDatos.Vendedores.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Vendedor.Nombre.Trim().ToUpper()));
                        if (vend2 != null)
                        {
                            return "Error: Existe un vendedor con el mismo nombre";
                        }
                        else
                        {
                            Vendedor.Activo = true;
                            baseDatos.Vendedores.Add(Vendedor);
                            baseDatos.SaveChanges();
                            return "El vendedor se guardó correctamente";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Vendedores != null)
                    {
                        baseDatos.Vendedores.Remove(Vendedor);
                    }
                    return "Error al guardar el vendedor";
                }
            }
        }

        public List<Vendedor> ObtenerVendedores()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Vendedores.Where(ej => ej.Activo == true ).OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarVendedor(Vendedor Vendedor)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Vendedor ven = baseDatos.Vendedores.FirstOrDefault(cl => cl.IdVendedor == Vendedor.IdVendedor);
                    if (ven != null)
                    {
                        Vendedor ven2 = baseDatos.Vendedores.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Vendedor.Codigo.Trim().ToUpper()) && de.IdVendedor != Vendedor.IdVendedor);
                        if (ven2 != null)
                        {
                            return "Error: Existe un vendedor con el mismo código";
                        }
                        else
                        {
                            Vendedor ven3 = baseDatos.Vendedores.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Vendedor.Nombre.Trim().ToUpper()) && de.IdVendedor != Vendedor.IdVendedor);
                            if (ven3 != null)
                            {
                                return "Error: Existe un vendedor con el mismo nombre";
                            }
                            else
                            {
                                ven.Codigo = Vendedor.Codigo;
                                ven.Nombre = Vendedor.Nombre;

                                baseDatos.SaveChanges();
                                return "El vendedor se modificó correctamente";
                            }

                        }


                    }
                    else
                    {
                        return "Error al modificar el vendedor";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el vendedor";
            }
        }

        public String EliminarVendedor(Vendedor Vendedor)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Vendedor ven = baseDatos.Vendedores.FirstOrDefault(de => de.IdVendedor == Vendedor.IdVendedor);
                    if (ven != null)
                    {
                        ven.Activo = false;
                        baseDatos.SaveChanges();
                        return "El vendedor se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el vendedor";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el vendedor";
            }
        }

        public List<Vendedor> BuscarVendedores(String Nombre, String codigo, bool activo)
        {
            List<Vendedor> result = new List<Vendedor>();


            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Vendedores.Where(pro => pro.IdVendedor > 0);
                    
                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Where(fun => fun.Nombre.ToUpper().StartsWith(Nombre.ToUpper()));
                    }

                    if (!String.IsNullOrEmpty(codigo))
                    {
                        query = query.Where(fun => fun.Codigo.ToUpper().StartsWith(codigo.ToUpper()));
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Vendedor BuscarVendedorId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Vendedores.FirstOrDefault(prop => prop.IdVendedor == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region Zona

        public String GuardarZona(Zona Zona)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Zona zon = baseDatos.Zonas.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Zona.Codigo.Trim().ToUpper()));

                    if (zon != null)
                    {
                        return "Error: Existe una zona con el mismo código";
                    }
                    else
                    {
                        Zona zon2 = baseDatos.Zonas.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Zona.Nombre.Trim().ToUpper()));
                        if (zon2 != null)
                        {
                            return "Error: Existe una zona con el mismo nombre";
                        }
                        else
                        {
                            Zona.Activo = true;
                            baseDatos.Zonas.Add(Zona);
                            baseDatos.SaveChanges();
                            return "La zona se guardó correctamente";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Zonas != null)
                    {
                        baseDatos.Zonas.Remove(Zona);
                    }
                    return "Error al guardar la zona";
                }
            }
        }

        public List<Zona> ObtenerZonas()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Zonas.Where(ej => ej.Activo == true).OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarZona(Zona Zona)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Zona ven = baseDatos.Zonas.FirstOrDefault(cl => cl.IdZona == Zona.IdZona);
                    if (ven != null)
                    {
                        Zona ven2 = baseDatos.Zonas.FirstOrDefault(de => de.Codigo.Trim().ToUpper().Equals(Zona.Codigo.Trim().ToUpper()) && de.IdZona != Zona.IdZona);
                        if (ven2 != null)
                        {
                            return "Error: Existe una zona con el mismo código";
                        }
                        else
                        {
                            Zona ven3 = baseDatos.Zonas.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Zona.Nombre.Trim().ToUpper()) && de.IdZona != Zona.IdZona);
                            if (ven3 != null)
                            {
                                return "Error: Existe una zona con el mismo nombre";
                            }
                            else
                            {
                                ven.Codigo = Zona.Codigo;
                                ven.Nombre = Zona.Nombre;

                                baseDatos.SaveChanges();
                                return "La zona se modificó correctamente";
                            }

                        }


                    }
                    else
                    {
                        return "Error al modificar la zona";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar la zona";
            }
        }

        public String EliminarZona(Zona Zona)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Zona ven = baseDatos.Zonas.FirstOrDefault(de => de.IdZona == Zona.IdZona);
                    if (ven != null)
                    {
                        ven.Activo = false;
                        baseDatos.SaveChanges();
                        return "La zona se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar la zona";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar la zona";
            }
        }

        public List<Zona> BuscarZonas(String Nombre, String codigo, bool activo)
        {
            List<Zona> result = new List<Zona>();


            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Zonas.Where(pro => pro.IdZona > 0);

                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Where(fun => fun.Nombre.ToUpper().StartsWith(Nombre.ToUpper()));
                    }

                    if (!String.IsNullOrEmpty(codigo))
                    {
                        query = query.Where(fun => fun.Codigo.ToUpper().StartsWith(codigo.ToUpper()));
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Zona BuscarZonaId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Zonas.FirstOrDefault(prop => prop.IdZona == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region Pais

        public List<Pais> ObtenerPaises()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Paises.OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Pais ObtenerPaisId(int id)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Paises.FirstOrDefault(pa => pa.IdPais == id);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Indicador

        public List<IndicadorFacturacion> ObtenerIndicadores()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Indicadores.OrderBy(ej => ej.Codigo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        #endregion

        #region Documentos

        public String GuardarDocumento(Documento documento)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    documento.Activo = true;
                    documento.cliente = baseDatos.Clientes.FirstOrDefault(pa => pa.IdCliente == documento.IdCliente);
                    documento.Usuario = baseDatos.Usuarios.FirstOrDefault(pa => pa.IdUsuario == documento.IdUsuario);
                    if (documento.detalle != null) {
                        foreach (Detalle det in documento.detalle) {
                            det.producto = baseDatos.Productos.Include("indicador").Include("SubGrupo").Include("SubGrupo.Grupo").FirstOrDefault(prop => prop.IdProducto == det.IdProducto);
                        }
                    }
                    if (documento.FechaVencimiento.Year == 1) {
                        documento.FechaVencimiento = DateTime.Now;
                    }
                    documento.MontoPagado = 0;
                    if (documento.documentosAsociados != null) {
                        List<Documento> docsAso = new List<Documento>();
                        foreach (Documento docu in documento.documentosAsociados) {
                            Documento docum = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == docu.IdDocumento);
                            docsAso.Add(docum);
                        }
                        documento.documentosAsociados = docsAso;
                    }   
                    baseDatos.Documentos.Add(documento);
                    //Si es nota de credito, suma en los documentos a los que afecta la nota de credito el monto pagado y actualiza el estado
                    if ((documento.TipoDocumento.Equals("102") || documento.TipoDocumento.Equals("112") || documento.TipoDocumento.Equals("122"))&& documento.documentosAsociados!=null && documento.documentosAsociados.Count>0 ) {
                        int cant = documento.documentosAsociados.Count;
                        decimal saldo = documento.Total / cant;
                        bool pagos = true;
                        foreach (Documento doc in documento.documentosAsociados) {
                            Documento docu = baseDatos.Documentos.FirstOrDefault(d=>d.IdDocumento==doc.IdDocumento);
                            docu.MontoPagado += saldo;
                            if (!docu.EstadoCredito.Equals("PAGO")) {
                                pagos = false;
                            }
                            if (docu.Total == docu.MontoPagado) {
                                docu.EstadoCredito = "PAGO";
                            }
                        }
                        if (!pagos)
                        {
                            documento.EstadoCredito = "PAGO";
                        }
                        else {
                            documento.EstadoCredito = "DEBE";
                        }
                        
                    }

                    baseDatos.SaveChanges();
                    
                    AumentarCodigo(documento.rut);
                    //Actualizo saldos clientes
                    if (documento.IdCliente != 0) {

                        if ((documento.TipoDocumento.Equals("102") || documento.TipoDocumento.Equals("112") || documento.TipoDocumento.Equals("122"))) {
                            SaldosCliente saldoActual = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == documento.IdCliente && sal.Mes == documento.Fecha.Month && sal.Año == documento.Fecha.Year);
                            if (saldoActual != null) {
                                saldoActual.Haber += documento.Total;
                            }
                        }
                        else if ((documento.TipoDocumento.Equals("103") || documento.TipoDocumento.Equals("113") || documento.TipoDocumento.Equals("123"))) {
                            SaldosCliente saldoActual = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == documento.IdCliente && sal.Mes == documento.Fecha.Month && sal.Año == documento.Fecha.Year);
                            if (saldoActual != null)
                            {
                                saldoActual.Debe += documento.Total;
                            }
                        }
                        else if ((documento.TipoDocumento.Equals("101") || documento.TipoDocumento.Equals("111") || documento.TipoDocumento.Equals("121")))
                        {
                            SaldosCliente saldoActual = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == documento.IdCliente && sal.Mes == documento.Fecha.Month && sal.Año == documento.Fecha.Year);
                            if (saldoActual != null)
                            {
                                saldoActual.Debe += documento.Total;
                            }
                        }
                        baseDatos.SaveChanges();
                    }
                    return "El documento se guardó correctamente";

                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return "Error al guardar el documento";
            }
        }

        public List<Documento> ObtenerDocumentosCliente(int idCliente, String tipoDoc,String rut)
        {
            List<Documento> result = new List<Documento>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Where(pro => pro.IdDocumento > 0 && pro.rut.Equals(rut)).OrderByDescending(p => p.Fecha);
                    query = query.Where(prop => prop.Activo == true && prop.EstadoDGI.Equals("Procesado") || prop.EstadoDGI.Equals("Aceptado")).OrderByDescending(p => p.Fecha);

                    if (!String.IsNullOrEmpty(tipoDoc))
                    {
                        if (tipoDoc.Equals("Factura"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("101") || fun.TipoDocumento.Equals("111")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Nota Débito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("103") || fun.TipoDocumento.Equals("113")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Remito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("181")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Resguardo"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("182")).OrderByDescending(p => p.Fecha);
                        }
                    }

                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderByDescending(p => p.Fecha);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Documento> ObtenerDocumentosAceptados(int? idCliente, String tipoDoc,DateTime FechaDesde, DateTime FechaHasta, String Serie, int nro,String rut)
        {
            List<Documento> result = new List<Documento>();
            
            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Where(pro => pro.IdDocumento > 0 && pro.rut.Equals(rut)).OrderByDescending(p => p.Fecha);

                    query = query.Where(prop => prop.Activo == true && prop.EstadoDGI.Equals("Aceptado")).OrderByDescending(p => p.Fecha);

                    if (!String.IsNullOrEmpty(tipoDoc))
                    {
                        if (tipoDoc.Equals("Factura"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("101") || fun.TipoDocumento.Equals("111")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Nota Débito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("103") || fun.TipoDocumento.Equals("113")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Remito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("181")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Resguardo"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("182")).OrderByDescending(p => p.Fecha);
                        }
                    }
                    query = query.Where(fun => fun.Fecha >= FechaDesde && fun.Fecha <= FechaHasta).OrderByDescending(p => p.Fecha);
                    if (!String.IsNullOrEmpty(Serie))
                    {
                        query = query.Where(fun => fun.Serie.ToUpper().Equals(Serie.ToUpper())).OrderByDescending(p => p.Fecha);
                    }
                    if (nro != 0)
                    {
                        query = query.Where(fun => fun.NroSerie == nro).OrderByDescending(p => p.Fecha);
                    }
                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderByDescending(p => p.Fecha);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Documento> ObtenerDocumentosAceptados(String rut)
        {
            List<Documento> result = new List<Documento>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Where(pro => pro.IdDocumento > 0 && pro.rut.Equals(rut)).OrderByDescending(p => p.Fecha);
                    query = query.Where(prop => prop.Activo == true && prop.EstadoDGI.Equals("Aceptado")).OrderByDescending(p => p.Fecha);


                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Documento> ObtenerDocumentos(int? idCliente, String tipoDoc, DateTime FechaDesde, DateTime FechaHasta, String Serie, int nro, String Estado, String rut, String NroGuia, String NroRemito)
        {
            List<Documento> result = new List<Documento>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Include("Cliente").Where(pro => pro.IdDocumento > 0 && pro.rut.Equals(rut)).OrderByDescending(p => p.IdDocumento);
                    query = query.Where(prop => prop.Activo == true).OrderBy(p => p.IdDocumento);

                    if (!String.IsNullOrEmpty(tipoDoc))
                    {
                        if (tipoDoc.Equals("Factura"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("101") || fun.TipoDocumento.Equals("111")).OrderBy(p => p.IdDocumento);
                        }
                        else if (tipoDoc.Equals("Nota Crédito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("102") || fun.TipoDocumento.Equals("112")).OrderBy(p => p.IdDocumento);
                        }
                        else if (tipoDoc.Equals("Nota Débito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("103") || fun.TipoDocumento.Equals("113")).OrderBy(p => p.IdDocumento);
                        }
                        else if (tipoDoc.Equals("Remito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("181")).OrderBy(p => p.IdDocumento);
                        }
                        else if (tipoDoc.Equals("Resguardo"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("182")).OrderBy(p => p.IdDocumento);
                        }
                    }
                    query = query.Where(fun => fun.Fecha >= FechaDesde && fun.Fecha <= FechaHasta).OrderBy(p => p.IdDocumento);
                    if (!String.IsNullOrEmpty(Serie))
                    {
                        query = query.Where(fun => fun.Serie.ToUpper().Equals(Serie.ToUpper())).OrderBy(p => p.IdDocumento);
                    }
                    if (!String.IsNullOrEmpty(Estado))
                    {
                        query = query.Where(fun => fun.EstadoDGI.ToUpper().Equals(Estado.ToUpper())).OrderBy(p => p.IdDocumento);
                    }
                    if (!String.IsNullOrEmpty(NroGuia))
                    {
                        query = query.Where(fun => fun.NroGuia.ToUpper().Equals(NroGuia.ToUpper())).OrderBy(p => p.IdDocumento);
                    }
                    if (!String.IsNullOrEmpty(NroRemito))
                    {
                        query = query.Where(fun => fun.NroRemito.ToUpper().Equals(NroRemito.ToUpper())).OrderBy(p => p.IdDocumento);
                    }
                    if (nro != 0)
                    {
                        query = query.Where(fun => fun.NroSerie == nro).OrderBy(p => p.IdDocumento);
                    }
                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderBy(p => p.IdDocumento);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public int ObtenerProximoCodigo(String rut)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Emisor.FirstOrDefault(p => p.ruc.Equals(rut)).ultimoDocumento;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public void AumentarCodigo(String rut)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    DatosEmisor param = baseDatos.Emisor.FirstOrDefault(p => p.ruc.Equals(rut));
                    param.ultimoDocumento += 1;
                    baseDatos.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public Documento ObtenerDocumentoId(int id)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Documentos.Include("Detalle").Include("Detalle.Producto").Include("Detalle.Producto.indicador").FirstOrDefault(d => d.IdDocumento == id);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public void AnularDocumento(int IdDoc) {
            try {
                using (var baseDatos = new Context())
                {
                    Documento documento = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == IdDoc);
                    if (documento != null)
                    {
                        documento.EstadoDGI = "Anulado";
                    }
                    baseDatos.SaveChanges();
                }
            }
            catch { }
        }

        public void ModificarEstado(int IdDoc, String estado, String motivo)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Documento documento = baseDatos.Documentos.Include("Cliente").Include("Usuario").FirstOrDefault(d => d.IdDocumento == IdDoc);
                    if (documento != null)
                    {
                        documento.EstadoDGI = estado;
                        documento.MotivoRechazo = motivo;
                    }
                    baseDatos.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                
            }
            catch(Exception ex) { }
        }

        public void ModificarEstadoCreditoCliente(int IdDoc, String estado)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Documento documento = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == IdDoc);
                    if (documento != null)
                    {
                        documento.EstadoCredito = estado;
                        
                    }
                    baseDatos.SaveChanges();
                }
            }
            catch { }
        }

        public void ModificarMontoPagado(int IdDoc, decimal montoPagado)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Documento documento = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == IdDoc);
                    if (documento != null)
                    {
                        documento.MontoPagado += montoPagado;

                    }
                    baseDatos.SaveChanges();
                }
            }
            catch { }
        }

        public decimal ObtenerMontoPagado(int IdDoc)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Documento documento = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == IdDoc);
                    if (documento != null)
                    {
                        return documento.MontoPagado;

                    }
                    return 0;
                }
            }
            catch {
                return 0;
            }
        }

        
        #endregion

        #region Emisor

        public DatosEmisor ObtenerDatosEmisor(String rut)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Emisor.FirstOrDefault(e => e.ruc.Equals(rut));
            }
        }

        public List<DatosEmisor> ObtenerEmisores()
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Emisor.Where(e => e.IdEmisor > 0).ToList();
            }
        }

        public DatosEmisor ObtenerDatosEmisorId(int id)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Emisor.FirstOrDefault(e => e.IdEmisor == id);
            }
        }

        #endregion

        #region Parametros

        public int TasaMinima()
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Parametros.FirstOrDefault(p => p.IdParametros > 0).TasaMinima;
            }
        }

        public int TasaBasica()
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Parametros.FirstOrDefault(p => p.IdParametros > 0).TasaBasica;
            }
        }

        public int NroRecibo(String rut)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.Emisor.FirstOrDefault(p => p.ruc.Equals(rut)).NroRecibo;
            }
        }

        public void AumentarNroRecibo(String rut)
        {
            using (var baseDatos = new Context())
            {
                DatosEmisor parm = baseDatos.Emisor.FirstOrDefault(p => p.ruc.Equals(rut));
                if (parm != null)
                {
                    parm.NroRecibo++;
                    baseDatos.SaveChanges();
                }
            }
        }
        #endregion

        #region Reportes

        public List<Documento> ObtenerDocumentosAnulados(int? idCliente, String tipoDoc, DateTime FechaDesde, DateTime FechaHasta,String rut)
        {
            List<Documento> result = new List<Documento>();
            
            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Where(pro => pro.IdDocumento > 0 && pro.rut.Equals(rut)).OrderByDescending(p => p.Fecha);
                    query = query.Where(prop => prop.Activo == true && prop.EstadoDGI.Equals("Anulado")).OrderByDescending(p => p.Fecha);

                    if (!String.IsNullOrEmpty(tipoDoc))
                    {
                        if (tipoDoc.Equals("Factura"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("101") || fun.TipoDocumento.Equals("111")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Nota Débito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("103") || fun.TipoDocumento.Equals("113")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Remito"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("181")).OrderByDescending(p => p.Fecha);
                        }
                        else if (tipoDoc.Equals("Resguardo"))
                        {
                            query = query.Where(fun => fun.TipoDocumento.Equals("182")).OrderByDescending(p => p.Fecha);
                        }
                    }
                    query = query.Where(fun => fun.Fecha >= FechaDesde && fun.Fecha <= FechaHasta).OrderByDescending(p => p.Fecha);

                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderByDescending(p => p.Fecha);
                    }
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<Documento> ObtenerDocumentosAceptadosAnulados()
        {
            List<Documento> result = new List<Documento>();


            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Documentos.Where(pro => pro.IdDocumento > 0).OrderByDescending(p => p.Fecha);
                    TimeSpan time = new TimeSpan(30, 1, 1, 1, 1);
                    DateTime fecha = DateTime.Now.Subtract(time);
                    query = query.Where(prop => prop.Activo == true && (prop.EstadoDGI.Equals("Aceptado") || (prop.EstadoDGI.Equals("Anulado") && prop.Fecha > fecha))).OrderByDescending(p => p.Fecha);


                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public List<DocumentoVencido> ObtenerDocumentosVencidos(String moneda, String rut) {
            try {
                List<DocumentoVencido> result = new List<DocumentoVencido>();
                using (var baseDatos = new Context())
                {
                    List<Documento> documentos = baseDatos.Documentos.Include("Cliente").Where(doc => (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(moneda.ToUpper()) && doc.rut.Equals(rut) && doc.FechaVencimiento < DateTime.Now).OrderBy(doc=>doc.FechaVencimiento).ToList();
                    if (documentos != null)
                    {
                        decimal suma = 0;
                        foreach (Documento documento in documentos) {
                            DocumentoVencido documentoVencido = new DocumentoVencido();
                            documentoVencido.Cliente = documento.cliente.Nombre;
                            documentoVencido.Fecha = documento.Fecha;
                            documentoVencido.FechaVencimiento = documento.FechaVencimiento;
                            documentoVencido.Moneda = moneda;
                            documentoVencido.MontoPagado = documento.MontoPagado;
                            documentoVencido.MontoTotal = documento.Total;
                            documentoVencido.Numero = documento.NroSerie.ToString();
                            documentoVencido.Serie = documento.Serie;
                            documentoVencido.Saldo = (documentoVencido.MontoTotal - documentoVencido.MontoPagado);
                            documentoVencido.TipoDocumento = ObtenerDetalleTipoDocumento(documento.TipoDocumento);
                            suma += (documentoVencido.Saldo);
                            documentoVencido.SumaMontos = suma;
                            result.Add(documentoVencido);
                        }
                    }
                }
                return result;
            }
            catch {
                return null;
            }
        }

        public List<ReporteRecibos> ObtenerRecibos(int? idCliente, DateTime FechaDesde, DateTime FechaHasta, int? nro, String rut, string Estado, int? idVendedor, int? idZona)
        {
            List<ReporteRecibos> result = new List<ReporteRecibos>();
            decimal TotalPesos = 0;
            decimal TotalDolares = 0;
            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Recibos.Include("cliente").Where(pro => pro.IdRecibo > 0 && pro.rut.Equals(rut) ).OrderBy(p => p.Numero);
                    if (nro != null)
                    {
                        query = query.Where(fun => fun.Numero == nro).OrderBy(p => p.Numero);
                    }
                    else
                    {
                        query = query.Where(fun => fun.Fecha >= FechaDesde && fun.Fecha <= FechaHasta).OrderBy(p => p.Numero);
                    }
                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderBy(p => p.Numero);
                    }
                    if (idVendedor != null)
                    {
                        query = query.Where(fun => fun.cliente.IdVendedor == idVendedor).OrderBy(p => p.Numero);
                    }
                    if (idZona != null)
                    {
                        query = query.Where(fun => fun.cliente.IdZona == idZona).OrderBy(p => p.Numero);
                    }
                    if (Estado.Equals("Anulados"))
                    {
                        query = query.Where(fun => fun.Anulado == true).OrderBy(p => p.Numero);
                    }else if (Estado.Equals("Activos")){
                        query = query.Where(fun => fun.Anulado == false).OrderBy(p => p.Numero);
                    }
                    List<CabezalRecibo> recibos = query.ToList();
                    foreach (CabezalRecibo rec in recibos)
                    {
                        ReporteRecibos aux = new ReporteRecibos();
                        aux.IdRecibo = rec.IdRecibo;
                        aux.Fecha = rec.Fecha;
                        aux.Cliente = rec.cliente.ToString();
                        aux.Importe = rec.Importe;
                        aux.Moneda = rec.Moneda;
                        aux.Numero = rec.Numero;
                        aux.StrAnulado = rec.StrAnulado;
                        result.Add(aux);
                        if (aux.Moneda.Equals("Pesos"))
                        {
                            TotalPesos += aux.Importe;
                        }
                        else
                        {
                            TotalDolares += aux.Importe;
                        }
                    }
                    if (TotalPesos > 0)
                    {
                        ReporteRecibos total = new ReporteRecibos();
                        total.IdRecibo = 0;
                        total.Fecha = FechaHasta;
                        total.Cliente = "TOTAL";
                        total.Importe = TotalPesos;
                        total.Moneda = "Pesos";
                        total.StrAnulado = "";
                        result.Add(total);
                    }
                    if (TotalDolares > 0)
                    {
                        ReporteRecibos total = new ReporteRecibos();
                        total.IdRecibo = 0;
                        total.Fecha = FechaHasta;
                        total.Cliente = "TOTAL";
                        total.Importe = TotalDolares;
                        total.Moneda = "Dolar";
                        total.StrAnulado = "";
                        result.Add(total);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public CabezalRecibo ObtenerReciboId(int id)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Recibos.Include("cliente").Include("lineas").FirstOrDefault(d => d.IdRecibo == id);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public CabezalRecibo ObtenerReciboNro(String nro)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    int numero = Int32.Parse(nro);
                    return baseDatos.Recibos.Include("cliente").Include("lineas").FirstOrDefault(d => d.Numero == numero);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<ListadoFacturacion> ObtenerListadoFacturacion(int? idCliente, DateTime FechaDesde, DateTime FechaHasta, String rut) {
            try {
                List<ListadoFacturacion> result = new List<ListadoFacturacion>();
                using (var baseDatos = new Context()) {
                    FechaHasta = FechaHasta.AddHours(23);
                    var query = baseDatos.Documentos.Include("Cliente").Where(doc=>doc.Fecha >= FechaDesde && doc.Fecha <= FechaHasta && doc.rut.Equals(rut));
                    if (idCliente != null) {
                        query = query.Include("Cliente").Where(doc => doc.IdCliente == idCliente);
                    }
                    List<Documento> documentos = query.ToList();
                    if (documentos != null) {
                        decimal suma = 0;
                        foreach (Documento documento in documentos) {
                            ListadoFacturacion listado = new ListadoFacturacion();
                            listado.Cliente = documento.cliente.Nombre;
                            listado.Fecha = documento.Fecha;
                            listado.FormaPago = documento.FormaPago;
                            listado.NroSerie = documento.NroSerie.ToString();
                            listado.Serie = documento.Serie;
                            listado.TipoDocumento = ObtenerDetalleTipoDocumento(documento.TipoDocumento);
                            if (documento.Moneda.Equals("Dolar") && documento.tipoCambio!=null)
                            {
                                listado.Total = (decimal)documento.tipoCambio * documento.Total;
                            }
                            else
                            {
                                listado.Total = documento.Total;
                            }
                            if (documento.TipoDocumento.Equals("102") || documento.TipoDocumento.Equals("112") || documento.TipoDocumento.Equals("122") || documento.TipoDocumento.Equals("132") || documento.TipoDocumento.Equals("142"))
                            {
                                suma -= listado.Total;
                            }
                            else {
                                suma += listado.Total;
                            }
                            
                            listado.MontoTotal = suma;
                            result.Add(listado);
                        }
                    }
                }
                return result;
            }
            catch {
                return null;
            }            
        }

        public List<ListadoClienteProducto> ObtenerListadoClienteProducto(int? idCliente, int? idVendedor, int? idZona, int? idProducto, DateTime FechaDesde, DateTime FechaHasta) {
            try {
                List<ListadoClienteProducto> result = new List<ListadoClienteProducto>();
                using (var baseDatos = new Context()) { 
                    FechaHasta = FechaHasta.AddHours(23);
                    var query = baseDatos.Documentos.Include("cliente").Include("detalle.producto").Where(doc => doc.Fecha >= FechaDesde && doc.Fecha <= FechaHasta);
                    if (idCliente != null) {
                        query = query.Include("cliente").Include("detalle.producto").Where(doc=>doc.IdCliente == idCliente);
                    }
                    if (idVendedor != null)
                    {
                        query = query.Include("cliente").Include("detalle.producto").Where(doc => doc.cliente.IdVendedor == idVendedor);
                    }
                    if (idZona != null)
                    {
                        query = query.Include("cliente").Include("detalle.producto").Where(doc => doc.cliente.IdZona == idZona);
                    }
                    if (idProducto != null)
                    {
                        Detalle prod = new Detalle();
                        prod.IdProducto = (int)idProducto;
                        query = query.Include("cliente").Include("detalle.producto").Where(doc => doc.detalle.Contains(prod));
                    }
                    decimal MontoFinal = 0;
                    decimal TotalKilos = 0;
                    List<Documento> documentos = query.ToList();
                    if (documentos != null) {
                        foreach (Documento documento in documentos) {
                            if (documento.detalle != null) { 
                                foreach(Detalle detalle in documento.detalle){
                                    ListadoClienteProducto listado = new ListadoClienteProducto();
                                    listado.Cantidad = detalle.Cantidad;
                                    listado.Cliente = documento.cliente.Nombre;
                                    listado.Kilos = detalle.Kilos;
                                    listado.Producto = detalle.producto.Nombre;
                                    listado.RUT = documento.cliente.nroDoc;
                                    if (documento.TipoDocumento.Equals("102") || documento.TipoDocumento.Equals("112"))
                                    {
                                        listado.Total = -detalle.MontoTotal;
                                    }else{
                                        listado.Total = detalle.MontoTotal;
                                    }
                                    result.Add(listado);
                                    MontoFinal += listado.Total;
                                    TotalKilos += listado.Kilos;
                                }
                            }
                        }
                        ListadoClienteProducto lineaFinal = new ListadoClienteProducto();
                        lineaFinal.Cantidad = 0;
                        lineaFinal.Cliente = "";
                        lineaFinal.Kilos = TotalKilos;
                        lineaFinal.Producto = "TOTAL";
                        lineaFinal.RUT = "";
                        lineaFinal.Total = MontoFinal;
                        result.Add(lineaFinal);

                    }
                }
                return result;
            }
            catch {
                return null;
            }
        }

        public List<ResumenVentas> ObtenerResumenVentas(DateTime fechaDesde, DateTime fechaHasta, bool? VanRecibos) {
            try {
                List<ResumenVentas> resumen = new List<ResumenVentas>();
                using(var baseDatos = new Context()){
                    List<Documento> documentos = null;
                    List<CabezalRecibo> recibos = null;
                    fechaHasta = fechaHasta.AddHours(23);
                    if (VanRecibos == null || !(bool)VanRecibos)
                    {
                        documentos = baseDatos.Documentos.Include("cliente").Where(doc => doc.Fecha >= fechaDesde && doc.Fecha <= fechaHasta).OrderBy(doc => doc.Fecha).ToList();
                    }
                    if (VanRecibos == null || (bool)VanRecibos)
                    {
                        recibos = baseDatos.Recibos.Include("cliente").Where(doc => doc.Fecha >= fechaDesde && doc.Fecha <= fechaHasta && doc.Anulado == false).OrderBy(doc => doc.Fecha).ToList();
                    }
                    if (documentos != null)
                    {
                        foreach (Documento doc in documentos)
                        {
                            ResumenVentas res = new ResumenVentas();
                            res.Fecha = doc.Fecha;
                            if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112") || doc.TipoDocumento.Equals("122") || doc.TipoDocumento.Equals("132"))
                            {
                                res.Importe = (-1) * doc.Total;
                            }
                            else {
                                res.Importe = doc.Total;
                            }
                            
                            res.Detalle = ObtenerDetalleTipoDocumento(doc.TipoDocumento) + " " + doc.Serie + " " + doc.NroSerie + " - " + doc.cliente.Nombre;
                            resumen.Add(res);
                        }
                    }
                    if (recibos != null)
                    {
                        foreach (CabezalRecibo rec in recibos)
                        {
                            ResumenVentas res = new ResumenVentas();
                            res.Fecha = rec.Fecha;
                            res.Importe = rec.Importe;
                            res.Detalle = "Cobro Nº " + rec.Numero+" - " + rec.cliente.Nombre;
                            resumen.Add(res);
                        }
                    }
                    
                    
                }
                resumen = resumen.OrderBy(res => res.Fecha).ToList();
                return resumen;
            }
            catch {
                return null;
            }
        }

        public List<InformeCobrosClientes> ObtenerInformeCobroClientes(String rut, String Moneda, int? idZona, int? idVendedor)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    List<InformeCobrosClientes> result = new List<InformeCobrosClientes>();
                    var query = baseDatos.Documentos.Include("Cliente").Where(doc => (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut)).OrderBy(doc => doc.cliente.Nombre);

                    if (idZona != null)
                    {
                        query = query.Where(fun => fun.cliente.IdZona == idZona).OrderBy(doc => doc.cliente.Nombre);
                    }

                    if (idVendedor != null)
                    {
                        query = query.Where(fun => fun.cliente.IdVendedor == idVendedor).OrderBy(doc => doc.cliente.Nombre);
                    }
                    List<Documento> documentos = query.OrderBy(doc => doc.cliente.Nombre).ToList();


                    //List<Documento> documentos = baseDatos.Documentos.Include("Cliente").Where(doc => (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut)).OrderBy(doc => doc.FechaVencimiento).ToList();
                    if (documentos != null)
                    {
                        var grupoCliente = documentos.GroupBy(cli => cli.IdCliente);
                        foreach (var grupo in grupoCliente)
                        {
                            List<Documento> documentosCliente = grupo.OrderBy(doc=>doc.IdDocumento).ToList();
                            if (documentosCliente != null && documentosCliente.Count > 0)
                            {
                                decimal SaldoTotal = 0;
                                foreach (Documento doc in documentosCliente)
                                {
                                    InformeCobrosClientes estado = new InformeCobrosClientes();
                                    if (doc.TipoDocumento.Equals("101"))
                                    {
                                        estado.Detalle = "E-Ticket " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = (doc.Total - doc.MontoPagado);
                                        estado.Total = doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("111"))
                                    {
                                        estado.Detalle = "E-Factura " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = (doc.Total - doc.MontoPagado);
                                        estado.Total = doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                                    {
                                        estado.Detalle = "E-Nota de Credito " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = -(doc.Total - doc.MontoPagado);
                                        estado.Total = -doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("103") || doc.TipoDocumento.Equals("113"))
                                    {
                                        estado.Detalle = "E-Nota de Debito " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = (doc.Total - doc.MontoPagado);
                                        estado.Total = doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("121"))
                                    {
                                        estado.Detalle = "E-Factura de Exp. " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = (doc.Total - doc.MontoPagado);
                                        estado.Total = doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("122"))
                                    {
                                        estado.Detalle = "E-Nota de Credito Exp. " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = -(doc.Total - doc.MontoPagado);
                                        estado.Total = -doc.Total;
                                        SaldoTotal -= (decimal)estado.Saldo;
                                    }
                                    else if (doc.TipoDocumento.Equals("123"))
                                    {
                                        estado.Detalle = "E-Nota de Debito Exp. " + doc.Serie + " " + doc.NroSerie;
                                        estado.Saldo = (doc.Total - doc.MontoPagado);
                                        estado.Total = doc.Total;
                                        SaldoTotal += (decimal)estado.Saldo;
                                    }
                                    estado.FechaVencimiento = doc.FechaVencimiento.ToShortDateString();
                                    estado.Cliente = doc.cliente.Nombre;
                                    estado.Telefono = doc.cliente.Tel.ToString();
                                    result.Add(estado);
                                }
                                if (SaldoTotal > 0)
                                {
                                    InformeCobrosClientes informe = new InformeCobrosClientes();
                                    informe.SaldoTotal = SaldoTotal;
                                    result.Add(informe);
                                    result.Add(new InformeCobrosClientes());
                                }

                            }


                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<InformeCobrosClientes> ObtenerBalanceClientes(String rut, String Moneda, int? idZona, int? idVendedor, DateTime fecha, int? idCliente)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    List<InformeCobrosClientes> result = new List<InformeCobrosClientes>();
                    var query = baseDatos.Clientes.OrderBy(cli => cli.Nombre);
                    //int mes = 1;
                    //int anio = DateTime.Now.Year;
                    //if (fecha.Month == 1)
                    //{
                    //    mes = 12;
                    //    anio = fecha.Year - 1;
                    //}
                    //else
                    //{
                    //    mes = fecha.Month - 1;
                    //    anio = fecha.Year;
                    //}
                        
                    if (idZona != null)
                    {
                        query = query.Where(fun => fun.IdZona == idZona).OrderBy(cli => cli.Nombre);
                    }
                    if (idVendedor != null)
                    {
                        query = query.Where(fun => fun.IdVendedor == idVendedor).OrderBy(cli => cli.Nombre);
                    }
                    if (idCliente != null)
                    {
                        query = query.Where(fun => fun.IdCliente == idCliente).OrderBy(cli => cli.Nombre);
                    }
                    decimal TotalFinal = 0;
                    List<Cliente> clientes = query.ToList();
                    if (clientes != null)
                    {
                        
                        foreach (Cliente cli in clientes)
                        {
                            Decimal SaldoTotal = 0;
                            //Buscar saldo anterior a la fecha
                            //SaldosCliente saldo = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == cli.IdCliente && sal.Mes == mes && sal.Año == anio);
                            //if (saldo != null)
                            //{
                            //    SaldoTotal += saldo.Debe - saldo.Haber;
                            //}
                            //sumar los documentos y recibos segun corresponda del mes de la fecha
                            //List<Documento> documentosActuales = baseDatos.Documentos.Where(doc => doc.Fecha.Month == fecha.Month && doc.Fecha <= fecha && doc.IdCliente == cli.IdCliente).ToList();
                            List<Documento> documentosActuales = baseDatos.Documentos.Where(doc => doc.Fecha <= fecha && doc.IdCliente == cli.IdCliente).ToList();
                            if (documentosActuales != null)
                            {
                                foreach (Documento documento in documentosActuales)
                                {
                                    if (documento.DBCR.Equals("DB"))
                                    {
                                        SaldoTotal += documento.Total;
                                    }
                                    else if (documento.DBCR.Equals("CR"))
                                    {
                                        SaldoTotal -= documento.Total;
                                    }
                                }
                            }
                            //List<CabezalRecibo> recibosActuales = baseDatos.Recibos.Where(doc => doc.Fecha.Month == fecha.Month && doc.Fecha <= fecha && doc.IdCliente == cli.IdCliente && doc.Anulado == false).ToList();
                            List<CabezalRecibo> recibosActuales = baseDatos.Recibos.Where(doc => doc.Fecha <= fecha && doc.IdCliente == cli.IdCliente && doc.Anulado == false).ToList();
                            if (recibosActuales != null)
                            {
                                foreach (CabezalRecibo cab in recibosActuales)
                                {
                                    SaldoTotal -= cab.Importe;
                                }
                            }
                            //si saldo es distinto de 0 agregar a result
                            if (SaldoTotal > 0)
                            {
                                InformeCobrosClientes informe = new InformeCobrosClientes();
                                informe.Cliente = cli.Nombre;
                                informe.SaldoTotal = SaldoTotal;
                                TotalFinal += SaldoTotal;
                                result.Add(informe);
                            }
                        }
                        if (TotalFinal > 0)
                        {
                            InformeCobrosClientes informe = new InformeCobrosClientes();
                            informe.Cliente = "TOTAL";
                            informe.SaldoTotal = TotalFinal;
                            result.Add(informe);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<InformePorcentajes> ObtenerPorcentajePorClientes(String rut, String Moneda, DateTime FechaDesde, DateTime FechaHasta, int? idVendedor)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    List<InformePorcentajes> result = new List<InformePorcentajes>();
                    decimal TotalFacturadoNC = 0;
                    decimal TotalFacturado = 0;
                    var query = baseDatos.Documentos.Include("Cliente").Where(doc => doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut) && doc.Fecha >= FechaDesde && doc.Fecha <= FechaHasta).OrderBy(doc => doc.FechaVencimiento);
                    if (idVendedor != null)
                    {
                        query = query.Where(fun => fun.cliente.IdVendedor == idVendedor).OrderBy(doc => doc.FechaVencimiento);
                    }
                    List<Documento> documentos = query.ToList();

                    //List<Documento> documentos = baseDatos.Documentos.Include("Cliente").Where(doc => (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut)).OrderBy(doc => doc.FechaVencimiento).ToList();
                    if (documentos != null)
                    {
                        TotalFacturado = documentos.Where(doc=> !doc.TipoDocumento.Equals("102") && !doc.TipoDocumento.Equals("112")).Sum(doc => doc.Total);
                        TotalFacturadoNC = documentos.Where(doc => doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112")).Sum(doc => doc.Total);
                        TotalFacturado -= TotalFacturadoNC;
                        var grupoCliente = documentos.GroupBy(cli => cli.IdCliente);
                        foreach (var grupo in grupoCliente)
                        {
                            List<Documento> documentosCliente = grupo.ToList();
                            if (documentosCliente != null && documentosCliente.Count > 0)
                            {
                                decimal Total = 0;
                                decimal Porcentaje = 0;
                                String NombreCliente = "";
                                String DocumentoCliente = "";
                                foreach (Documento doc in documentosCliente)
                                {
                                    if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                                    {
                                        Total -= (decimal)doc.Total;
                                        NombreCliente = doc.cliente.Nombre;
                                        DocumentoCliente = doc.cliente.nroDoc.ToString();
                                    }
                                    else
                                    {
                                        Total += (decimal)doc.Total;
                                        NombreCliente = doc.cliente.Nombre;
                                        DocumentoCliente = doc.cliente.nroDoc.ToString();
                                    }
                                }
                                if (Total > 0)
                                {
                                    InformePorcentajes informe = new InformePorcentajes();
                                    informe.Nombre = NombreCliente;
                                    informe.Detalle = DocumentoCliente;
                                    informe.Total = Total;
                                    informe.Porcentaje = CalcularPorcentaje(TotalFacturado, Total); ;
                                    result.Add(informe);
                                }
                            }


                        }
                        if (TotalFacturado > 0)
                        {
                            InformePorcentajes informe = new InformePorcentajes();
                            informe.Nombre = "Total";
                            informe.Detalle = "";
                            informe.Total = TotalFacturado;

                            informe.Porcentaje = 100;
                            result.Add(informe);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<InformePorcentajes> ObtenerPorcentajePorMercaderia(String rut, String Moneda, DateTime FechaDesde, DateTime FechaHasta, int? idVendedor, int? idProducto)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    List<InformePorcentajes> result = new List<InformePorcentajes>();
                    var queryProd = baseDatos.Productos.Include("indicador").Where(ej => ej.Activo == true && ej.rut.Equals(rut)).OrderBy(ej => ej.Nombre);
                    if(idProducto!=null){
                        queryProd = queryProd.Where(ej => ej.IdProducto==idProducto).OrderBy(ej => ej.Nombre);
                    }
                    var productos = queryProd.ToList();
                    foreach (var pro in productos)
                    {
                        InformePorcentajes i = new InformePorcentajes();
                        i.Nombre = pro.Nombre;
                        i.codigoProducto = pro.Codigo;
                        i.cantidad = 0;
                        i.kilos = 0;
                        i.Total = 0;
                        result.Add(i);
                    }
                    decimal TotalRedondeo = 0;
                    decimal TotalKilos = 0;
                    decimal TotalFacturado = 0;
                    decimal TotalFacturadoNC = 0;
                    var query = baseDatos.Documentos.Include("cliente").Include("detalle.producto").Where(doc => doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut) && doc.Fecha >= FechaDesde && doc.Fecha <= FechaHasta).OrderBy(doc => doc.FechaVencimiento);
                    if (idVendedor != null)
                    {
                        query = query.Where(fun => fun.cliente.IdVendedor == idVendedor).OrderBy(doc => doc.FechaVencimiento);
                    }
                    TotalFacturado -= TotalFacturadoNC;
                    List<Documento> documentos = query.ToList();

                    //List<Documento> documentos = baseDatos.Documentos.Include("Cliente").Where(doc => (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut)).OrderBy(doc => doc.FechaVencimiento).ToList();
                    if (documentos != null)
                    {
                        TotalFacturado = documentos.Where(doc => !doc.TipoDocumento.Equals("102") && !doc.TipoDocumento.Equals("112")).Sum(doc => doc.Total);
                        TotalFacturadoNC = documentos.Where(doc => doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112")).Sum(doc => doc.Total);
                        TotalFacturado -= TotalFacturadoNC;
                        foreach (var doc in documentos)
                        {
                            TotalRedondeo += doc.Redondeo;
                            foreach (var det in doc.detalle)
                            {
                                foreach (var r in result)
                                {
                                    if (r.codigoProducto == det.producto.Codigo)
                                    {
                                        if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                                        {
                                            r.cantidad -= det.Cantidad;
                                            r.kilos -= det.Kilos;
                                            r.Total -= det.MontoTotal;
                                            TotalKilos -= det.Kilos;
                                        }
                                        else
                                        {
                                            r.cantidad += det.Cantidad;
                                            r.kilos += det.Kilos;
                                            r.Total += det.MontoTotal;
                                            TotalKilos += det.Kilos;
                                        }
                                        
                                        r.Porcentaje = CalcularPorcentaje(TotalFacturado, Convert.ToDecimal(r.Total));
                                    }
                                }
                            }
                        }
                        if (TotalRedondeo != 0 && idProducto==null)
                        {
                            InformePorcentajes informe = new InformePorcentajes();
                            informe.Nombre = "Redondeos";
                            informe.Detalle = "";
                            informe.Total = TotalRedondeo;

                            informe.Porcentaje = CalcularPorcentaje(TotalFacturado, Convert.ToDecimal(informe.Total));
                            result.Add(informe);
                        }
                        if (TotalFacturado > 0)
                        {
                            InformePorcentajes informe = new InformePorcentajes();
                            informe.Nombre = "Total";
                            informe.Detalle = "";
                            informe.kilos = TotalKilos;
                            informe.Total = TotalFacturado;

                            informe.Porcentaje = 100;
                            result.Add(informe);
                        }
                    }
                    for (int i = result.Count - 1; i >= 0; i--)
                    {
                        if (result[i].Total == 0)
                        {
                            result.RemoveAt(i);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Usuario

        public String GuardarUsuario(Usuario Usuario)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Usuario gru = baseDatos.Usuarios.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Usuario.Nombre.Trim().ToUpper()));

                    if (gru != null)
                    {
                        return "Error: Existe un usuario con el mismo nombre";
                    }
                    else
                    {

                        baseDatos.Usuarios.Add(Usuario);
                        baseDatos.SaveChanges();
                        return "El usuario se guardó correctamente";


                    }

                }
            }
            catch (Exception e)
            {
                using (var baseDatos = new Context())
                {
                    if (baseDatos != null && baseDatos.Usuarios != null)
                    {
                        baseDatos.Usuarios.Remove(Usuario);
                    }
                    return "Error al guardar el usuario";
                }
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Usuarios.OrderBy(ej => ej.Nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String ModificarUsuario(Usuario Usuario)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Usuario gru = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == Usuario.IdUsuario);
                    if (gru != null)
                    {

                        Usuario gru3 = baseDatos.Usuarios.FirstOrDefault(de => de.Nombre.Trim().ToUpper().Equals(Usuario.Nombre.Trim().ToUpper()) && de.IdUsuario != Usuario.IdUsuario);
                        if (gru3 != null)
                        {
                            return "Error: Existe un usuario con el mismo nombre";
                        }
                        else
                        {
                            gru.Contrasena = Usuario.Contrasena;
                            gru.Nombre = Usuario.Nombre;
                            baseDatos.SaveChanges();
                            return "El usuario se modificó correctamente";
                        }




                    }
                    else
                    {
                        return "Error al modificar el usuario";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error al modificar el usuario";
            }
        }

        public String EliminarUsuario(Usuario usuario)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    Usuario gru = baseDatos.Usuarios.FirstOrDefault(de => de.IdUsuario == usuario.IdUsuario);
                    if (gru != null)
                    {
                        baseDatos.Usuarios.Remove(usuario);
                        baseDatos.SaveChanges();
                        return "El usuario se eliminó correctamente";
                    }
                    else
                    {
                        return "Error al eliminar el usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el usuario";
            }
        }

        public List<Usuario> BuscarUsuarios(String Nombre, bool activo)
        {
            List<Usuario> result = new List<Usuario>();
            

            try
            {
                using (var baseDatos = new Context())
                {
                    var query = baseDatos.Usuarios.Where(pro => pro.IdUsuario > 0);
                    if (!String.IsNullOrEmpty(Nombre))
                    {
                        query = query.Where(fun => fun.Nombre.ToUpper().StartsWith(Nombre.ToUpper()));
                    }


                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        public Usuario BuscarUsuarioId(int id)
        {

            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.Usuarios.FirstOrDefault(prop => prop.IdUsuario == id);
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public Usuario ValidarUsuario(String nombre, String clave)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    string pass = Sistema.GetInstancia().ObtenerPasswordEncriptada(clave);
                    Usuario usu = baseDatos.Usuarios.FirstOrDefault(prop => prop.Nombre.ToUpper().Equals(nombre.ToUpper()));
                    if (usu != null)
                    {
                        if (usu.Contrasena.Equals(pass.ToUpper()))
                        {
                            return usu;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return usu;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region CodigosEmisor

        public String CodTerminal(String rut)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.CodigosEmisores.FirstOrDefault(p => p.rut.Equals(rut)).CodTerminal;
            }
        }

        public String CodComercio(String rut)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.CodigosEmisores.FirstOrDefault(p => p.rut.Equals(rut)).CodComercio;
            }
        }
        public String Contrasena(String rut)
        {
            using (var baseDatos = new Context())
            {
                return baseDatos.CodigosEmisores.FirstOrDefault(p => p.rut.Equals(rut)).Contrasena;
            }
        }

        #endregion

        #region CodigoRetencionPercepcion

        public List<CodigoRetencionPercepcion> ObtenerCodigosRetencionPercepcion()
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    return baseDatos.CodigosPercepcionRetencion.OrderBy(ej => ej.NroForm).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CodigoRetencionPercepcion BuscarCodigoRetencionPercepcionId(int id)
        {
            try
            {
                using (var baseDatos = new Context())
                {
                    CodigoRetencionPercepcion ind = baseDatos.CodigosPercepcionRetencion.FirstOrDefault(indi => indi.IdCodigoRetencionPercepcion == id);
                    return ind;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region EstadoCuenta

            public String CobroDeudores(List<DeudaClientes> estados, decimal importe, int idCliente, String moneda, string nroRecibo, string rut, String Observaciones) {
            try
            {
                if (estados != null && estados.Count > 0)
                {
                    DeudaClientes estado = estados.ElementAt(estados.Count - 1);
                    using (var baseDatos = new Context())
                    {
                        decimal resto = importe;
                        List<LineaRecibo> lineas = new List<LineaRecibo>();
                        foreach (DeudaClientes est in estados)
                        {
                            Documento doc = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == est.IdDocumento);
                            if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                            {
                                decimal saldoFaltante = doc.Total - doc.MontoPagado;
                                doc.MontoPagado += saldoFaltante;
                                doc.EstadoCredito = "PAGO";
                                    
                                LineaRecibo linea = new LineaRecibo();
                                linea.Documento = doc;
                                linea.IdDocumento = doc.IdDocumento;
                                linea.ImportePagado = saldoFaltante;
                                lineas.Add(linea);

                                foreach (DeudaClientes e in estados)
                                {
                                    if (saldoFaltante > 0)
                                    {
                                        Documento documento = baseDatos.Documentos.FirstOrDefault(d => d.IdDocumento == e.IdDocumento);
                                        if (!documento.TipoDocumento.Equals("102") || !documento.TipoDocumento.Equals("112") && (!documento.EstadoCredito.Equals("PAGO")))
                                        {
                                            Decimal pendiente = documento.Total - documento.MontoPagado;
                                            if (saldoFaltante >= pendiente && pendiente > 0)
                                            {
                                                documento.MontoPagado += pendiente;
                                                documento.EstadoCredito = "PAGO";
                                                saldoFaltante -= pendiente;
                                            }
                                            else
                                            {
                                                documento.MontoPagado += saldoFaltante;
                                                documento.EstadoCredito = "PARCIAL";
                                                saldoFaltante = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (resto > 0)
                                {
                                    if (doc != null)
                                    {
                                        doc.cliente = baseDatos.Clientes.FirstOrDefault(cl => cl.IdCliente == doc.IdCliente);
                                        doc.Usuario = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == doc.IdUsuario);
                                        decimal saldoFaltante = doc.Total - doc.MontoPagado;
                                        if (resto >= saldoFaltante)
                                        {
                                            doc.MontoPagado += saldoFaltante;
                                            doc.EstadoCredito = "PAGO";
                                            resto -= saldoFaltante;
                                            LineaRecibo linea = new LineaRecibo();
                                            linea.Documento = doc;
                                            linea.IdDocumento = doc.IdDocumento;
                                            linea.ImportePagado = saldoFaltante;
                                            lineas.Add(linea);
                                        }
                                        else
                                        {
                                            doc.MontoPagado += resto;
                                            doc.EstadoCredito = "PARCIAL";
                                            LineaRecibo linea = new LineaRecibo();
                                            linea.Documento = doc;
                                            linea.IdDocumento = doc.IdDocumento;
                                            linea.ImportePagado = resto;
                                            resto = 0;
                                            lineas.Add(linea);

                                        }
                                    }
                                } 
                            }
                        }
                            
                        if (lineas.Count > 0) { 
                            CabezalRecibo recibo = new CabezalRecibo();
                            recibo.cliente = baseDatos.Clientes.FirstOrDefault(cli=> cli.IdCliente==idCliente);
                            recibo.Fecha = DateTime.Now;
                            recibo.IdCliente = idCliente;
                            recibo.Importe = importe;
                            recibo.Anulado = false;
                            recibo.ImporteAsignado = estado.SaldoTotal;
                            recibo.Numero = Int32.Parse(nroRecibo);
                            //recibo.Numero = NroRecibo(recibo.cliente.rut).ToString();
                            recibo.lineas = lineas;
                            recibo.rut = rut;
                            recibo.Moneda = moneda;
                            recibo.Observaciones = Observaciones;
                            baseDatos.Recibos.Add(recibo);
                            baseDatos.SaveChanges();
                                SaldosCliente saldoActual = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == idCliente && sal.Mes == DateTime.Now.Month && sal.Año == DateTime.Now.Year);
                            if (saldoActual != null)
                            {
                                saldoActual.Haber += importe;
                            }
                        
                        }
                    }
                    return "El pago se realizó correctamente";
                }
                else {
                    return "No existe deuda a cobrar";
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return "Error cobrar";
            }
            catch {
                return "Error al cobrar";
                //if (ex.InnerException == null)
                //{
                //    return "Error: " + ex.Message;
                //}
                //else {
                //    return "Error: " + ex.InnerException.Message;
                //}

            }

        }

            public String AsignarPagos(List<int> idDocumentos, List<int> idRecibos, int idCliente, String moneda, String rut)
            {
                try
                {

                    if (idDocumentos.Count() != 0)
                    {
                        using (var baseDatos = new Context())
                        {
                            List<Documento> documentos = baseDatos.Documentos.Where(doc => idDocumentos.Contains(doc.IdDocumento)).ToList();
                            List<CabezalRecibo> recibos = baseDatos.Recibos.Where(rec => idRecibos.Contains(rec.IdRecibo)).ToList();
                            decimal totalDocumentos = documentos.Sum(doc => doc.Total - doc.MontoPagado);
                            decimal totalRecibos = recibos.Sum(rec => rec.Importe - rec.ImporteAsignado);
                            List<LineaRecibo> lineas = new List<LineaRecibo>();
                            List<Documento> documentosAfectados = new List<Documento>();
                            List<Documento> documentosDebe = new List<Documento>();
                            List<Documento> documentosHaber = new List<Documento>();
                            foreach (Documento doc in documentos)
                            {
                                if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                                {
                                    documentosHaber.Add(doc);
                                }
                                else
                                {
                                    documentosDebe.Add(doc);
                                }

                            }
                            decimal sumaDebe = documentosDebe.Sum(doc => doc.Total - doc.MontoPagado);
                            decimal sumaHaber = documentosHaber.Sum(doc => doc.Total - doc.MontoPagado);
                            decimal total = sumaDebe - sumaHaber - totalRecibos;
                            //se agregan las lineas a los recibos seleccionados
                            //si es 0 queda documentos pagos y recibo totalmente asignado
                            //si es >0 queda algun doocumento con pago parcial y recibo totalmente asignado
                            //si es <0 los recibos queda con saldo para asignar
                            foreach (Documento doc in documentosDebe)
                            {
                                foreach (Documento nc in documentosHaber)
                                {
                                    if (!documentosAfectados.Contains(doc))
                                    {
                                        documentosAfectados.Add(doc);
                                    }
                                    doc.cliente = baseDatos.Clientes.FirstOrDefault(cl => cl.IdCliente == doc.IdCliente);
                                    nc.cliente = baseDatos.Clientes.FirstOrDefault(cl => cl.IdCliente == nc.IdCliente);
                                    doc.Usuario = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == doc.IdUsuario);
                                    nc.Usuario = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == nc.IdUsuario);

                                    decimal saldoFaltante = doc.Total - doc.MontoPagado;
                                    if (saldoFaltante != 0 && (nc.Total - nc.MontoPagado) != 0)
                                    {
                                        if ((nc.Total - nc.MontoPagado) >= saldoFaltante)
                                        {
                                            doc.MontoPagado += saldoFaltante;
                                            doc.EstadoCredito = "PAGO";
                                            nc.MontoPagado += saldoFaltante;
                                            if (nc.Total == nc.MontoPagado)
                                            {
                                                nc.EstadoCredito = "PAGO";
                                            }
                                            else
                                            {
                                                nc.EstadoCredito = "PARCIAL";
                                            }
                                        }
                                        else
                                        {
                                            doc.MontoPagado += (nc.Total - nc.MontoPagado);
                                            doc.EstadoCredito = "PARCIAL";
                                            nc.MontoPagado += nc.Total;
                                            nc.EstadoCredito = "PAGO";
                                        }
                                    }
                                }
                                foreach (CabezalRecibo rec in recibos)
                                {
                                    if (!documentosAfectados.Contains(doc))
                                    {
                                        documentosAfectados.Add(doc);
                                    }

                                    doc.cliente = baseDatos.Clientes.FirstOrDefault(cl => cl.IdCliente == doc.IdCliente);
                                    doc.Usuario = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == doc.IdUsuario);
                                    //cuota.Usuario = baseDatos.Usuarios.FirstOrDefault(cl => cl.IdUsuario == doc.IdUsuario);

                                    decimal saldoFaltante = doc.Total - doc.MontoPagado;
                                    if (saldoFaltante != 0 && (rec.Importe - rec.ImporteAsignado) != 0)
                                    {
                                        if ((rec.Importe - rec.ImporteAsignado) >= saldoFaltante)
                                        {

                                            doc.MontoPagado += saldoFaltante;
                                            doc.EstadoCredito = "PAGO";
                                            rec.ImporteAsignado += saldoFaltante;
                                            LineaRecibo linea = new LineaRecibo();
                                            //linea.Documento = doc;
                                            linea.IdDocumento = doc.IdDocumento;
                                            linea.Documento = baseDatos.Documentos.FirstOrDefault(c => c.IdDocumento == doc.IdDocumento);
                                            linea.ImportePagado = saldoFaltante;
                                            lineas.Add(linea);
                                            rec.lineas.Add(linea);
                                        }
                                        else
                                        {
                                            doc.MontoPagado += (rec.Importe - rec.ImporteAsignado);
                                            doc.EstadoCredito = "PARCIAL";
                                            LineaRecibo linea = new LineaRecibo();
                                            //linea.Documento = doc;
                                            linea.IdDocumento = doc.IdDocumento;
                                            linea.Documento = baseDatos.Documentos.FirstOrDefault(c => c.IdDocumento == doc.IdDocumento);
                                            linea.ImportePagado = (rec.Importe - rec.ImporteAsignado);
                                            rec.ImporteAsignado += (rec.Importe - rec.ImporteAsignado);
                                            lineas.Add(linea);
                                            rec.lineas.Add(linea);
                                        }
                                    }
                                }

                            }
                            baseDatos.SaveChanges();
                            return "Se asignaron correctamente los pagos";
                        }

                    }
                    else
                    {
                        return "Error: debe seleccionar al menos un documento y un recibo o nota de credito";
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                    return "Error cobrar";
                }
                catch
                {
                    return "Error al cobrar";
                }
            }

            public List<EstadoCuentaCliente> ObtenerReporteEstadoCuentaClientes(int idCliente, DateTime fechaDesde,String moneda) {
                try {
                    List<EstadoCuentaCliente> result = new List<EstadoCuentaCliente>();
                  
                    int mes = 1;
                    int anio = DateTime.Now.Year;
                    if (fechaDesde.Month == 1)
                    {
                        mes = 12;
                        anio = fechaDesde.Year - 1;
                    }
                    else {
                        mes = fechaDesde.Month - 1;
                        anio = fechaDesde.Year;
                    }
                    decimal saldoDebeActual = 0;
                    decimal saldoHaberActual = 0;
                    using (var baseDatos = new Context()) {
                        //Obtengo saldo mes anterior
                        SaldosCliente saldo = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.IdCliente == idCliente && sal.Mes == mes && sal.Año == anio);
                        
                        
                        
                            
                                decimal saldoMesActualDebe = 0;
                                decimal saldoMesActualHaber = 0;
                                // Si la fecha desde es mayor al 1ero. calculo el saldo de los movimientos del 1ero a la fecha desde.
                                if (fechaDesde.Day > 1)
                                {
                                    DateTime fecha1 = new DateTime(fechaDesde.Year, fechaDesde.Month, 1);

                                    List<Documento> documentos = baseDatos.Documentos.Where(doc => doc.Fecha >= fecha1 && doc.Fecha < fechaDesde && doc.IdCliente == idCliente && doc.Moneda.Equals(moneda)).ToList();
                                    if (documentos != null) {
                                        foreach (Documento doc in documentos) {
                                            if (doc.DBCR.Equals("DB")) {
                                                saldoMesActualDebe += doc.Total;

                                            }
                                            else if (doc.DBCR.Equals("CR")) {
                                                saldoMesActualHaber += doc.Total;
                                            }
                                        }
                                    }
                                    List<CabezalRecibo> recibos = baseDatos.Recibos.Where(doc => doc.Fecha >= fecha1 && doc.Fecha < fechaDesde && doc.IdCliente == idCliente && doc.Anulado == false && doc.Moneda.Equals(moneda)).ToList();
                                    if (recibos != null) {
                                        foreach (CabezalRecibo cab in recibos) {
                                            saldoMesActualHaber += cab.Importe;
                                        }
                                    }
                                }
                                
                                EstadoCuentaCliente estado = new EstadoCuentaCliente();
                                if (saldo != null)
                                {
                                    estado.Debe = saldo.Debe + saldoMesActualDebe;
                                    estado.Haber = saldo.Haber + saldoMesActualHaber;

                                }
                                else {
                                    estado.Debe = saldoMesActualDebe;
                                    estado.Haber = saldoMesActualHaber;
                                    
                                }

                                saldoDebeActual += estado.Debe;
                                saldoHaberActual += estado.Haber;
                                estado.Saldo = saldoDebeActual - saldoHaberActual;
                                estado.Fecha = fechaDesde;
                                estado.Moneda = moneda;
                                estado.Detalle = "Saldo Inicial";
                                result.Add(estado);
                                
                            //Obtengo movimientos realizados desde la fecha desde hasta la fecha actual
                        
                                List<Documento> documentosActuales = baseDatos.Documentos.Where(doc => doc.Fecha >= fechaDesde && doc.IdCliente == idCliente).ToList();
                                if (documentosActuales != null)
                                {
                                    foreach (Documento documento in documentosActuales)
                                    {
                                        String detalle = ObtenerDetalleTipoDocumento(documento.TipoDocumento);
                                        EstadoCuentaCliente nuevo = new EstadoCuentaCliente();
                                        nuevo.Detalle = detalle;
                                        nuevo.Fecha = documento.Fecha;
                                        nuevo.Numero = documento.NroSerie.ToString();
                                        nuevo.Serie = documento.Serie;
                                        nuevo.TipoDocumento = documento.TipoDocumento;
                                        nuevo.Moneda = documento.Moneda;
                                        if (documento.DBCR.Equals("DB"))
                                        {
                                            saldoDebeActual += documento.Total;
                                            nuevo.Debe = documento.Total;
                                        }
                                        else if (documento.DBCR.Equals("CR"))
                                        {
                                            saldoHaberActual += documento.Total;
                                            nuevo.Haber = documento.Total;
                                        }
                                        else {
                                            nuevo.Debe = documento.Total;
                                            nuevo.Haber = documento.Total;
                                        }
                                        nuevo.Saldo = saldoDebeActual - saldoHaberActual;
                                        result.Add(nuevo);
                                    }
                                }
                                List<CabezalRecibo> recibosActuales = baseDatos.Recibos.Include("Lineas").Include("Lineas.Documento").Where(doc => doc.Fecha >= fechaDesde && doc.IdCliente == idCliente && doc.Anulado == false).ToList();
                                if (recibosActuales != null)
                                {
                                    foreach (CabezalRecibo cab in recibosActuales)
                                    {
                                        saldoHaberActual += cab.Importe;
                                        EstadoCuentaCliente nuevo = new EstadoCuentaCliente();
                                        String detalle = "Pago: ";
                                        if (cab.lineas != null) {
                                            int cont = cab.lineas.Count;
                                            foreach (LineaRecibo linea in cab.lineas) {
                                                detalle += linea.Documento.TipoDocumento + " - " + linea.Documento.Serie + " - " + linea.Documento.NroSerie;
                                                cont --;
                                                if (cont > 0) { 
                                                    detalle+= " / ";
                                                }
                                            }
                                        }
                                        nuevo.Detalle = detalle;
                                        nuevo.Fecha = cab.Fecha;
                                        nuevo.Haber += cab.Importe;
                                        nuevo.Numero += cab.Numero;
                                        nuevo.Moneda = cab.Moneda;
                                        nuevo.Saldo = saldoDebeActual - saldoHaberActual;
                                        result.Add(nuevo);
                                    }
                                }
                        
                    }
                    result = result.OrderBy(est => est.Fecha).ToList();
                    saldoDebeActual = 0;
                    saldoHaberActual = 0;
                    //Actualizo los saldos
                    if (result != null) {
                        foreach (EstadoCuentaCliente est in result) {
                            saldoDebeActual += est.Debe;
                            saldoHaberActual += est.Haber;
                            est.Saldo = saldoDebeActual - saldoHaberActual;
                        }
                    }
                    return result;
                }
                catch {
                    return null;
                }
            }

            public bool ActualizarSaldosClientes() {
                try {
                    using (var baseDatos = new Context()) {
                        SaldosCliente saldo1 = baseDatos.SaldosClientes.FirstOrDefault(sal => sal.Año == DateTime.Now.Year && sal.Mes == DateTime.Now.Month);
                        if (saldo1 == null) {
                            List<Cliente> clientes = baseDatos.Clientes.ToList();
                            if (clientes != null) {
                                int mes = 1;
                                int anio = DateTime.Now.Year;
                                if (DateTime.Now.Month == 1)
                                {
                                    mes = 12;
                                    anio = DateTime.Now.Year - 1;
                                }
                                else {
                                    mes = DateTime.Now.Month - 1;
                                    anio = DateTime.Now.Year;
                                }
                                foreach (Cliente cliente in clientes) {
                                    decimal debeAnterior = 0;
                                    decimal haberAnterior = 0;
                                    SaldosCliente saldoAnterior = baseDatos.SaldosClientes.FirstOrDefault(sal=>sal.IdCliente== cliente.IdCliente && sal.Mes==mes&& sal.Año==anio);
                                    if(saldoAnterior!=null){
                                        debeAnterior = saldoAnterior.Debe;
                                        haberAnterior = saldoAnterior.Haber;
                                    }
                                    SaldosCliente saldo = new SaldosCliente();
                                    saldo.Año = DateTime.Now.Year;
                                    saldo.Cliente = cliente;
                                    saldo.IdCliente = cliente.IdCliente;
                                    saldo.Mes = DateTime.Now.Month;
                                    saldo.Debe = debeAnterior;
                                    saldo.Haber = haberAnterior;
                                    baseDatos.SaldosClientes.Add(saldo);
                                    baseDatos.SaveChanges();
                                }
                            }
                        }
                    }
                    return true;
                }
                catch {
                    return false;
                }
            }

            public List<DeudaClientes> ObtenerDeudaCliente(int idCliente, String rut, String Moneda)
            {
                try
                {
                    using (var baseDatos = new Context())
                    {
                        List<DeudaClientes> result = new List<DeudaClientes>();

                        List<Documento> documentos = baseDatos.Documentos.Where(doc => doc.IdCliente == idCliente && (doc.EstadoCredito.Equals("DEBE") || doc.EstadoCredito.Equals("PARCIAL")) && doc.Moneda.ToUpper().Equals(Moneda.ToUpper()) && doc.rut.Equals(rut)).OrderBy(doc => doc.FechaVencimiento).ToList();
                        if (documentos != null)
                        {
                            decimal Saldo = 0;
                            foreach (Documento doc in documentos)
                            {
                                DeudaClientes estado = new DeudaClientes();
                                if (doc.TipoDocumento.Equals("101"))
                                {
                                    estado.Detalle = "E-Ticket " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = (doc.Total - doc.MontoPagado);
                                    estado.Total = doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("111"))
                                {
                                    estado.Detalle = "E-Factura " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = (doc.Total - doc.MontoPagado);
                                    estado.Total = doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112"))
                                {
                                    estado.Detalle = "E-Nota de Credito " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = -(doc.Total - doc.MontoPagado);
                                    estado.Total = -doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("103") || doc.TipoDocumento.Equals("113"))
                                {
                                    estado.Detalle = "E-Nota de Debito " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = (doc.Total - doc.MontoPagado);
                                    estado.Total = doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("121"))
                                {
                                    estado.Detalle = "E-Factura de Exp. " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = (doc.Total - doc.MontoPagado);
                                    estado.Total = doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("122"))
                                {
                                    estado.Detalle = "E-Nota de Credito Exp. " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = -(doc.Total - doc.MontoPagado);
                                    estado.Total = -doc.Total;
                                    Saldo -= estado.SaldoDocumento;
                                }
                                else if (doc.TipoDocumento.Equals("123"))
                                {
                                    estado.Detalle = "E-Nota de Debito Exp. " + doc.Serie + " " + doc.NroSerie;
                                    estado.SaldoDocumento = (doc.Total - doc.MontoPagado);
                                    estado.Total = doc.Total;
                                    Saldo += estado.SaldoDocumento;
                                }
                                estado.FechaEmision = doc.Fecha.ToShortDateString();
                                estado.FechaVencimiento = doc.FechaVencimiento.ToShortDateString();
                                estado.IdDocumento = doc.IdDocumento;
                                estado.Moneda = doc.Moneda;

                                //if ((doc.TipoDocumento.Equals("102") || doc.TipoDocumento.Equals("112") || doc.TipoDocumento.Equals("122")) && doc.documentosAsociados != null && doc.documentosAsociados.Count > 0)
                                //{

                                //}
                                //else {
                                estado.SaldoTotal = Saldo;
                                result.Add(estado);
                                // }

                            }
                        }
                        return result;
                    }
                }
                catch
                {
                    return null;
                }
            }

            public List<DeudaClientes> ObtenerPagosPendientes(int idCliente, String rut, String Moneda)
            {
                try
                {
                    using (var baseDatos = new Context())
                    {
                        List<DeudaClientes> result = new List<DeudaClientes>();
                        decimal Saldo = 0;

                        List<CabezalRecibo> recibos = baseDatos.Recibos.Where(rec => rec.IdCliente == idCliente && rec.Anulado == false && ((rec.Importe - rec.ImporteAsignado) > 0) && rec.Moneda.ToUpper().Equals(Moneda.ToUpper()) && rec.rut.Equals(rut)).ToList();
                        if (recibos != null)
                        {
                            foreach (CabezalRecibo rec in recibos)
                            {
                                DeudaClientes estado = new DeudaClientes();
                                estado.FechaEmision = rec.Fecha.ToShortDateString();
                                estado.FechaVencimiento = rec.Fecha.ToShortDateString();
                                estado.Moneda = rec.Moneda;
                                estado.SaldoDocumento = (rec.ImporteAsignado - rec.Importe);
                                estado.Total = rec.Importe * -1;
                                Saldo += estado.SaldoDocumento;
                                estado.IdDocumento = rec.IdRecibo;

                                if (rec.NotaCredito)
                                {
                                    Documento doc = rec.getNotaCredito();
                                    estado.Detalle = "NC - " + doc.Serie + doc.NroSerie;
                                }
                                else
                                {
                                    estado.Detalle = "Recibo - " + rec.Numero.ToString();
                                }


                                estado.SaldoTotal = Saldo;
                                result.Add(estado);
                            }
                        }
                        return result;
                    }
                }
                catch
                {
                    return null;
                }
            }

            public CabezalRecibo ObtenerRecibo(int NroRecibo)
            {
                try
                {
                    using (var baseDatos = new Context())
                    {
                        return baseDatos.Recibos.Include("cliente").Include("lineas").FirstOrDefault(r => r.Numero == NroRecibo);
                    }
                }
                catch
                {
                    return null;
                }
            }

            public String AnularRecibo(String nro, String Motivo)
            {
                try
                {

                    if (!String.IsNullOrEmpty(nro))
                    {
                        using (var baseDatos = new Context())
                        {
                            int NroRecibo = Int32.Parse(nro);
                            CabezalRecibo recibo = baseDatos.Recibos.FirstOrDefault(rec => rec.Numero == NroRecibo);
                            foreach (LineaRecibo linea in recibo.lineas)
                            {
                                Documento doc = baseDatos.Documentos.Include("Cliente").Include("Usuario").FirstOrDefault(d => d.IdDocumento == linea.IdDocumento);
                                doc.MontoPagado -= linea.ImportePagado;
                                if (doc.MontoPagado == 0)
                                {
                                    doc.EstadoCredito = "DEBE";
                                }
                                else
                                {
                                    doc.EstadoCredito = "PARCIAL";
                                }
                            }
                            
                            SaldosCliente saldo = baseDatos.SaldosClientes.Include("Cliente").FirstOrDefault(sal => sal.IdCliente == recibo.IdCliente && sal.Mes == recibo.Fecha.Month && sal.Año == recibo.Fecha.Year);
                            saldo.Haber -= recibo.Importe;
                            recibo.Anulado = true;
                            recibo.MotivoAnulacion = Motivo;
                            recibo.Numero = 0;
                            baseDatos.SaveChanges();
                            return "recibo anulado";
                        }
                    }
                    else
                    {
                        return "Recibo no puede estar vacio";
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                    return "Error al anular";
                }
                catch
                {
                    return "Error al anular recibo";
                }
            }
            
        #endregion

        #region FuncionesVarias

            public String ObtenerDetalleTipoDocumento(String TipoDocumento) {
                String detalle = "";
                if (TipoDocumento.Equals("113"))
                {
                    detalle = "Nota de Debito e-Factura ";
                }
                else if (TipoDocumento.Equals("103"))
                {
                    detalle = "Nota de Debito e-ticket";
                }
                else if (TipoDocumento.Equals("123"))
                {
                    detalle = "Nota de Debito e-Factura de exportacion";
                }
                else if (TipoDocumento.Equals("133"))
                {
                    detalle = "Nota de Deb. Cta Aj. e-ticket ";
                }
                else if (TipoDocumento.Equals("143"))
                {
                    detalle = "Nota de Deb. Cta Aj. e-Factura ";
                }
                else if (TipoDocumento.Equals("111"))
                {
                    detalle = "e-factura ";
                }
                else if (TipoDocumento.Equals("101"))
                {
                    detalle = "e-ticket ";
                }
                else if (TipoDocumento.Equals("121"))
                {
                    detalle = "e-factura exportacion";
                }
                else if (TipoDocumento.Equals("131"))
                {
                    detalle = "Fac Vta Cta Aj. e-ticket";
                }
                else if (TipoDocumento.Equals("141"))
                {
                    detalle = "Fac Vta Cta Aj. e-factura";
                }
                else if (TipoDocumento.Equals("102"))
                {
                    detalle = "Nota de Credito e-ticket";
                }
                else if (TipoDocumento.Equals("112"))
                {
                    detalle = "Nota de Credito e-factura";
                }
                else if (TipoDocumento.Equals("122"))
                {
                    detalle = "Nota de Credito e-factura exportacion";
                }
                else if (TipoDocumento.Equals("132"))
                {
                    detalle = "Nota Cr. Vta Cta Aj. e-ticket";
                }
                else if (TipoDocumento.Equals("142"))
                {
                    detalle = "Nota Cr. Vta Cta Aj. e-factura";
                }
                else
                {
                    detalle = TipoDocumento;
                }
                return detalle;

            }

            public string ObtenerPasswordEncriptada(String password)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                var sha1 = SHA1.Create();          
	            byte[] hashBytes = sha1.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }
                string result = sb.ToString();
                return result;
            }

            public decimal CalcularPorcentaje(decimal Total, decimal Fraccion)
            {
                decimal result = 0;
                result = (Fraccion * 100) / Total;
                result = decimal.Round(result, 3);
                return result;

            }
        #endregion

        #region Stock

            public void AumentarStock(int idProd, decimal cant, decimal kilos) {
                try {
                    using (var baseDatos = new Context()) {
                        StockProducto stock = baseDatos.StockProductos.Include("Producto").FirstOrDefault(st=>st.IdProducto == idProd);
                        if (stock != null) {
                            stock.Kilos += kilos;
                            stock.Cantidad += cant;
                            baseDatos.SaveChanges();
                        }
                    }
                }
                catch { }
            }

            public void DisminuirStock(int idProd, decimal cant, decimal kilos)
            {
                try
                {
                    using (var baseDatos = new Context())
                    {
                        StockProducto stock = baseDatos.StockProductos.Include("Producto").FirstOrDefault(st => st.IdProducto == idProd);
                        if (stock != null)
                        {
                            stock.Kilos -= kilos;
                            stock.Cantidad -= cant;
                            baseDatos.SaveChanges();
                        }
                    }
                }
                catch { }
            }
		public float ObtenerCotizacionDolar() 
            {
                try
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    Stream d;
                    StreamReader r;
                    string line;

                    d = client.OpenRead("http://www.bcu.gub.uy/estadisticas-e-indicadores/paginas/cotizaciones.aspx"); // Accede a la pagina que quieres buscar
                    r = new StreamReader(d); // lee la informacion o contenido de la web
                    line = r.ReadLine(); // recorre linea x linea la web
                    int lineaactual = 0;
                    string lineaValor = "";
                    float cotizacionDolar = 0;
                    while (line != null) // mientras exista contenido
                    {
                        string aux = ">DLS. USA BILLETE</td>";
                        if (lineaactual > 0)
                        {
                            lineaactual++;
                        }
                        if (line.ToString().Contains(aux))
                        {
                            lineaactual++;
                        }

                        if (lineaactual == 3)
                        {
                            lineaValor = line.ToString();
                            lineaValor = lineaValor.Replace("\t", "");
                            lineaValor = lineaValor.Replace("<td class=\"Venta alt\">", "");
                            lineaValor = lineaValor.Replace("</td>", "");
                            cotizacionDolar = (float)Convert.ToDouble(lineaValor);
                        }
                        // aca realizas tu codigo de verificacion o obtener informacion
                        line = r.ReadLine(); // para seguir leendo las otras lineas de la pagina
                    }
                    d.Close();
                    return cotizacionDolar;
                }catch
                {
                    return (float)Convert.ToDouble("0.0");
                }
            }
        #endregion
    }
}
