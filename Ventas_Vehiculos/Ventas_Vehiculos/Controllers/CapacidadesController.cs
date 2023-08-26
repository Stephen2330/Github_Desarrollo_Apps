using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;

namespace Ventas_Vehiculos.Controllers
{
    public class CapacidadesController : Controller
    {
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Capacidades
		public ActionResult Index()
        {
            return View(db.TBL_Capacidad.ToList());
        }


		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Capacidad> capacidades;

			capacidades = db.TBL_Capacidad.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				capacidades = capacidades.Where(m => m.TC_Descripcion.Contains(searchText));
			}

			int totalItems = capacidades.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;
			ViewBag.CurrentFilter = searchText;

			var capacidadesOrdenadas = capacidades.OrderBy(m => m.TC_Descripcion);
			var capacidadesPaginas = capacidadesOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(capacidadesPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}


		// GET: Capacidades/Details/5
		public ActionResult Details(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			TBL_Capacidad capacidad = db.TBL_Capacidad.Find(id);
			if (capacidad == null)
			{
				return HttpNotFound();
			}
			return View(capacidad);
		}

        // GET: Capacidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Capacidades/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idCapacidad, TC_Descripcion")] TBL_Capacidad capacidad)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Capacidad.Add(capacidad);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(capacidad);
		}

        // GET: Capacidades/Edit/5
        public ActionResult Edit(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Capacidad capacidad = db.TBL_Capacidad.Find(id);
			if (capacidad == null)
			{
				return HttpNotFound();
			}
			return View(capacidad);
		}

        // POST: Capacidades/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idCapacidad, TC_Descripcion")] TBL_Capacidad capacidad)
        {
			if (ModelState.IsValid)
			{
				db.Entry(capacidad).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(capacidad);
		}

        // GET: Capacidades/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Capacidades/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
