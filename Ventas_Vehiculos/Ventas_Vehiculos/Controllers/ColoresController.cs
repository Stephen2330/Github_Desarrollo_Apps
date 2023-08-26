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
    public class ColoresController : Controller
    {
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Colores
		public ActionResult Index()
        {
            return View(db.TBL_Color.ToList());
        }


       


		// GET: Colores/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TBL_Color color = db.TBL_Color.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Color> colores;

			colores = db.TBL_Color.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				colores = colores.Where(m => m.TC_Descripcion.Contains(searchText));
			}

			int totalItems = colores.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;
			ViewBag.CurrentFilter = searchText;

			var coloresOrdenadas = colores.OrderBy(m => m.TC_Descripcion);
			var coloresPaginas = coloresOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(coloresPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}





		// GET: Colores/Create
		public ActionResult Create()
        {
            return View();
        }

        // POST: Colores/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idColor, TC_Descripcion")] TBL_Color color)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Color.Add(color);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(color);
		}

        // GET: Colores/Edit/5
        public ActionResult Edit(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Color color = db.TBL_Color.Find(id);
			if (color == null)
			{
				return HttpNotFound();
			}
			return View(color);
		}

        // POST: Colores/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idColor, TC_Descripcion")] TBL_Color color)
        {
			if (ModelState.IsValid)
			{
				db.Entry(color).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(color);
		}

        // GET: Colores/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Colores/Delete/5
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
