using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Consumo
    {

        [Key]
        public int idConsumo { get; set; }
        public string UsernameColaborador { get; set; }
        public string tipoConsumo { get; set; } //{diario, semanal, hora}
        public double lectura { get; set; }
        
        public DateTime fechaCreacion { get; set; }

        public Consumo(string userName_Colaborador, double lectura_Consumo)
        {
            
            UsernameColaborador = userName_Colaborador;
            lectura = lectura_Consumo;
            fechaCreacion = DateTime.Now;
            tipoConsumo = "Mensual";

        }
        public Consumo()
        {
            fechaCreacion = DateTime.Now;
            tipoConsumo = "Mensual";

        }
        

    }
}