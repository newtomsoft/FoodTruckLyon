﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodTruck.Models
{
    public class ArticleUI : Article
    {
        public ArticleUI(Article lArticle)
        {
            Quantite = 1;

            Id = lArticle.Id;
            Nom = lArticle.Nom;
            Image = lArticle.Image;
            Prix = lArticle.Prix;
            FamilleId = lArticle.FamilleId;
            NombreVendus = lArticle.NombreVendus;
            Description = lArticle.Description;
            Allergenes = lArticle.Allergenes;
            Grammage = lArticle.Grammage;
            Litrage = lArticle.Litrage;
            DansCarte = lArticle.DansCarte;
        }
        public ArticleUI(Article lArticle, int quantite)
        {
            Quantite = quantite;

            Id = lArticle.Id;
            Nom = lArticle.Nom;
            Image = lArticle.Image;
            Prix = lArticle.Prix;
            FamilleId = lArticle.FamilleId;
            NombreVendus = lArticle.NombreVendus;
            Description = lArticle.Description;
            Allergenes = lArticle.Allergenes;
            Grammage = lArticle.Grammage;
            Litrage = lArticle.Litrage;
            DansCarte = lArticle.DansCarte;
        }

        public int Quantite { get; set; }
    }
}