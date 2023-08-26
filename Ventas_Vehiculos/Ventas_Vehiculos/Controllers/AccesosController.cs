using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Ventas_Vehiculos.Models;
using System.Runtime.Remoting.Messaging;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Net;
using Ventas_Vehiculos;
using Ventas_Vehiculos.Models;
using System.Data.Entity.Infrastructure;

namespace Ventas_Vehiculos.Controllers
{
	public class AccesosController : Controller
	{
		static string conn = "Data Source=DESKTOP-6R7OIPF\\SQLEXPRESS;Initial Catalog=DB_Vehiculos; Integrated Security=true";


		// GET: Accesos
		public ActionResult Inicio_Sesion()
		{

			return View();
		}

		public ActionResult Registro()
		{

			return View();
		}


		[HttpPost]
		public ActionResult Registro(Usuarios objUsuarios)
		{
			bool registrado;
			string mensaje;
			if (objUsuarios.TC_Clave == objUsuarios.confirmar_clave) {
			}  else {
				ViewData["mensaje"] = "Contraseñas no coinciden";
				return View();
			}
			using (SqlConnection cons = new SqlConnection(conn)) {

				SqlCommand cmd = new SqlCommand("SP_RegistroUsuario", cons);
				cmd.Parameters.AddWithValue("cedula", objUsuarios.TN_Cedula);
				cmd.Parameters.AddWithValue("nombre", objUsuarios.TC_Nombre);
				cmd.Parameters.AddWithValue("apellido1", objUsuarios.TC_PrimerApellido);
				cmd.Parameters.AddWithValue("apellido2", objUsuarios.TC_SegundoApellido);
				cmd.Parameters.AddWithValue("correo", objUsuarios.TC_Correo);
				cmd.Parameters.AddWithValue("direccion", objUsuarios.TC_Direccion);
				cmd.Parameters.AddWithValue("clave", objUsuarios.TC_Clave);
				cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
				cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
				cmd.CommandType = CommandType.StoredProcedure;
				cons.Open();
				cmd.ExecuteNonQuery();
				registrado = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
				mensaje = cmd.Parameters["mensaje"].Value.ToString();
			}
			ViewData["mensaje"] = mensaje;
			if (registrado)
			{
				return RedirectToAction("Inicio_Sesion", "Accesos");
			}
			else
			{
				ViewData["mensaje"] = "Contraseñas no coinciden";
				return View();
			}
		
		}//registro
		[HttpPost]
		public ActionResult  Inicio_Sesion(Usuarios objUsuarios)
		{
			using (SqlConnection cons = new SqlConnection(conn))
			{
				SqlCommand cmd = new SqlCommand("SP_ValidarUsuario", cons);
				cmd.Parameters.AddWithValue("Correo", objUsuarios.TC_Correo);
				cmd.Parameters.AddWithValue("Clave", objUsuarios.TC_Clave);
				cmd.CommandType = CommandType.StoredProcedure;
				cons.Open();
				objUsuarios.TN_IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
			}
			if (objUsuarios.TN_IdUsuario != 0)
			{
				using (var context = new DB_VehiculosEntities3())
				{
					//validar rol
					bool isValid = context.TBL_Usuario.Any(x => x.TC_Correo == objUsuarios.TC_Correo &&
					x.TC_Clave == objUsuarios.TC_Clave && x.TC_TipoUsuario == "Administrador");

					if (isValid)
					{
						FormsAuthentication.SetAuthCookie(objUsuarios.TC_Correo, false);
						return RedirectToAction("Index", "Home");
					}

					bool isValidCliente = context.TBL_Usuario.Any(x => x.TC_Correo == objUsuarios.TC_Correo &&
					x.TC_Clave == objUsuarios.TC_Clave && x.TC_TipoUsuario == "Cliente");

					if (isValidCliente)
					{
						FormsAuthentication.SetAuthCookie(objUsuarios.TC_Correo, false);
						return RedirectToAction("Index", "Home_Cliente");
					}

					ModelState.AddModelError("", "Usuario o contraseña incorrecto");

				return View();

				}
			}
			else
			{
				ViewData["mensaje"] = "Usuario no encontrado";
				return View();

			}


		}

	}//class
}//namespace