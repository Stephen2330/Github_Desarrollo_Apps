using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;

namespace Ventas_Vehiculos.Controllers
{
	public class AnnosController : Controller
	{
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Annos
		public ActionResult Index()
		{
			return View(db.TBL_Anno.ToList());
		}


		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Anno> annos;

			annos = db.TBL_Anno.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				annos = annos.Where(m => m.TC_Descripcion.Contains(searchText));
			}
			int totalItems = annos.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;

			ViewBag.CurrentFilter = searchText;

			var annosOrdenadas = annos.OrderBy(m => m.TC_Descripcion);
			var annosPaginas = annosOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(annosPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}




		// GET: Annos/Details/5
		public ActionResult Details(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			TBL_Anno anno = db.TBL_Anno.Find(id);
			if (anno == null)
			{
				return HttpNotFound();
			}
			return View(anno);
		}

		// GET: Annos/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Annos/Create
		[HttpPost]
		public ActionResult Create([Bind(Include = "TN_idAnno, TC_Descripcion")] TBL_Anno anno)
		{
			if (ModelState.IsValid)
			{
				db.TBL_Anno.Add(anno);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(anno);
		}

		// GET: Annos/Edit/5
		public ActionResult Edit(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Anno anno = db.TBL_Anno.Find(id);
			if (anno == null)
			{
				return HttpNotFound();
			}
			return View(anno);
		}

		// POST: Annos/Edit/5
		[HttpPost]
		public ActionResult Edit([Bind(Include = "TN_idAnno, TC_Descripcion")] TBL_Anno anno)
		{
			if (ModelState.IsValid)
			{
				db.Entry(anno).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(anno);
		}

		// GET: Annos/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Annos/Delete/5
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
