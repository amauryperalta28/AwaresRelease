using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Reportes_Ruta
    {
        [Key]
        public int id { get; set; }
        public int numRuta { get; set; }

        public int numReporte { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}