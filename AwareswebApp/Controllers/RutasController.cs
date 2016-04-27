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
        public ActionResult Create(string correctorFilter)
        {
            Business obj = new Business();

            var CorrectorLst = new List<string>();

            
            //Hago query en la tabla reportes en donde obtengo los sectores en los que se han reportado situaciones y los guardo en una variable
            var CorrectorQry = from a in db.Colaboradores
                               where a.tipoUsuario == "Corrector"
                            select a.nombreUsuario;

            CorrectorLst.AddRange(CorrectorQry.Distinct());
            ViewBag.correctorFilter = new SelectList(CorrectorLst);

            var listaReportes = from a in db.Reportes
                                where a.estatus == "No resuelto"
                                select a;
           

            ViewBag.Latitud = 18.523471;
            ViewBag.Longitud = -69.8746229;
            ViewBag.coordenadas = listaReportes;


            return View(listaReportes);
            
        }
        [HttpPost]
        [Route("Rutas/SaveRutas/")]
        public ActionResult SaveRutas(List<string> model)
        {
            //Obtener lista de reportes

            //Buscar los id de rutas distintos de la tabla rutas

            var rutasid = (from a in db.Reportes_Ruta
                           select a.numRuta).Distinct().Max();

            //Almacenar el mayor y sumarle 1
            rutasid += 1;

            // AGregar reporte al encabezado
            /*
             * Buscar los id de rutas distintos de la tabla rutas

            Almacenar el mayor y sumarle 1

            insertar los id de los reportes con su id de ruta
             */
            var reportes_ruta = from a in model
                                select  new Reportes_Ruta
                                {
                                    numReporte = Convert.ToInt32(a),
                                    numRuta = rutasid,
                                    fechaCreacion = DateTime.Now
                                };
            // Se insertan los registros en la base de datos
            db.Reportes_Ruta.AddRange(reportes_ruta);
            db.SaveChanges();


            return View();
        }
    }
}