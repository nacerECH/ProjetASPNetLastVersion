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
    using System.ComponentModel;
    using System.Web;

    public partial class Proprietaire
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proprietaire()
        {
            this.ContactSupports = new HashSet<ContactSupport>();
            this.Historiques = new HashSet<Historique>();
            this.Produits = new HashSet<Produit>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public Nullable<bool> isCompany { get; set; } = false;
        public string Tel { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }

        [DisplayName("Choisir une image")]
        public string Logo { get; set; }

        public HttpPostedFileBase imageFile { get; set; }
        public Nullable<bool> isHonored { get; set; }
        public Nullable<bool> isBlocked { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactSupport> ContactSupports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historique> Historiques { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
