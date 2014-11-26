using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Business
    {
        private DbModels db = new DbModels();
        /*
         *Obtiene la Cantidad de reportes por sector, filtrado por mes y año y situacion
         *@param month       Este es el mes por el que se va a filtrar
         *@param year        Este es el año por el que se va a filtrar
         *                   El año sera especificado siempre
         *@param situacion   Esta es la sitaucion por la que se va a filtrar
         *@return            Una lista de reportes por sector filtrado por mes, año  y situacion
        */
        public List<rePortesPorSector> getReportMonthYearSituation(string month, string year, string situacion)
        {
            // Obtengo los datos y convierto
            int mes = Convert.ToInt32(month);
            int anio = Convert.ToInt32(year);
            
            //Si me indicaron el mes
            if (!String.IsNullOrEmpty(month))
            {
                //Si me indicaron la situacion
                if (!String.IsNullOrEmpty(situacion))
                {
                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year == anio &&  a.fechaCreacion.Month == mes &&
                                    a.situacion == situacion
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
                else
                {

                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year == anio &&
                                    a.fechaCreacion.Month == mes
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
            }
            else
            {
                var res = from a in db.Reportes
                          where a.fechaCreacion.Year == anio 
                          group a by a.sector into b
                          select new rePortesPorSector
                          {
                              sector = b.Key,
                              cantidad = b.Count()
                          };
                return res.ToList();
            
            }




        }

        /*
         *Obtiene la Cantidad de reportes por sector, filtrado año, situacion y por un rango de meses
         *@param year        Este es el año por el que se va a filtrar
         *                   El año sera especificado siempre
         *@param mes1        Limite inferior del rango de meses                   
         *@param mes2        Limite superior del rango de meses
         *@param situacion   Esta es la sitaucion por la que se va a filtrar
         *@return            Una lista de reportes por sector filtrado por un rango de mes, año  y situacion
        */
        public List<rePortesPorSector> getReportRangeMonthYearSituation(string year, string mes1, string mes2, string situacion)
        {
            int anio = Convert.ToInt32(year);
            int m1 = Convert.ToInt32(mes1);
            int m2 = Convert.ToInt32(mes2);

            //Si me indicaron el mes
            if (!String.IsNullOrEmpty(mes1) && !String.IsNullOrEmpty(mes2))
            {
                //Si me indicaron la situacion
                if (!String.IsNullOrEmpty(situacion))
                {
                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year == anio &&
                                    a.fechaCreacion.Month >= m1 &&  a.fechaCreacion.Month <= m2 &&
                                    a.situacion == situacion
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
                else
                {
                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year == anio &&
                                    a.fechaCreacion.Month >= m1 &&  a.fechaCreacion.Month <= m2 
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
            }
            else
            {
                var res = from a in db.Reportes
                          where a.fechaCreacion.Year == anio
                          group a by a.sector into b
                          select new rePortesPorSector
                          {
                              sector = b.Key,
                              cantidad = b.Count()
                          };
                return res.ToList();
            }
        }

        /*
         Obtiene la Cantidad de reportes por sector, filtrado por mes, situacion y un rango de años
         *@param month        Este es el año por el que se va a filtrar
         *                   El año sera especificado siempre
         *@param year1        Limite inferior del rango de meses                   
         *@param year2        Limite superior del rango de meses
         *@param situacion   Esta es la sitaucion por la que se va a filtrar
         *@return            Una lista de reportes por sector filtrado por un rango de mes, año  y situacion 
        */
         public List<rePortesPorSector> getReportMonthRangeYearSituation(string month, string year1, string year2, string situacion)
        {
            int mes = Convert.ToInt32(month);
            int y1 = Convert.ToInt32(year1);
            int y2 = Convert.ToInt32(year2);

            //Si me indicaron el mes
            if (!String.IsNullOrEmpty(year1) && !String.IsNullOrEmpty(year2))
            {
                //Si me indicaron la situacion
                if (!String.IsNullOrEmpty(situacion))
                {
                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year >= y1 && a.fechaCreacion.Year <= y2 &&
                                    a.fechaCreacion.Month == mes &&
                                    a.situacion == situacion
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
                else
                {
                    var res = from a in db.Reportes
                              where a.fechaCreacion.Year >= y1 && a.fechaCreacion.Year <= y2 &&
                                    a.fechaCreacion.Month == mes 
                              group a by a.sector into b
                              select new rePortesPorSector
                              {
                                  sector = b.Key,
                                  cantidad = b.Count()
                              };
                    return res.ToList();
                }
            }
            else
            {
                // Si no se indica rango de años
                var res = new List<rePortesPorSector>();
                return res.ToList();
            }

        }
    }
}