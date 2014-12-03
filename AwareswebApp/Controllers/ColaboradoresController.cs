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
    public class ColaboradoresController : Controller
    {
        private DbModels db = new DbModels();

        public ActionResult Menu()
        {
            return View();
        }
        // GET: Colaboradores
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Colaboradores.ToList());
        }

        public JsonResult Validate(string username, string password)
        {
            // Se verifica si el colaborador ingresado, esta creado
            // en la base de datos
            var colab = from n in db.Colaboradores
                        where n.nombreUsuario == username &&
                              n.Password == password
                        select n;
                        
            if(colab.Count() == 1)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            
        }
        // GET: Colaboradores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colaborador colaborador = db.Colaboradores.Find(id);
            if (colaborador == null)
            {
                return HttpNotFound();
            }
            return View(colaborador);
        }
        // GET: Colaboradores/Create
        public JsonResult Crear(string usuario, string email, string password)
        {
            // Se verifica si el usuario o el correo electronico existen
            var user = from n in db.Colaboradores
                       where n.nombreUsuario == usuario ||
                             n.Email == email
                       select n;

            
            // Si el usuario no existe, crealo
            if (user.Count() == 0)
            {
                Colaborador colab = new Colaborador(usuario, email, password);
                db.Colaboradores.Add(colab);
                db.SaveChanges();
                
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else{
                return Json(0, JsonRequestBehavior.AllowGet);
            }

           
        }

        // GET: Colaboradores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colaboradores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nombreUsuario,Email,Password")] Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                db.Colaboradores.Add(colaborador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(colaborador);
        }

       
        // GET: Colaboradores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colaborador colaborador = db.Colaboradores.Find(id);
            if (colaborador == null)
            {
                return HttpNotFound();
            }
            return View(colaborador);
        }

        // POST: Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Colaborador colaborador = db.Colaboradores.Find(id);
            db.Colaboradores.Remove(colaborador);
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
