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
    
    public partial class OubliMotDePasse
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; }
        public string CodeVerification { get; set; }
        public System.DateTime DateFinValidite { get; set; }
    
        public virtual Utilisateur Utilisateur { get; set; }
    }
}