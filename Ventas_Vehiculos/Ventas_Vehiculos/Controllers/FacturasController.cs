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

namespace Ventas_Vehiculos.Controllers
{
    public class FacturasController : Controller
    {
        private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();

        // GET: Facturas
        public ActionResult Index()
        {
            var tBL_Factura = db.TBL_Factura.Include(t => t.TBL_Anno).Include(t => t.TBL_Capacidad).Include(t => t.TBL_Color).Include(t => t.TBL_Compra).Include(t => t.TBL_Disenno).Include(t => t.TBL_Marca).Include(t => t.TBL_Modelo).Include(t => t.TBL_Motor).Include(t => t.TBL_Usuario).Include(t => t.TBL_Vehiculo);
            return View(tBL_Factura.ToList());
        }

		[HttpGet]
		public ActionResult Index(string marca, int? page)
		{
			ViewBag.MarcasSelect = new SelectList(db.TBL_Marca.OrderBy(m => m.TC_Descripcion), "TN_IdMarca", "TC_Descripcion", marca);

			var facturas = db.TBL_Factura.Include(f => f.TBL_Vehiculo)
										  .Include(f => f.TBL_Usuario)
										  .Include(f => f.TBL_Marca)
										  .Include(f => f.TBL_Modelo)
										  .Include(f => f.TBL_Capacidad)
										  .Include(f => f.TBL_Color)
										  .Include(f => f.TBL_Anno)
										  .Include(f => f.TBL_Motor)
										  .Include(f => f.TBL_Disenno)
										  .Include(f => f.TBL_Compra)
										  .OrderBy(f => f.TBL_Marca.TC_Descripcion)
										  .ThenBy(f => f.TN_IdFactura);

			if (!string.IsNullOrEmpty(marca) && marca != "0")
			{
				int marcaId = int.Parse(marca);
				facturas = (IOrderedQueryable<TBL_Factura>)facturas.Where(f => f.TBL_Marca.TN_IdMarca == marcaId);
			}

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			int totalItems = facturas.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
			ViewBag.totalPages = totalPages;
			var pagedFacturas = facturas.ToPagedList(pageNumber, pageSize);

			ViewBag.CurrentFilter = marca;

			return View(pagedFacturas);
		}

		// GET: Facturas/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Factura tBL_Factura = db.TBL_Factura.Find(id);
            if (tBL_Factura == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Factura);
        }

		

		// GET: Facturas/Create
		public ActionResult Create()
        {
			ViewBag.SortedMarcas = GetSortedSelectList(db.TBL_Marca);
			ViewBag.SortedModelos = GetSortedSelectList(db.TBL_Modelo);
			ViewBag.SortedCapacidades = GetSortedSelectList(db.TBL_Capacidad);
			ViewBag.SortedColores = GetSortedSelectList(db.TBL_Color);
			ViewBag.SortedAnnos = GetSortedSelectList(db.TBL_Anno);
			ViewBag.SortedMotores = GetSortedSelectList(db.TBL_Motor);
			ViewBag.SortedDisennos = GetSortedSelectList(db.TBL_Disenno);



			ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion");
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion");
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion");
            ViewBag.TN_IdCompra = new SelectList(db.TBL_Compra, "TN_IdCompra", "TN_IdCompra");
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion");
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion");
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion");
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion");
            ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario");
            ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo");
            return View();
        }

