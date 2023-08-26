using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;
using PagedList.Mvc;
using PagedList;
using System.Drawing.Printing;
using System.Drawing;

namespace Ventas_Vehiculos.Controllers
{
    public class MarcasController : Controller
    {
        private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Marcas
		public ActionResult Index()
        {
            return View(db.TBL_Marca.ToList());
        }

		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Marca> marcas;
			
			 marcas = db.TBL_Marca.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				marcas = marcas.Where(m => m.TC_Descripcion.Contains(searchText));
			}
			int totalItems = marcas.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;
			ViewBag.CurrentFilter = searchText;

			var marcasOrdenadas = marcas.OrderBy(m => m.TC_Descripcion);
			var marcasPaginas = marcasOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);

			
			return View(marcasPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}

		

		// GET: Marcas/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TBL_Marca marca = db.TBL_Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idMarca, TC_Descripcion")] TBL_Marca marca)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Marca.Add(marca);
                db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(marca);
		}

		// GET: Marcas/Edit/5
		public ActionResult Edit(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Marca marca = db.TBL_Marca.Find(id);
			if (marca == null)
			{
				return HttpNotFound();
			}
			return View(marca);
		}

		// POST: Marcas/Edit/5
		[HttpPost]
		public ActionResult Edit([Bind(Include = "TN_idMarca,TC_Descripcion")] TBL_Marca marca)
		{
			if (ModelState.IsValid)
			{
				db.Entry(marca).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(marca);
		}

		// GET: Marcas/Delete/5
		public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Marcas/Delete/5
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

		


	}//class
}//namespace
