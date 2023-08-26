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
using PagedList.Mvc;

namespace Ventas_Vehiculos.Controllers
{
    public class ComprasController : Controller
    {
        private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();

        // GET: Compras
        public ActionResult Index()
        {
            var tBL_Compra = db.TBL_Compra.Include(t => t.TBL_Anno).Include(t => t.TBL_Capacidad).Include(t => t.TBL_Color).Include(t => t.TBL_Disenno).Include(t => t.TBL_Marca).Include(t => t.TBL_Modelo).Include(t => t.TBL_Motor).Include(t => t.TBL_Usuario).Include(t => t.TBL_Usuario1).Include(t => t.TBL_Vehiculo);
            return View(tBL_Compra.ToList());
        }


		[HttpGet]
		public ActionResult Index(string marcaFiltro, int? page)
		{
			ViewBag.MarcasSelect = new SelectList(db.TBL_Marca.OrderBy(m => m.TC_Descripcion), "TN_IdMarca", "TC_Descripcion");

			var compras = db.TBL_Compra.Include(c => c.TBL_Vehiculo)
									   .Include(c => c.TBL_Marca)
									   .OrderBy(c => c.TBL_Marca.TC_Descripcion);

			if (!string.IsNullOrEmpty(marcaFiltro) && marcaFiltro != "0")
			{
				int marcaId = int.Parse(marcaFiltro);
				compras = (IOrderedQueryable<TBL_Compra>)compras.Where(c => c.TBL_Marca.TN_IdMarca == marcaId);
			}

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			int totalItems = compras.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
			ViewBag.totalPages = totalPages;

			var orderedCompras = compras.ToList(); // Obtener todos los datos ordenados antes de la paginación
			var pagedCompras = orderedCompras.ToPagedList(pageNumber, pageSize);

			ViewBag.CurrentFilter = marcaFiltro;

			return View(pagedCompras);
		}

		// GET: Compras/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Compra tBL_Compra = db.TBL_Compra.Find(id);
            if (tBL_Compra == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Compra);
        }


		// GET: Compras/Create
		public ActionResult Create()
        {
			ViewBag.TN_IdAnno = GetSortedSelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion");
			ViewBag.TN_IdCapacidad = GetSortedSelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion");
			ViewBag.TN_IdColor = GetSortedSelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion");
			ViewBag.TN_IdDisenno = GetSortedSelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion");
			ViewBag.TN_IdMarca = GetSortedSelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion");
			ViewBag.TN_IdModelo = GetSortedSelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion");
			ViewBag.TN_IdMotor = GetSortedSelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion");
			ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario");
			ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo");
			return View();
        }
		private SelectList GetSortedSelectList<T>(IEnumerable<T> source, string valueField, string textField)
		{
			var sortedList = source.OrderBy(item => item.GetType().GetProperty(textField).GetValue(item, null))
								   .Select(item => new SelectListItem
								   {
									   Value = item.GetType().GetProperty(valueField).GetValue(item, null).ToString(),
									   Text = item.GetType().GetProperty(textField).GetValue(item, null).ToString()
								   })
								   .ToList();
			return new SelectList(sortedList, "Value", "Text");
		}

		// POST: Compras/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TN_IdCompra,TN_IdVehiculo,TN_Cantidad,TN_IdMarca,TN_IdModelo,TN_IdCapacidad,TN_IdColor,TN_IdAnno,TN_IdMotor,TN_IdDisenno,TN_IdUsuario")] TBL_Compra tBL_Compra)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Compra.Add(tBL_Compra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Compra.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Compra.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Compra.TN_IdColor);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Compra.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Compra.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Compra.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Compra.TN_IdMotor);
            ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario", tBL_Compra.TN_IdUsuario);
            ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo", tBL_Compra.TN_IdVehiculo);
            return View(tBL_Compra);
        }

		// GET: Compras/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			TBL_Compra tBL_Compra = db.TBL_Compra.Find(id);
			if (tBL_Compra == null)
			{
				return HttpNotFound();
			}

			// Get related data
			ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario", tBL_Compra.TN_IdUsuario);
			ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo", tBL_Compra.TN_IdVehiculo);

			ViewBag.TN_IdAnno = GetSortedSelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Compra.TN_IdAnno);
			ViewBag.TN_IdCapacidad = GetSortedSelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Compra.TN_IdCapacidad);
			ViewBag.TN_IdColor = GetSortedSelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Compra.TN_IdColor);
			ViewBag.TN_IdDisenno = GetSortedSelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Compra.TN_IdDisenno);
			ViewBag.TN_IdMarca = GetSortedSelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Compra.TN_IdMarca);
			ViewBag.TN_IdModelo = GetSortedSelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Compra.TN_IdModelo);
			ViewBag.TN_IdMotor = GetSortedSelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Compra.TN_IdMotor);


			return View(tBL_Compra);
		}



		// Agrega esta función en tu controlador para ordenar las listas de selección
		private SelectList GetSortedSelectList<T>(IEnumerable<T> source, string valueField, string textField, object selectedValue = null)
		{
			var sortedList = source.OrderBy(item => item.GetType().GetProperty(textField).GetValue(item, null))
								   .Select(item => new SelectListItem
								   {
									   Value = item.GetType().GetProperty(valueField).GetValue(item, null).ToString(),
									   Text = item.GetType().GetProperty(textField).GetValue(item, null).ToString(),
									   Selected = (selectedValue != null && item.GetType().GetProperty(valueField).GetValue(item, null).ToString() == selectedValue.ToString())
								   })
								   .ToList();
			return new SelectList(sortedList, "Value", "Text");
		}


		// POST: Compras/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "TN_IdCompra,TN_IdVehiculo,TN_Cantidad,TN_IdMarca,TN_IdModelo,TN_IdCapacidad,TN_IdColor,TN_IdAnno,TN_IdMotor,TN_IdDisenno,TN_IdUsuario")] TBL_Compra tBL_Compra)
		{
			if (ModelState.IsValid)
			{
				db.Entry(tBL_Compra).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			// Get related data
			ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_Nombre", tBL_Compra.TN_IdUsuario);
			ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_Placa", tBL_Compra.TN_IdVehiculo);

			ViewBag.TN_IdAnno = GetSortedSelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Compra.TN_IdAnno);
			ViewBag.TN_IdCapacidad = GetSortedSelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Compra.TN_IdCapacidad);
			ViewBag.TN_IdColor = GetSortedSelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Compra.TN_IdColor);
			ViewBag.TN_IdDisenno = GetSortedSelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Compra.TN_IdDisenno);
			ViewBag.TN_IdMarca = GetSortedSelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Compra.TN_IdMarca);
			ViewBag.TN_IdModelo = GetSortedSelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Compra.TN_IdModelo);
			ViewBag.TN_IdMotor = GetSortedSelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Compra.TN_IdMotor);


			return View(tBL_Compra);
		}       // GET: Compras/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Compra tBL_Compra = db.TBL_Compra.Find(id);
            if (tBL_Compra == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_Compra tBL_Compra = db.TBL_Compra.Find(id);
            db.TBL_Compra.Remove(tBL_Compra);
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