		private SelectList GetSortedSelectList(IEnumerable<TBL_Disenno> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdDisenno", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Motor> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdMotor", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Modelo> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdModelo", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Marca> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdMarca", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Anno> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdAnno", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Capacidad> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdCapacidad", "TC_Descripcion");
		}

		private SelectList GetSortedSelectList(IEnumerable<TBL_Color> items)
		{
			var sortedItems = items.OrderBy(i => i.TC_Descripcion).ToList();
			return new SelectList(sortedItems, "TN_IdColor", "TC_Descripcion");
		}



		// POST: Facturas/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TN_IdFactura,TN_IdVehiculo,TN_IdUsuario,TN_IdMarca,TN_IdModelo,TN_IdCapacidad,TN_IdColor,TN_IdAnno,TN_IdMotor,TN_IdDisenno,TN_IdCompra")] TBL_Factura tBL_Factura)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Factura.Add(tBL_Factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Factura.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Factura.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Factura.TN_IdColor);
            ViewBag.TN_IdCompra = new SelectList(db.TBL_Compra, "TN_IdCompra", "TN_IdCompra", tBL_Factura.TN_IdCompra);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Factura.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Factura.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Factura.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Factura.TN_IdMotor);
            ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario", tBL_Factura.TN_IdUsuario);
            ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo", tBL_Factura.TN_IdVehiculo);
            return View(tBL_Factura);
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Factura tBL_Factura = db.TBL_Factura.Find(id);
            if (tBL_Factura == null)
            {
                return HttpNotFound();
            }


            

			// Load data for select lists
			var marcas = db.TBL_Marca.OrderBy(m => m.TC_Descripcion).ToList();
			ViewBag.TN_IdMarca = new SelectList(marcas, "TN_IdMarca", "TC_Descripcion", tBL_Factura.TN_IdMarca);

			var modelos = db.TBL_Modelo.OrderBy(m => m.TC_Descripcion).ToList();
			ViewBag.TN_IdModelo = new SelectList(modelos, "TN_IdModelo", "TC_Descripcion", tBL_Factura.TN_IdModelo);

			var capacidades = db.TBL_Capacidad.OrderBy(c => c.TC_Descripcion).ToList();
			ViewBag.TN_IdCapacidad = new SelectList(capacidades, "TN_IdCapacidad", "TC_Descripcion", tBL_Factura.TN_IdCapacidad);

			var colores = db.TBL_Color.OrderBy(c => c.TC_Descripcion).ToList();
			ViewBag.TN_IdColor = new SelectList(colores, "TN_IdColor", "TC_Descripcion", tBL_Factura.TN_IdColor);

			var annos = db.TBL_Anno.OrderBy(c => c.TC_Descripcion).ToList();
			ViewBag.TN_IdAnno = new SelectList(annos, "TN_IdAnno", "TC_Descripcion", tBL_Factura.TN_IdAnno);

			var motores = db.TBL_Motor.OrderBy(c => c.TC_Descripcion).ToList();
			ViewBag.TN_IdMotor = new SelectList(motores, "TN_IdMotor", "TC_Descripcion", tBL_Factura.TN_IdMotor);

			var disennos = db.TBL_Disenno.OrderBy(c => c.TC_Descripcion).ToList();
			ViewBag.TN_IdDisenno = new SelectList(disennos, "TN_IdDisenno", "TC_Descripcion", tBL_Factura.TN_IdDisenno);
			
            
            // Load data for Numero de Compra, Usuario y Vehiculo
			var compras = db.TBL_Compra.OrderBy(c => c.TN_IdCompra).ToList();
			ViewBag.TN_IdCompra = new SelectList(compras, "TN_IdCompra", "TN_IdCompra", tBL_Factura.TN_IdCompra);

			var usuarios = db.TBL_Usuario.OrderBy(u => u.TN_IdUsuario).ToList();
			ViewBag.TN_IdUsuario = new SelectList(usuarios, "TN_IdUsuario", "TN_IdUsuario", tBL_Factura.TN_IdUsuario);

			var vehiculos = db.TBL_Vehiculo.OrderBy(v => v.TN_IdVehiculo).ToList();
			ViewBag.TN_IdVehiculo = new SelectList(vehiculos, "TN_IdVehiculo", "TN_IdVehiculo", tBL_Factura.TN_IdVehiculo);


			return View(tBL_Factura);
        }

		private SelectList GetSortedSelectList<T>(IQueryable<T> query, string valueField, string textField, object selectedValue = null)
		{
			var items = query.OrderBy(item => item.GetType().GetProperty(textField).GetValue(item, null))
							.Select(item => new SelectListItem
							{
								Value = item.GetType().GetProperty(valueField).GetValue(item, null).ToString(),
								Text = item.GetType().GetProperty(textField).GetValue(item, null).ToString()
							});

			return new SelectList(items, "Value", "Text", selectedValue);
		}


		// POST: Facturas/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TN_IdFactura,TN_IdVehiculo,TN_IdUsuario,TN_IdMarca,TN_IdModelo,TN_IdCapacidad,TN_IdColor,TN_IdAnno,TN_IdMotor,TN_IdDisenno,TN_IdCompra")] TBL_Factura tBL_Factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_Factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TN_IdAnno = new SelectList(db.TBL_Anno, "TN_IdAnno", "TC_Descripcion", tBL_Factura.TN_IdAnno);
            ViewBag.TN_IdCapacidad = new SelectList(db.TBL_Capacidad, "TN_IdCapacidad", "TC_Descripcion", tBL_Factura.TN_IdCapacidad);
            ViewBag.TN_IdColor = new SelectList(db.TBL_Color, "TN_IdColor", "TC_Descripcion", tBL_Factura.TN_IdColor);
            ViewBag.TN_IdCompra = new SelectList(db.TBL_Compra, "TN_IdCompra", "TN_IdCompra", tBL_Factura.TN_IdCompra);
            ViewBag.TN_IdDisenno = new SelectList(db.TBL_Disenno, "TN_IdDisenno", "TC_Descripcion", tBL_Factura.TN_IdDisenno);
            ViewBag.TN_IdMarca = new SelectList(db.TBL_Marca, "TN_IdMarca", "TC_Descripcion", tBL_Factura.TN_IdMarca);
            ViewBag.TN_IdModelo = new SelectList(db.TBL_Modelo, "TN_IdModelo", "TC_Descripcion", tBL_Factura.TN_IdModelo);
            ViewBag.TN_IdMotor = new SelectList(db.TBL_Motor, "TN_IdMotor", "TC_Descripcion", tBL_Factura.TN_IdMotor);
            ViewBag.TN_IdUsuario = new SelectList(db.TBL_Usuario, "TN_IdUsuario", "TN_IdUsuario", tBL_Factura.TN_IdUsuario);
            ViewBag.TN_IdVehiculo = new SelectList(db.TBL_Vehiculo, "TN_IdVehiculo", "TN_IdVehiculo", tBL_Factura.TN_IdVehiculo);
            return View(tBL_Factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Factura tBL_Factura = db.TBL_Factura.Find(id);
            if (tBL_Factura == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_Factura tBL_Factura = db.TBL_Factura.Find(id);
            db.TBL_Factura.Remove(tBL_Factura);
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
