//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonicaExtra.Model.monicaextra
{
    using System;
    using System.Collections.Generic;
    
    public partial class movimientocaja
    {
        public short NumeroTransacion { get; set; }
        public string Beneficiario { get; set; }
        public string Concepto { get; set; }
        public string Rnc { get; set; }
        public string Ncf { get; set; }
        public Nullable<short> TipoMovimiento { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public string Itebis { get; set; }
        public Nullable<decimal> Neto { get; set; }
        public string Soporte { get; set; }
        public string Fecha { get; set; }
        public Nullable<int> Saldo { get; set; }
        public string EntradaSalida { get; set; }
        public string CodigoCajero { get; set; }
        public Nullable<short> NumeroCaja { get; set; }
        public string TipoMoneda { get; set; }
        public Nullable<decimal> TasaCambio { get; set; }
        public Nullable<short> Estatus { get; set; }
        public Nullable<short> Clasificancf { get; set; }
        public Nullable<short> NumeroCierre { get; set; }
    }
}
