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
            if (!month.Equals("0"))
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
            else if(!String.IsNullOrEmpty(situacion))
            {
                var res = from a in db.Reportes
                          where a.fechaCreacion.Year == anio &&
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

        /**
         * Obtienen el porcentaje almacenado de cada tipo de reporte, filtrado por rangos de mes,
         * rangos de año, y sector.
         * @param m1         Este es el limite inferior del rango de mes
         * @param m2         Este es el limite superior del rango de mes
         * @param y1         Este es el limite inferior del rango de año
         * @param y2         Este es el limite superior del rango de año
         * @param sector     Este es el sector
         * @return           Una lista de porcentajes de reportes
         */
         public List<rePortesPorTipoSituacion> getReportPercentMonthYearSector(string m1, string m2, string y1, string y2, string sector)
         { 
             //Obtengo los valores indicados
             int mes1 = Convert.ToInt32(m1);
             int mes2 = Convert.ToInt32(m2);
             int year1 = Convert.ToInt32(y1);
             int year2 = Convert.ToInt32(y2);

             // Si especifican el sector y el rango de mes
             if( (!m1.Equals("0") && !m2.Equals("0")) )
             {
                 if(!string.IsNullOrEmpty(sector))
                 {
                     var res = from a in db.Reportes
                               where a.fechaCreacion.Year >= year1 &&
                                     a.fechaCreacion.Year <= year2 &&
                                     a.fechaCreacion.Month >= mes1 &&
                                     a.fechaCreacion.Month <= mes2 &&
                                     a.sector == sector
                               group a by a.situacion into b
                               select new rePortesPorTipoSituacion
                               {
                                   situacion = b.Key,
                                   cantidad = b.Count()
                               };
                     return res.ToList();
                 }
                 else
                 {
                     var res = from a in db.Reportes
                               where a.fechaCreacion.Year >= year1 &&
                                     a.fechaCreacion.Year <= year2 &&
                                     a.fechaCreacion.Month >= mes1 &&
                                     a.fechaCreacion.Month <= mes2 
                               group a by a.situacion into b
                               select new rePortesPorTipoSituacion
                               {
                                   situacion = b.Key,
                                   cantidad = b.Count()
                               };
                     return res.ToList();

                 }
                 
             }
             // Si solo me especifican el sector
             else if(!string.IsNullOrEmpty(sector))
             {
                 var res = from a in db.Reportes
                           where a.fechaCreacion.Year >= year1 &&
                                 a.fechaCreacion.Year <= year2 &&
                                 a.sector == sector
                           group a by a.situacion into b
                           select new rePortesPorTipoSituacion
                           {
                               situacion = b.Key,
                               cantidad = b.Count()
                           };
                 return res.ToList();

             }
                 // Solo se indicaron los años
             else if (!m1.Equals("0") || !m2.Equals("0"))
             {
                 
                 return null;              

             }
             else 
             {
                 if(!string.IsNullOrEmpty(sector))
                 {
                     var res = from a in db.Reportes
                               where a.fechaCreacion.Year >= year1 &&
                                     a.fechaCreacion.Year <= year2 &&
                                     a.sector == sector
                               group a by a.situacion into b
                               select new rePortesPorTipoSituacion
                               {
                                   situacion = b.Key,
                                   cantidad = b.Count()
                               };
                     return res.ToList(); 

                 }
                 else
                 {
                     var res = from a in db.Reportes
                               where a.fechaCreacion.Year >= year1 &&
                                     a.fechaCreacion.Year <= year2
                               group a by a.situacion into b
                               select new rePortesPorTipoSituacion
                               {
                                   situacion = b.Key,
                                   cantidad = b.Count()
                               };
                     return res.ToList(); 
                 }
                 
             }
         }

        /**
         * Obtiene el porcentaje de Reportes que fueron resueltos y los que no fueron resueltos
         * @param m1         Este es el limite inferior del rango de mes
         * @param m2         Este es el limite superior del rango de mes
         * @param y1         Este es el limite inferior del rango de año
         * @param y2         Este es el limite superior del rango de año
         * @return           Una lista de porcentajes de situaciones resueltas y no resueltas
         */
        public List<reportesPorEstatus> getReportStatus(string m1, string m2, string y1, string y2)
         {
             //Obtengo los valores indicados
             int mes1 = Convert.ToInt32(m1);
             int mes2 = Convert.ToInt32(m2);
             int year1 = Convert.ToInt32(y1);
             int year2 = Convert.ToInt32(y2);
             TiposEstatus st1 = new TiposEstatus { id = "1", descrip = "No resuelto" };
             TiposEstatus st2 = new TiposEstatus { id = "2", descrip = "Resuelto" };
             List<TiposEstatus> status = new List<TiposEstatus>{st1,st2};
             
             status.Add(st1);
             status.Add(st2);
             // Si especifican el sector y el rango de mes
             if ((!m1.Equals("0") && !m2.Equals("0")))
             {
                 var res = from a in db.Reportes
                           where a.fechaCorreccion.Year >= year1 &&
                                 a.fechaCorreccion.Year <= year2 &&
                                 a.fechaCorreccion.Month >= mes1 &&
                                 a.fechaCorreccion.Month <= mes2 
                           group a by a.estatus into b
                           select new reportesPorEstatus
                           {
                               status = b.Key,
                               cantidad = b.Count()
                           };
                 return res.ToList();


             }
             else if ((!m1.Equals("0") || !m2.Equals("0")))
             {
                 
                 return null;

             }
             else
             {
                 var res = from a in db.Reportes
                           where a.fechaCorreccion.Year >= year1 &&
                                 a.fechaCorreccion.Year <= year2 
                           group a by a.estatus into b
                           select new reportesPorEstatus
                           {
                               status = b.Key,
                               cantidad = b.Count()
                           };
                 return res.ToList();

             }


         }


    }
}