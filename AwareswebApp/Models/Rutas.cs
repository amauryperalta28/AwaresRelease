using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Rutas
    {
        /**
         * Encabezado de la ruta
         */
           [Key]
            public int numRuta { get; set; }
            public string Descripcion { get; set; }

        /**
         * Detalle de la ruta
         */
            public int idLugar { get; set; }
            public int idUsuario { get; set; }
            public int numReporte { get; set; }
            public string situacion { get; set; }
          
            public string ubicacion { get; set; }
            public string longitud { get; set; }
            public string latitud { get; set; }
            public string estatusReporte { get; set; }
            
            public DateTime fechaCreacion { get; set; }
            
    }
}