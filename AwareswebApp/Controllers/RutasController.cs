using AwareswebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AwareswebApp.Controllers
{
    public class RutasController : Controller
    {
        private DbModels db = new DbModels();
        // GET: Rutas
        public ActionResult Index()
        {
            return View();
        }

        /**
         * Proporciona una lista de los reportes asociados a una ruta, asignada a
         * un usuario indicado.
         * @param usrCor        Usuario al que se le asigno la ruta
         * @return              Una lista de los reportes asociados a la ruta asignada 
         *                      al usuario.
         */
        [Route("Rutas/getRoutesPerUser/{usrCor}")]
        public ActionResult getRoutesPerUser(string usrCor)
        {

            //Obtener ruta asignada a usrCor de tabla RutaH y guardarlo en numRuta
            int numRuta = 0;
             numRuta = (from a in db.RutaH
                           where a.usuario == usrCor && 
                                 a.status == "No Completada"
                           select a.Rutaid).SingleOrDefault();

            //De la tablas rutas obtener los numeros de reportes asociados a numruta

            var reportAsg = (from a in db.Reportes_Ruta
                             where a.numRuta == numRuta
                             select a.numReporte).ToList();

            // De la tabla reporte obtener los reportes asociado al usuario
            var reportes = (from r in reportAsg
                           join rep in db.Reportes on r equals rep.numReporte
                           select new 
                           {
                               rep.numReporte,
                               rep.latitud,
                               rep.longitud,
                               rep.situacion,
                               rep.sector,
                               rep.calle,
                               rep.estatus
                           }).ToList();

           

            if (reportes.Count() > 0)
            {
                return Json(reportes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            
        }

        /**
         * Pantalla para la creacion de rutas
         */
        public ActionResult Create(string tipoSituacion, string sector, string localidad, string estatus)
        {
            Business obj = new Business();


            var listaReportes = from a in db.Reportes
                                where a.estatus == "No resuelto"
                                select a;
           

            ViewBag.Latitud = 18.523471;
            ViewBag.Longitud = -69.8746229;
            ViewBag.coordenadas = listaReportes;


            return View(listaReportes);
            
        }
    }
}