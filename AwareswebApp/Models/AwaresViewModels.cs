using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    /** 
     * Modelo de vista para representar pantalla de historial de Consumo General
     */
    public class HistGenConsumoViewModel
    {
        // Este es el nombre del usuario
        public string username;

        // Este es el consuno del usuario
        public double consumo;

        public HistGenConsumoViewModel() { username = ""; consumo = 0.0; }
    
    }
    /** 
    * Modelo de vista para representar pantalla de historial de Consumo Detallado por mes
    */
    public class HistDetConsumoViewModel
    {
        // Este es el nombre del usuario
        public string UsernameColaborador;

        // Este es el mes del consumo
        public int month;

        // Este es el anio del consumo
        public int year;

        // Este es el consuno del usuario
        public double lectura;

        public string fechaCreacion;
    
    }

}