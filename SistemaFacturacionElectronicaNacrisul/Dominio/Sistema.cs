using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dominio.Clases_Dominio;

namespace Dominio
{
    public partial class Sistema
    {
        private static Sistema sistema;
       // private Context baseDatos;
        public decimal Total;
        public decimal SubTotal;
        public decimal Impuestos;
        public byte[] PDFActual;
        public byte[] ExcelActual;
       // public decimal deuda = 0;

        public static Sistema GetInstancia()
        {
            if (sistema == null)
            {
                sistema = new Sistema();
                
            }

            return sistema;
        }

        public static Sistema GetInstancia(String baseDatos)
        {
            if (sistema == null)
            {
                sistema = new Sistema(baseDatos);
            }

            return sistema;
        }

        private Sistema()
        {
            //Crea el contexto para acceder 
            //baseDatos = new Context();
            Total = 0;
            SubTotal = 0;
            Impuestos = 0;
            PDFActual = null;
            ExcelActual = null;
         //   deuda = 0;
        }

        private Sistema(String nombreBaseDatos)
        {
            //Crea el contexto para acceder 
           // baseDatos = new Context(nombreBaseDatos);
            Total = 0;
            SubTotal = 0;
            Impuestos = 0;
            PDFActual = null;
            ExcelActual = null;
          //  deuda = 0;
        }

        
    }
}
