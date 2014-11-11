using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AwareswebApp.Models;


namespace AwareswebApp.Controllers
{
    public class ConsumosController : Controller
    {
        private DbModels db = new DbModels();

        /**
         * Obtiene una lista de los consumos de los colaboradores filtrado por
         * usuario, año y mes
         * @param usuario Nombre de usuario del colaborador
         * @param anio    Anio del consumo a verificar
         * @param mes     Mes  del consumo a verificar
         * 
         * @return        La lista de los consumos filtrados por usuario, anio y mes
         * 
         */
        public void getHistorialConsumo(string usuario, string mes, string year)
        {
            
            
            #region Algoritmo
            /*
             
              Si me indicaron el anio hacer     
             
                  Si me indican el usuario, mes
                  Retornar el consumo filtrado por usuario, mes y anio
             
                  En cambio si solo me indican mes
                  Retornar el consumo filtrado por mes y anio de cada usuario
             
                  En cambio si solo me indican  el anio
                  Retornar el consumo de cada usuario por anio
             fin-si
             Sino
              Retornar el consumo del usuario 0
             
             */
            #endregion
            if (!string.IsNullOrEmpty(year))
            {
                int y = Convert.ToInt32(year);
                //Obtiene el consumo filtrado por mes, anio y usuario
                if (!string.IsNullOrEmpty(mes) && !string.IsNullOrEmpty(usuario))
                {
                    int m = Convert.ToInt32(mes);

                    var cons = from a in db.Consumos
                               where a.fechaCreacion.Month == m && a.fechaCreacion.Year == y && a.UsernameColaborador == usuario
                               group a by a.UsernameColaborador into b
                               select new HistGenConsumoViewModel { username = b.Key, consumo = b.Sum(a => a.lectura) };
                    ViewBag.consumos= cons.ToList();
                }
                    //Obtiene el consumo por anio y mes
                else if(!string.IsNullOrEmpty(mes) )
                {
                    int m = Convert.ToInt32(mes);

                    var cons = from a in db.Consumos
                               where a.fechaCreacion.Month == m && a.fechaCreacion.Year == y 
                               group a by a.UsernameColaborador into b
                               select new HistGenConsumoViewModel { username = b.Key, consumo = b.Sum(a => a.lectura) };
                    ViewBag.consumos = cons.ToList();
                }

                else if (!string.IsNullOrEmpty(usuario))
                {
                    var cons = from a in db.Consumos
                               where a.fechaCreacion.Year == y && a.UsernameColaborador == usuario
                               group a by a.UsernameColaborador into b
                               select new HistGenConsumoViewModel { username = b.Key, consumo = b.Sum(a => a.lectura) };
                    ViewBag.consumos = cons.ToList();
                }
                    //Obtiene el consumo por anio y usuario
                else 
                {
                    var cons = from a in db.Consumos
                               where a.fechaCreacion.Year == y
                               group a by a.UsernameColaborador into b
                               select new HistGenConsumoViewModel { username = b.Key, consumo = b.Sum(a => a.lectura) };
                    ViewBag.consumos = cons.ToList();
                }

            }
            else
            {
               var cons = from a in db.Consumos
                          where a.UsernameColaborador == "1"
                          group a by a.UsernameColaborador into b
                          select new HistGenConsumoViewModel { username = b.Key, consumo = b.Sum(a => a.lectura) };

               ViewBag.consumos = cons.ToList();
            
            }

           
        }
        
        // GET: Consumos
        public void Receive(string userNameColaborador, string lecturaConsumo)
        {
            // Se recibe las lecturas
            String l = lecturaConsumo;

            // Se separan las lecturas por usuario
            String[] lecturas = l.Split('-');

            
            //Se recorren las lecturas
            for (int i = 0; i < lecturas.Length; i++)
            {
                // Se separan los datos usuario y lectura
                String[] datos = lecturas[i].Split(',');
                string colab = datos[1];
                double lectura = Convert.ToDouble( datos[0]);

                Consumo c = new Consumo(colab, lectura);
                db.Consumos.Add(c);
                
            }
            
            db.SaveChanges();
            
        }

        public ActionResult Menu()
        {
            return View();
        }

        /**
         * Envia una lista de reportes de un usuario especificado
         * 
         * @return Lista de reportes no resueltos
         */
        [Route("Consumos/getConsumesUser/{username}/{contrasena}")]
        public JsonResult getConsumesUser(string userName, string contrasena)
        {

            //Verifico si el usuario y contrasena son validos
            int usuario = (from a in db.Colaboradores
                           where a.Password == contrasena &&
                                 a.nombreUsuario == userName
                           select a).ToList().Count;
            //Verifico si el usuario y contrasena son validos
            if (usuario == 1)
            {
                var rep = from a in db.Colaboradores
                          where a.nombreUsuario == userName
                          select a;
                return Json(rep, JsonRequestBehavior.AllowGet);
            }


            return Json(0, JsonRequestBehavior.AllowGet);

        }
        
        // GET: Consumos

        // Historial de Consumos General

