//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectAspNETv2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Historque
    {
        public int Id { get; set; }
        public string operation { get; set; }
        public Nullable<System.DateTime> operation_date { get; set; }
        public Nullable<int> proprietaireId { get; set; }
        public Nullable<int> produitId { get; set; }
    
        public virtual Produit Produit { get; set; }
        public virtual Proprietaire Proprietaire { get; set; }
    }
}
