//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodTruck
{
    using System;
    using System.Collections.Generic;
    
    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            this.Commande_Article = new HashSet<Commande_Article>();
            this.Panier = new HashSet<Panier>();
            this.PanierProspect = new HashSet<PanierProspect>();
        }
    
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Image { get; set; }
        public int FamilleId { get; set; }
        public int NombreVendus { get; set; }
        public string Description { get; set; }
        public string Allergenes { get; set; }
        public int Grammage { get; set; }
        public int Litrage { get; set; }
        public bool DansCarte { get; set; }
        public double PrixHT { get; set; }
        public double PrixTTC { get; set; }
    
        public virtual FamilleArticle FamilleArticle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Commande_Article> Commande_Article { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Panier> Panier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PanierProspect> PanierProspect { get; set; }
    }
}
