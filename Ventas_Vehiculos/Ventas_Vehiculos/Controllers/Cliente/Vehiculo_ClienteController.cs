using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;

namespace Ventas_Vehiculos.Controllers.Cliente
{
    public class Vehiculo_ClienteController : Controller
    {
        private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();

        // GET: Vehiculo_Cliente
        public ActionResult Index()
        {
            var tBL_Vehiculo = db.TBL_Vehiculo.Include(t => t.TBL_Anno).Include(t => t.TBL_Capacidad).Include(t => t.TBL_Color).Include(t => t.TBL_Disenno).Include(t => t.TBL_Marca).Include(t => t.TBL_Modelo).Include(t => t.TBL_Motor);
            return View(tBL_Vehiculo.ToList());
        }

		[HttpGet]
		public ActionResult Index(int? marca, int? page)
		{
			ViewBag.Marcas = db.TBL_Marca.OrderBy(m => m.TC_Descripcion).ToList();
			ViewBag.MarcasSelect = new SelectList(ViewBag.Marcas, "TN_IdMarca", "TC_Descripcion");
			var vehiculos = db.TBL_Vehiculo.Include(v => v.TBL_Marca).Include(v => v.TBL_Modelo).Include(v => v.TBL_Capacidad).Include(v => v.TBL_Color).Include(v => v.TBL_Anno).Include(v => v.TBL_Motor).Include(v => v.TBL_Disenno);

			if (marca.HasValue)
			{
				vehiculos = vehiculos.Where(v => v.TBL_Marca.TN_IdMarca == marca);
			}
			// Aplicar el orden alfabético por marca
			vehiculos = vehiculos.OrderBy(v => v.TBL_Marca.TC_Descripcion);

			int pageNumber = page ?? 1; // Si el valor de page es nulo, asumimos la primera página
			int pageSize = 10; // Tamaño de página
			int totalItems = vehiculos.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;
			// Obtener la página actual de vehículos
			var vehiculosPaginados = vehiculos.ToPagedList(pageNumber, pageSize);

			ViewBag.PageNumber = pageNumber;

			return View(vehiculosPaginados);
		}






		// GET: Vehiculo_Cliente/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Vehiculo tBL_Vehiculo = db.TBL_Vehiculo.Find(id);
            if (tBL_Vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Vehiculo);
        }

        // GET: Vehiculo_Cliente/Create
        public ActionResult Create()
        {
            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion");
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion");
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion");
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion");
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion");
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion");
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion");
            return View();
        }

        // POST: Vehiculo_Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TN_IdVehiculo,TN_IdMarca,TN_IdModelo,TN_IdColor,TN_IdAnno,TN_IdCapacidad,TN_IdMotor,TN_IdDisenno")] TBL_Vehiculo tBL_Vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Vehiculo.Add(tBL_Vehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Vehiculo.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Vehiculo.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Vehiculo.TN_IdColor);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Vehiculo.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Vehiculo.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Vehiculo.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Vehiculo.TN_IdMotor);
            return View(tBL_Vehiculo);
        }

        // GET: Vehiculo_Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Vehiculo tBL_Vehiculo = db.TBL_Vehiculo.Find(id);
            if (tBL_Vehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Vehiculo.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Vehiculo.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Vehiculo.TN_IdColor);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Vehiculo.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Vehiculo.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Vehiculo.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Vehiculo.TN_IdMotor);
            return View(tBL_Vehiculo);
        }

        // POST: Vehiculo_Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TN_IdVehiculo,TN_IdMarca,TN_IdModelo,TN_IdColor,TN_IdAnno,TN_IdCapacidad,TN_IdMotor,TN_IdDisenno")] TBL_Vehiculo tBL_Vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_Vehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Vehiculo.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Vehiculo.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Vehiculo.TN_IdColor);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Vehiculo.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Vehiculo.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Vehiculo.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Vehiculo.TN_IdMotor);
            return View(tBL_Vehiculo);
        }

        // GET: Vehiculo_Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Vehiculo tBL_Vehiculo = db.TBL_Vehiculo.Find(id);
            if (tBL_Vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Vehiculo);
        }

        // POST: Vehiculo_Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_Vehiculo tBL_Vehiculo = db.TBL_Vehiculo.Find(id);
            db.TBL_Vehiculo.Remove(tBL_Vehiculo);
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
