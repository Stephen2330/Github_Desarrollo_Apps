using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;

namespace Ventas_Vehiculos.Controllers
{
    public class ModelosController : Controller
    {
        private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
        // GET: Modelos
        public ActionResult Index()
        {
            return View(db.TBL_Modelo.ToList());
        }

        // GET: Modelos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            TBL_Modelo modelo = db.TBL_Modelo.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }



		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Modelo> modelos;

			modelos = db.TBL_Modelo.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				modelos = modelos.Where(m => m.TC_Descripcion.Contains(searchText));
			}
			int totalItems = modelos.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;

			ViewBag.CurrentFilter = searchText;

			var modelosOrdenadas = modelos.OrderBy(m => m.TC_Descripcion);
			var modelosPaginas = modelosOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(modelosPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}


		// GET: Modelos/Create
		public ActionResult Create()
        {
            return View();
        }

        // POST: Modelos/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idModelo, TC_Descripcion")] TBL_Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Modelo.Add(modelo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelo);

        }

        // GET: Modelos/Edit/5
        public ActionResult Edit(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Modelo modelo = db.TBL_Modelo.Find(id);
			if (modelo == null)
			{
				return HttpNotFound();
			}
			return View(modelo);
		}

        // POST: Modelos/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idModelo, TC_Descripcion")] TBL_Modelo modelo)
        {
			if (ModelState.IsValid)
			{
				db.Entry(modelo).State = EntityState.Modified;
                db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(modelo);
		}

        // GET: Modelos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Modelos/Delete/5
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
