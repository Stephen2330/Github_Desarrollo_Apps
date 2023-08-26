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
    public class DisennosController : Controller
    {
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Disennos
		public ActionResult Index()
        {
            return View(db.TBL_Disenno.ToList());
        }


		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Disenno> disennos;

			disennos = db.TBL_Disenno.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				disennos = disennos.Where(m => m.TC_Descripcion.Contains(searchText));
			}
			int totalItems = disennos.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;

			ViewBag.CurrentFilter = searchText;

			var disennosOrdenadas = disennos.OrderBy(m => m.TC_Descripcion);
			var disennosPaginas = disennosOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(disennosPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}


		// GET: Disennos/Details/5
		public ActionResult Details(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			TBL_Disenno disenno = db.TBL_Disenno.Find(id);
			if (disenno == null)
			{
				return HttpNotFound();
			}
			return View(disenno);
		}

        // GET: Disennos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disennos/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idDisenno, TC_Descripcion")] TBL_Disenno disenno)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Disenno.Add(disenno);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(disenno);
		}

        // GET: Disennos/Edit/5
        public ActionResult Edit(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Disenno disenno = db.TBL_Disenno.Find(id);
			if (disenno == null)
			{
				return HttpNotFound();
			}
			return View(disenno);
		}

        // POST: Disennos/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idDisenno, TC_Descripcion")] TBL_Disenno disenno)
        {
			if (ModelState.IsValid)
			{
				db.Entry(disenno).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(disenno);
		}

        // GET: Disennos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Disennos/Delete/5
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
