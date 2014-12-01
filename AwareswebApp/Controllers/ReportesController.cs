using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwareswebApp.Models;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web.UI.DataVisualization.Charting;


namespace AwareswebApp.Controllers
{
    public class ReportesController : Controller
    {
        private DbModels db = new DbModels();
        private string pais;
        private string localidad;
        private string sector;
        private String calle;
        public List<string> atributos = new List<string>();


        static string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";

        public ActionResult BuildChart()
        {

            return View();
        }

        #region estadisticas

        public ActionResult showChart(string monthFilter1, string monthFilter2, string yearFilter1, string yearFilter2, string sectorFilter)
        {
            
            getParamsEst(monthFilter1, monthFilter2, yearFilter1, yearFilter2, sectorFilter);
            // Se rellenan los dropdownlist con los elementos correspondientes
            string[] month = { " 1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            var monthLst = new List<string>();
            var yearLst = new List<string>();
            var SectorLst = new List<string>();
            //Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable

            var yearQry = from a in db.Reportes
                          select a.fechaCreacion.Year.ToString();

            var SectorQry = from a in db.Reportes
                        select a.sector;

            // Los anios en los que se han realizado consumos en el dropdownlist
            yearLst.AddRange(yearQry.Distinct());
            ViewBag.yearFilter1 = new SelectList(yearLst);
            ViewBag.yearFilter2 = new SelectList(yearLst);

            //Agrego los meses al dropdownLst
            monthLst.AddRange(month);
            ViewBag.monthFilter1 = new SelectList(monthLst);
            ViewBag.monthFilter2 = new SelectList(monthLst);

            SectorLst.AddRange(SectorQry.Distinct());
            ViewBag.sectorFilter = new SelectList(SectorLst);

            return View();
        }
       
        public ActionResult showChart2()
        {
           // Se obtiene los parametros guardados en el session collection
            string m1 = Session["monthFilter1"].ToString();
            string m2 = Session["monthFilter2"].ToString();
            string y1 = Session["yearFilter1"].ToString();
            string y2 = Session["yearFilter2"].ToString();
            string sector = Session["sectorFilter"].ToString();
            Session.Clear();
            Business obj = new Business();

            var d = obj.getReportPercentMonthYearSector(m1, m2, y1, y2, sector);
           

            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Est_PorSector(string yearFilter, string monthFilter, string tipoSituacion)
        {
            string[] month = { " 1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            if (!String.IsNullOrEmpty(monthFilter) && !String.IsNullOrEmpty(tipoSituacion))
            {
                Session["p1"] = yearFilter;
                Session["p2"] = monthFilter;
                Session["p3"] = tipoSituacion;


            }
            else if(!String.IsNullOrEmpty(monthFilter))
            {
                Session["p1"] = yearFilter;
                Session["p2"] = monthFilter;
            }
            else if (!String.IsNullOrEmpty(yearFilter))
            {
                Session["p1"] = yearFilter;
            }
           
            // Creo una lista para guardar colaboradores
            
            var monthLst = new List<string>();
            var yearLst = new List<string>();
            var RpLst = new List<string>();
            //Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable

            var yearQry = from a in db.Reportes
                          select a.fechaCreacion.Year.ToString();

            var RpQry = from a in db.Reportes
                        select a.situacion;

            // Los anios en los que se han realizado consumos en el dropdownlist
            yearLst.AddRange(yearQry.Distinct());
            ViewBag.yearFilter = new SelectList(yearLst);

            //Agrego los meses al dropdownLst
            monthLst.AddRange(month);
            ViewBag.monthFilter = new SelectList(monthLst);

            RpLst.AddRange(RpQry.Distinct());
            ViewBag.tipoSituacion = new SelectList(RpLst);
            return View();
        }

        // Obtiene la Cantidad de reportes por sector, filtrado por mes y año y situacion
        public ActionResult Est_PorSectorData()
        {
            int param = Session.Count;
            string _mes = "0";
            string _anio = "0";
            string _situacion = "";

            if (param == 3)
            {
                 _anio = Session["p1"].ToString();
                 _mes = Session["p2"].ToString();
                _situacion = Session["p3"].ToString();
            
            }
            else if (param == 2)
            {
                _anio = Session["p1"].ToString();
                _mes = Session["p2"].ToString();
            }
            else if(param == 1)
            {
                _anio = Session["p1"].ToString();
            }
            Session.Clear();
            

            //var d = from a in db.Reportes
            //        group a by a.sector into b
            //        select new rePortesPorSector
            //        {
            //            sector = b.Key,
            //            cantidad = b.Count()
            //        };

            Business obj = new Business();
            var d = obj.getReportMonthYearSituation(_mes, _anio, _situacion);

          

            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Est_porEstatus(string monthFilter1, string monthFilter2, string yearFilter1, string yearFilter2)
        {
            getParamsEst(monthFilter1, monthFilter2, yearFilter1, yearFilter2, "");
            // Se rellenan los dropdownlist con los elementos correspondientes
            string[] month = { " 1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            var monthLst = new List<string>();
            var yearLst = new List<string>();
            //Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable

            var yearQry = from a in db.Reportes
                          select a.fechaCreacion.Year.ToString();

            // Los anios en los que se han realizado consumos en el dropdownlist
            yearLst.AddRange(yearQry.Distinct());
            ViewBag.yearFilter1 = new SelectList(yearLst);
            ViewBag.yearFilter2 = new SelectList(yearLst);

            //Agrego los meses al dropdownLst
            monthLst.AddRange(month);
            ViewBag.monthFilter1 = new SelectList(monthLst);
            ViewBag.monthFilter2 = new SelectList(monthLst);

            return View();
        }

        public ActionResult Est_porEstatusData()
        {
            // Se obtiene los parametros guardados en el session collection
            string m1 = Session["monthFilter1"].ToString();
            string m2 = Session["monthFilter2"].ToString();
            string y1 = Session["yearFilter1"].ToString();
            string y2 = Session["yearFilter2"].ToString();
            Session.Clear();
            Business obj = new Business();

            var d = obj.getReportStatus(m1, m2, y1, y2);
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }

        public void getParamsEst(string monthFilter1, string monthFilter2, string yearFilter1, string yearFilter2, string sectorFilter)
        {
            if (!String.IsNullOrEmpty(monthFilter1) && !String.IsNullOrEmpty(monthFilter2) )
            {
                if(!String.IsNullOrEmpty(sectorFilter))
                {
                    Session["monthFilter1"] = monthFilter1;
                    Session["monthFilter2"] = monthFilter2;
                    Session["sectorFilter"] = sectorFilter;
                    Session["yearFilter1"] = yearFilter1;
                    Session["yearFilter2"] = yearFilter2;
                }
                else
                {
                    Session["monthFilter1"] = monthFilter1;
                    Session["monthFilter2"] = monthFilter2;
                    Session["sectorFilter"] = "";
                    Session["yearFilter1"] = yearFilter1;
                    Session["yearFilter2"] = yearFilter2;
                }
                
            }
            else if (!String.IsNullOrEmpty(sectorFilter) && ( !String.IsNullOrEmpty(monthFilter1) || !String.IsNullOrEmpty(monthFilter2)))
            {
                Session["monthFilter1"] = "0";
                Session["monthFilter2"] = "0";
                Session["sectorFilter"] = sectorFilter;
                Session["yearFilter1"] = yearFilter1;
                Session["yearFilter2"] = yearFilter2;
            }
            else if (!String.IsNullOrEmpty(yearFilter1) )
            {
                if ((!String.IsNullOrEmpty(monthFilter1) || !String.IsNullOrEmpty(monthFilter2)))
                {
                    Session["monthFilter1"] = "0";
                    Session["monthFilter2"] = "0";
                    Session["sectorFilter"] = "";
                    Session["yearFilter1"] = "0";
                    Session["yearFilter2"] = "0";
                }
                else
                {
                    if(!String.IsNullOrEmpty(sectorFilter))
                    {
                        Session["monthFilter1"] = "0";
                        Session["monthFilter2"] = "0";
                        Session["sectorFilter"] = sectorFilter;
                        Session["yearFilter1"] = yearFilter1;
                        Session["yearFilter2"] = yearFilter2;
                    }
                    else
                    {
                        Session["monthFilter1"] = "0";
                        Session["monthFilter2"] = "0";
                        Session["sectorFilter"] = "";
                        Session["yearFilter1"] = yearFilter1;
                        Session["yearFilter2"] = yearFilter2;
                    }
                    
                }
                
            }
            else
            {
                Session["monthFilter1"] = "0";
                Session["monthFilter2"] = "0";
                Session["sectorFilter"] = "";
                Session["yearFilter1"] = "0";
                Session["yearFilter2"] = "0";
            }
        }
        #endregion

        public ActionResult Menu()
        {
            
            return View();
        }
        // GET: Reportes
        [Authorize]
        public ActionResult Index(string tipoSituacion, string sector, string localidad)
        {
            Business obj = new Business();
         
            #region CreacionVariables
            // Creo una lista para guardar los tipos de situaciones reportadas
            var tipoSitLst = new List<string>();
            // Creo una lista para guardar sectores reportados
            var sectorLst = new List<string>();
            
            // Creo una lista para guardar las localidades reportadas
            var localidadLst = new List<string>();

            #endregion

            #region RellenadoVariables
            //Hago query en la tabla reportes en donde obtengo los tipos de situaciones reportados y los guardo en una variable
            var tipoSitQry = from a in db.Reportes
                             select a.situacion;
            //Hago query en la tabla reportes en donde obtengo los sectores reportados
            var sectorQry = from a in db.Reportes
                            select a.sector;

            // Hago query en la tabla reportes obteniendo las localidades reportadas
            var localidadQry = from a in db.Reportes
                               select a.localidad;

            // Relleno la lista con los tipos de situaciones no repetidas de los que se han hecho reportes
            tipoSitLst.AddRange(tipoSitQry.Distinct());
            ViewBag.tipoSituacion = new SelectList(tipoSitLst);

            // Relleno la lista con los sectores no repetidos en los que se han hecho reportes
            sectorLst.AddRange(sectorQry.Distinct());
            ViewBag.sector = new SelectList(sectorLst);

            // Relleno la lista con las localidades no repetidas
            localidadLst.AddRange(localidadQry.Distinct());
            ViewBag.localidad = new SelectList(localidadLst);
            // Guardo en variable los reportes del tipo y sector indicados, si se indico. Si no se indico retorno todo los reportes no resueltos

            var listaReportes = from a in db.Reportes
                                where a.estatus == "1"
                                select a;
            #endregion

            #region CombinacionReportes
            // Reportes por localidad y tipo situacion
            if (!String.IsNullOrEmpty(tipoSituacion) && !String.IsNullOrEmpty(localidad))
            {
                listaReportes = from a in db.Reportes
                                where a.situacion == tipoSituacion &&
                                      a.localidad == a.localidad &&
                                      a.estatus == "1"
                                select a;

            }
            // Reportes por sector y tipo situacion
            else if (!String.IsNullOrEmpty(tipoSituacion) && !String.IsNullOrEmpty(sector))
            {
                listaReportes = from a in db.Reportes
                                where a.situacion == tipoSituacion &&
                                      a.sector == sector &&
                                      a.estatus == "1"
                                select a;

            }
            //Reportes por tipo situacion
            else if (!String.IsNullOrEmpty(tipoSituacion))
            {
                listaReportes = from a in db.Reportes
                                where a.situacion == tipoSituacion &&
                                      a.estatus == "1"
                                select a;

            }
            //Reportes por localidades
            else if (!String.IsNullOrEmpty(localidad))
            {
                listaReportes = from a in db.Reportes
                                where a.localidad == localidad &&
                                      a.estatus == "1"
                                select a;

            }
            //Reportes por sectores
            else if (!String.IsNullOrEmpty(sector))
            {
                listaReportes = from a in db.Reportes
                                where a.sector == sector &&
                                      a.estatus == "1"
                                select a;

            }
            #endregion

            ViewBag.Latitud = 18.523471;
            ViewBag.Longitud = -69.8746229;
            ViewBag.coordenadas = listaReportes;
            
            
            return View(listaReportes);
        }
        [Authorize]
        public ActionResult ZonasMasAfectadas(string sector)
        {
            // Creo una lista1, lista2 para guardar cadenas

            var sectorLst = new List<string>();

            //Hago query en la tabla reportes en donde obtengo los sectores en los que se han reportado situaciones y los guardo en una variable
            var sectorQry = from a in db.Reportes
                            select a.sector;

            // Relleno la lista con los sectores no repetidos en los que se han hecho reportes
            sectorLst.AddRange(sectorQry.Distinct());
            ViewBag.sector = new SelectList(sectorLst);
            // Guardo en variable los reportes del tipo y sector indicados, si se indico. Si no se indico retorno todo los reportes no resueltos

            //var listaReportes = from a in db.Reportes
            //                    where a.estatus == "1"
            //                    select a;

            List<Reporte> listaReportes = (from a in db.Reportes
                                           where a.estatus == "1"
                                           select a).ToList();


            if (!String.IsNullOrEmpty(sector))
            {
                listaReportes = (from a in db.Reportes
                                 where a.sector == sector &&
                                       a.estatus == "1"
                                 select a).ToList();

            }
            
            ViewBag.Latitud = 18.523471;
            ViewBag.Longitud = -69.8746229;
            ViewBag.coordenadas = listaReportes;

                         
            return View(listaReportes);
        }

        // GET: Reportes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reporte reporte = db.Reportes.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }
        
        /**
         * Envia una lista de reportes no resueltos en formato Json
         * 
         * @return Lista de reportes no resueltos
         */
        public JsonResult getReports()
        {
            // Obtengo los reportes que no han sido resueltos
            var n = from a in db.Reportes
                    where a.estatus == "1"
                    select a;

            // Envio lista de reportes

            return Json(n, JsonRequestBehavior.AllowGet);

        }
        /**
         * Envia una lista de reportes de un usuario especificado
         * 
         * @return Lista de reportes no resueltos
         */
        [Route("Reportes/getReportsUser/{username}/{contrasena}")]
        public JsonResult getReportsUser(string userName, string contrasena)
        {
            
            //Verifico si el usuario y contrasena son validos
            int usuario = (from a in db.Colaboradores
                          where a.Password == contrasena &&
                                a.nombreUsuario == userName
                          select a).ToList().Count;
            //Verifico si el usuario y contrasena son validos
            if (usuario == 1)
            {
                var rep = from a in db.Reportes
                          where a.userName == userName
                          select new { userName = a.userName,
                                       situacion = a.situacion,
                                       sector = a.sector,
                                       estatus = a.estatus,
                                       fechaCreacion = a.fechaCreacion.Day.ToString() + "/" + a.fechaCreacion.Month.ToString() + "/" + a.fechaCreacion.Year.ToString(),

                          };
                return Json(rep, JsonRequestBehavior.AllowGet);
            }


            return Json(0, JsonRequestBehavior.AllowGet);
            
        }
        
        public async Task<JsonResult>  Crear(int numReporteUsr, string userName, string situacion, double longitud, double latitud, string sector1)
        {
          await RetrieveFormatedAddress(latitud.ToString(),longitud.ToString());
          string s = sector;
          
            //Tomo el username y verifico si el colaborador existe
          var colabExiste = (from a in db.Colaboradores
                             where a.nombreUsuario == userName
                             select a).ToList().Count;
          // Se busca en la BD el numero de reporte relativo al usuario enviado
          //var numReportExist = (from a in db.Reportes
          //                      where a.numReporteUsr == numReporteUsr &&
          //                            a.userName == userName
          //                      select a).ToList().Count;
            
           
            ///* Se verifica que el colaborador exista y que la secuencia de reporte relativo
            // Al usuario no este repetido*/
          if (colabExiste == 1)
          {
              Reporte report = new Reporte(numReporteUsr, userName, situacion, longitud, latitud, pais, localidad, sector, calle);
              db.Reportes.Add(report);
              db.SaveChanges();
              return Json(1, JsonRequestBehavior.AllowGet);
          }
          else
          {
              return Json(0, JsonRequestBehavior.AllowGet);
          }

        }

        #region Geocode
        public async Task RetrieveFormatedAddress(string lat, string lng)
        {
          var result=  Task.Factory.StartNew(()=> downloadLinkReverseGeoCode(lat,lng));
          await Task.WhenAll(result);
          Thread.Sleep(2000);
          string s = sector;

        }
        private void downloadLinkReverseGeoCode(string lat, string lng)
        {
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                wc.DownloadStringAsync(new Uri(requestUri));


            }
        
        }
        public void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var xmlElm = XElement.Parse(e.Result);
            IEnumerable<XElement> data = xmlElm.Elements();

            Console.WriteLine("List of all Atributes of a ADDRESS");

            IEnumerable<XElement> data1 = (from a in data.Descendants()
                                           where a.Name == "address_component"
                                           select a).ToList().Take(5);
            
            calle =  data1.ElementAt(0).Element("long_name").Value.ToString();
            sector = data1.ElementAt(1).Element("long_name").Value;
            localidad = data1.ElementAt(2).Element("long_name").Value;
            pais = data1.ElementAt(4).Element("long_name").Value;

            
            
            

           
        }
        #endregion
        // GET: Reportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reportes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "numReporte,numReporteUsr,userName,situacion,longitud,latitud")] Reporte reporte)
        {
            if (ModelState.IsValid)
            {
                db.Reportes.Add(reporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reporte);
        }

        // GET: Reportes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reporte reporte = db.Reportes.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // POST: Reportes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "numReporte,numReporteUsr,userName,Descripcion,situacion,ubicacion,longitud,latitud,FotoUrl,Comentarios,estatus,fechaCreacion,fechaCorreccion")] Reporte reporte)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(reporte).State = System.Data.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(reporte);
        //}

        // GET: Reportes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reporte reporte = db.Reportes.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // POST: Reportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reporte reporte = db.Reportes.Find(id);
            db.Reportes.Remove(reporte);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
