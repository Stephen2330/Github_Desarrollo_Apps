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
    public class MotoresController : Controller
    {
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Motores
		public ActionResult Index()
        {
            return View(db.TBL_Motor.ToList());
        }


		[HttpGet]
		public ActionResult Index(int? page, string searchText)
		{

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			IEnumerable<TBL_Motor> motores;

			motores = db.TBL_Motor.AsQueryable();

			if (!string.IsNullOrEmpty(searchText))
			{
				motores = motores.Where(m => m.TC_Descripcion.Contains(searchText));
			}
			int totalItems = motores.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;

			ViewBag.CurrentFilter = searchText;

			var motoresOrdenadas = motores.OrderBy(m => m.TC_Descripcion);
			var motoresPaginas = motoresOrdenadas.Skip((pageNumber - 1) * pageSize).Take(pageSize);


			return View(motoresPaginas);
			//return View(marcasOrdenadas.ToList());
			// Paginate the marcas

		}



		// GET: Motores/Details/5
		public ActionResult Details(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			TBL_Motor motor = db.TBL_Motor.Find(id);
			if (motor == null)
			{
				return HttpNotFound();
			}
			return View(motor);
		}

        // GET: Motores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Motores/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_idMotor, TC_Descripcion")] TBL_Motor motor)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Motor.Add(motor);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(motor);
		}

        // GET: Motores/Edit/5
        public ActionResult Edit(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Motor motor = db.TBL_Motor.Find(id);
			if (motor == null)
			{
				return HttpNotFound();
			}
			return View(motor);
		}

        // POST: Motores/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idMotor, TC_Descripcion")] TBL_Motor motor)
        {
			if (ModelState.IsValid)
			{
				db.Entry(motor).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(motor);
		}

        // GET: Motores/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Motores/Delete/5
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
