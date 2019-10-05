﻿using System.Collections.Generic;
using System.Linq;
using FoodTruck.Models;

namespace FoodTruck.DAL
{
    class PanierDAL
    {
        public int UtilisateurId { get; set; }

        public PanierDAL(int utilisateurId)
        {
            UtilisateurId = utilisateurId;
        }

        ///Ajouter un article au panier en base d'un utilisateur
        public void Ajouter(Article lArticle)
        {
            Panier lePanier = new Panier
            {
                ArticleId = lArticle.Id,
                UtilisateurId = UtilisateurId,
                Quantite = 1
            };
            lePanier.PrixTotal = lArticle.Prix * lePanier.Quantite;

            using (foodtruckEntities db = new foodtruckEntities())
            {
                db.Panier.Add(lePanier);
                db.SaveChanges();
            }
        }

        ///Modifier la quantité d'un article du panier en base d'un utilisateur
        public void ModifierQuantite(Article lArticle, int quantite)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                Panier lePanier = (from panier in db.Panier
                                   where panier.UtilisateurId == UtilisateurId && panier.ArticleId == lArticle.Id
                                   select panier).FirstOrDefault();
                lePanier.Quantite += quantite;
                lePanier.PrixTotal += quantite * lArticle.Prix;
                db.SaveChanges();
            }
        }

        /// Supprimer l'article du panier en base de l'utilisateur
        public void Supprimer(Article lArticle)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                Panier lePanier = (from panier in db.Panier
                                   where panier.UtilisateurId == UtilisateurId && panier.ArticleId == lArticle.Id
                                   select panier).FirstOrDefault();

                db.Panier.Remove(lePanier);
                db.SaveChanges();
            }
        }

        /// Supprimer le panier en base de l'utilisateur
        public void Supprimer()
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                var lePanier = from panier in db.Panier
                               where panier.UtilisateurId == UtilisateurId
                               select panier;

                db.Panier.RemoveRange(lePanier);
                db.SaveChanges();
            }
        }

        public List<Panier> ListerPanierUtilisateur()
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Panier> paniers = (from panier in db.Panier
                                        join article in db.Article on panier.ArticleId equals article.Id
                                        where panier.UtilisateurId == UtilisateurId
                                        select panier).ToList();
                return paniers;
            }
        }

        public List<Article> ListerArticlesPanierUtilisateur()
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Article> articles = (from panier in db.Panier
                                        join article in db.Article on panier.ArticleId equals article.Id
                                        where panier.UtilisateurId == UtilisateurId
                                        select article).ToList();
                return articles;
            }
        }

    }
}