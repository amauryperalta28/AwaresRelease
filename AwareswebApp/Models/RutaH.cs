using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class RutaH
    {
        [Key]
        public int Rutaid{ get; set; }
        public string usuario{ get; set; }
        public string status{ get; set; }
    }
}