        public ActionResult historialConsumosGeneral(string colabFilter, string monthFilter, string yearFilter)
        {
            string[] month = { " 1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            // Creo una lista para guardar colaboradores
            var ColabLst = new List<string>();
            var monthLst = new List<string>();
            var yearLst = new List<string>();
            //Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable
            var ColabQry = from a in db.Consumos
                           select a.UsernameColaborador;
            var yearQry = from a in db.Consumos
                          select a.fechaCreacion.Year.ToString();


            // Relleno la lista con los usuarios no repetidos que han hecho consumos
            ColabLst.AddRange(ColabQry.Distinct());
            ViewBag.colabFilter = new SelectList(ColabLst);

            // Los anios en los que se han realizado consumos en el dropdownlist
            yearLst.AddRange(yearQry.Distinct());
            ViewBag.yearFilter = new SelectList(yearLst);

            //Agrego los meses al dropdownLst
            monthLst.AddRange(month);
            ViewBag.monthFilter = new SelectList(monthLst);
            // Guardo en variable los consumos hechos por el usuario indicado, si se indico, si no se indico no desplego nada.

            getHistorialConsumo(colabFilter, monthFilter, yearFilter);

            return View();
        }

        // Historial de Consumos Detallado por mes

        public ActionResult historialConsumosDetPorMes(string colabFilter, string yearFilter)
        {
            // Creo una lista para guardar colaboradores
            var ColabLst = new List<string>();
            var yearLst = new List<string>();

            //Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable
            var ColabQry = from a in db.Consumos
                           select a.UsernameColaborador;

            var yearQry = from a in db.Consumos
                          select a.fechaCreacion.Year.ToString();

            // Relleno la lista con los usuarios no repetidos que han hecho consumos
            ColabLst.AddRange(ColabQry.Distinct());
            ViewBag.colabFilter = new SelectList(ColabLst);

            // Los anios en los que se han realizado consumos en el dropdownlist
            yearLst.AddRange(yearQry.Distinct());
            ViewBag.yearFilter = new SelectList(yearLst);

            int y = Convert.ToInt32(yearFilter);
            if(string.IsNullOrEmpty(colabFilter))
            {

                var consumos2 = (from a in db.Consumos
                                 where  a.fechaCreacion.Year == y
                                 group a by new { a.UsernameColaborador, a.fechaCreacion.Month, a.fechaCreacion.Year } into b
                                 select new HistDetConsumoViewModel
                                 {
                                     username = b.Key.UsernameColaborador,
                                     month = b.Key.Month,
                                     year = b.Key.Year,
                                     consumo = b.Sum(a => a.lectura)
                                 });
                ViewBag.consumos = consumos2.ToList();
            }
            else
            {
                var consumos2 = (from a in db.Consumos
                                 where a.UsernameColaborador == colabFilter && a.fechaCreacion.Year == y
                                 group a by new { a.UsernameColaborador, a.fechaCreacion.Month, a.fechaCreacion.Year } into b
                                 select new HistDetConsumoViewModel
                                 {
                                     username = b.Key.UsernameColaborador,
                                     month = b.Key.Month,
                                     year = b.Key.Year,
                                     consumo = b.Sum(a => a.lectura)
                                 });
                ViewBag.consumos = consumos2.ToList();
            }

            
            return View();
        
        }
        public ActionResult Index(string colabFilter, string monthFilter, string yearFilter)
        {
            //string[] month = { " 1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
         
            //// Creo una lista para guardar colaboradores
            //var ColabLst = new List<string>();
            //var monthLst = new List<string>();
            //var yearLst = new List<string>();
            ////Hago query en la tabla consumos en donde obtengo los usuarios que han realizado consumos y los guardo en una variable
            //var ColabQry = from a in db.Consumos
            //               select a.UsernameColaborador;
            //var yearQry = from a in db.Consumos
            //              select a.fechaCreacion.Year.ToString();


            //// Relleno la lista con los usuarios no repetidos que han hecho consumos
            //ColabLst.AddRange(ColabQry.Distinct());
            //ViewBag.colabFilter = new SelectList(ColabLst);

            //// Los anios en los que se han realizado consumos en el dropdownlist
            //yearLst.AddRange(yearQry.Distinct());
            //ViewBag.yearFilter = new SelectList(yearLst);

            ////Agrego los meses al dropdownLst
            //monthLst.AddRange(month);
            //ViewBag.monthFilter = new SelectList(monthLst);
            //// Guardo en variable los consumos hechos por el usuario indicado, si se indico, si no se indico no desplego nada.
                       
            //getHistorialConsumo(colabFilter, monthFilter, yearFilter);

            return View();
        }

        // GET: Consumos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo consumo = db.Consumos.Find(id);
            if (consumo == null)
            {
                return HttpNotFound();
            }
            return View(consumo);
        }

        // GET: Consumos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consumos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idConsumo,userNameColaborador,tipoConsumo,lectura,fechaCreacion")] Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                db.Consumos.Add(consumo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consumo);
        }

        // GET: Consumos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo consumo = db.Consumos.Find(id);
            if (consumo == null)
            {
                return HttpNotFound();
            }
            return View(consumo);
        }

        // POST: Consumos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idConsumo,userNameColaborador,tipoConsumo,lectura,fechaCreacion")] Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consumo);
        }

        // GET: Consumos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo consumo = db.Consumos.Find(id);
            if (consumo == null)
            {
                return HttpNotFound();
            }
            return View(consumo);
        }

        // POST: Consumos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consumo consumo = db.Consumos.Find(id);
            db.Consumos.Remove(consumo);
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
