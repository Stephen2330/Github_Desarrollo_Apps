//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ventas_Vehiculos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Vehiculo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_Vehiculo()
        {
            this.TBL_Compra = new HashSet<TBL_Compra>();
            this.TBL_Factura = new HashSet<TBL_Factura>();
        }
    
        public int TN_IdVehiculo { get; set; }
        public int TN_IdMarca { get; set; }
        public int TN_IdModelo { get; set; }
        public int TN_IdColor { get; set; }
        public int TN_IdAnno { get; set; }
        public int TN_IdCapacidad { get; set; }
        public int TN_IdMotor { get; set; }
        public int TN_IdDisenno { get; set; }
    
        public virtual TBL_Anno TBL_Anno { get; set; }
        public virtual TBL_Capacidad TBL_Capacidad { get; set; }
        public virtual TBL_Color TBL_Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_Compra> TBL_Compra { get; set; }
        public virtual TBL_Disenno TBL_Disenno { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_Factura> TBL_Factura { get; set; }
        public virtual TBL_Marca TBL_Marca { get; set; }
        public virtual TBL_Modelo TBL_Modelo { get; set; }
        public virtual TBL_Motor TBL_Motor { get; set; }
    }
}