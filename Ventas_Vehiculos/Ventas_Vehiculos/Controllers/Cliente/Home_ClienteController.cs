using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ventas_Vehiculos.Controllers.Cliente
{
	
    public class Home_ClienteController : Controller
    {
		// GET: Home_Cliente
		public ActionResult Index()
		{
			return View();
		}



		public ActionResult Cerrar_Sesion()
		{
			Session["usuario"] = null;
			return RedirectToAction("Inicio_Sesion", "Accesos");
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
