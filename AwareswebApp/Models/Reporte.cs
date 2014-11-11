using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Reporte
    {
        
        [Key]
        public int numReporte { get; set; }
        public int numReporteUsr { get; set; }
        public string userName { get; set; }

        public string Descripcion { get; set; }
        public string situacion { get; set; }
        #region ubicacion
        public string pais { get; set; }
        public string localidad { get; set; }
        public string sector { get; set; }

        public string calle { get; set; }

        public double longitud { get; set; }
        public double latitud { get; set; }
        #endregion
       
        public string FotoUrl { get; set; }
        
        public string Comentarios { get; set; }        
        public string estatus { get; set; }

        public DateTime fechaCreacion { get; set; }
        public DateTime fechaCorreccion { get; set; }

        public Reporte(int numReporteUsr, string idUsuario, string situacion,  
                       double longitud, double latitud, string pais, string localidad, string sector, string calle)
        {
            this.numReporteUsr = numReporteUsr;
            this.userName = idUsuario;
            this.situacion = situacion;
            this.longitud = longitud;
            this.latitud = latitud;
            this.pais = pais;
            this.localidad = localidad;
            this.sector = sector;
            this.calle = calle;
            fechaCorreccion = DateTime.Now.Add(new TimeSpan(7));
            fechaCreacion = DateTime.Now;
            estatus = "1";
            Comentarios = " ";
            Descripcion = "";
            FotoUrl = "";

        }

        public Reporte ()
	    {
            fechaCorreccion = DateTime.Now.Add(new TimeSpan(7));
            fechaCreacion = DateTime.Now;
            estatus = "1";
            Comentarios = " ";
            Descripcion = "";
            FotoUrl = "";
            situacion = "";
            
	    }
    }
}