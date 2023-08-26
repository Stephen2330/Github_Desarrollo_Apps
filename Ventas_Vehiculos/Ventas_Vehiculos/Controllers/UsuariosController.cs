using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ventas_Vehiculos.Models;
using PagedList;
using PagedList.Mvc;

namespace Ventas_Vehiculos.Controllers
{
    public class UsuariosController : Controller
    {
		private DB_VehiculosEntities3 db = new DB_VehiculosEntities3();
		// GET: Usuarios

		public ActionResult Index()
        {
            return View(db.TBL_Usuario.ToList());
        }


        [HttpGet]
		public ActionResult Index(string searchName, int? page)
		{
			
			ViewBag.CurrentFilter = searchName;
			var usuarios = db.TBL_Usuario.AsQueryable();

			if (!string.IsNullOrEmpty(searchName))
			{
				usuarios = usuarios.Where(u => u.TC_Nombre.Contains(searchName));
			}
			usuarios = usuarios.OrderBy(u => u.TC_Nombre); // Ordenar alfabéticamente por nombre

			int pageSize = 10;
			int pageNumber = (page ?? 1);
			ViewBag.PageNumber = pageNumber;
			int totalItems = usuarios.Count(); // Cantidad total de elementos
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Cálculo de total de páginas
			ViewBag.totalPages = totalPages;
			var pagedUsuarios = usuarios.ToPagedList(pageNumber, pageSize);
			var pagedUsersAsEnumerable = pagedUsuarios.AsEnumerable();
			return View(pagedUsersAsEnumerable);
		}


		// GET: Usuarios/Details/5
		public ActionResult Details(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
                TBL_Usuario usuario = db.TBL_Usuario.Find(id);
			if (usuario == null)
			{
				return HttpNotFound();
			}
			return View(usuario);
		}

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TN_IdUsuario, TC_TipoUsuario, TN_Cedula, TC_Nombre, TC_PrimerApellido, TC_SegundoApellido, TC_Correo, TC_Direccion, TC_Clave")] TBL_Usuario usuario)
        {
			if (ModelState.IsValid)
			{
				db.TBL_Usuario.Add(usuario);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(usuario);
		}

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TBL_Usuario usuario = db.TBL_Usuario.Find(id);
			if (usuario == null)
			{
				return HttpNotFound();
			}
			return View(usuario);
		}

        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TN_idUsuario, TC_TipoUsuario, TN_Cedula, TC_Nombre, TC_PrimerApellido, TC_SegundoApellido, TC_Correo, TC_Direccion, TC_Clave")] TBL_Usuario usuario)
        {
			if (ModelState.IsValid)
			{
				db.Entry(usuario).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(usuario);
		}

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
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
