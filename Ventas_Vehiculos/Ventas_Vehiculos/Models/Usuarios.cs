using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ventas_Vehiculos.Models
{
	public class Usuarios
	{

		public int TN_IdUsuario { get; set; }
		public string TC_TipoUsuario { get; set; }
		public int TN_Cedula { get; set; }
		public string TC_Nombre { get; set; }
		public string TC_PrimerApellido { get; set; }
		public string TC_SegundoApellido { get; set; }
		public string TC_Correo { get; set; }
		public string TC_Direccion { get; set; }
		public string TC_Clave { get; set; }
		public string confirmar_clave { get; set; }



	}//class
}//namespace