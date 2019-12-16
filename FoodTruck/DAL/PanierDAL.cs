﻿using FoodTruck.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodTruck.DAL
{
    class PanierDAL
    {
        public int ClientId { get; set; }

        public PanierDAL(int clientId)
        {
            ClientId = clientId;
        }

        public List<Panier> ListerPanierClient()
        {
            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    List<Panier> paniers = (from panier in db.Panier
            //                            join article in db.Article on panier.ArticleId equals article.Id
            //                            where panier.ClientId == ClientId
            //                            select panier).ToList();
            //    return paniers;
            //}
            //TODO OMNIFW
            return new List<Panier>();
        }

        ///Ajouter un article non présent au panier en base d'un client
        public void Ajouter(Article article, int quantite = 1)
        {
            Panier panier = new Panier
            {
                ArticleId = article.Id,
                ClientId = ClientId,
                Quantite = quantite,
                PrixTotal = Math.Round(quantite * article.PrixTTC, 2)
            };
            panier.Enregistrer();
        }

        ///Modifier la quantité d'un article du panier en base d'un client
        public void ModifierQuantite(Article article, int quantite)
        {
            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    Panier panier = (from p in db.Panier
            //                     where p.ClientId == ClientId && p.ArticleId == article.Id
            //                     select p).FirstOrDefault();
            //    panier.Quantite += quantite;
            //    panier.PrixTotal = Math.Round(panier.PrixTotal + quantite * article.PrixTTC, 2);
            //    db.SaveChanges();
            //}
            throw new NotImplementedException();
        }

        /// Supprimer l'article du panier en base de du client
        public void Supprimer(Article article)
        {
            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    Panier panier = (from p in db.Panier
            //                     where p.ClientId == ClientId && p.ArticleId == article.Id
            //                     select p).FirstOrDefault();

            //    db.Panier.Remove(panier);
            //    db.SaveChanges();
            //}
            throw new NotImplementedException();
        }

        /// Supprimer le panier en base du client
        public void Supprimer()
        {
            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    var panier = from p in db.Panier
            //                 where p.ClientId == ClientId
            //                 select p;

            //    db.Panier.RemoveRange(panier);
            //    db.SaveChanges();
            //}
            throw new NotImplementedException();
        }

        public List<Article> ArticlesPanierClient()
        {
            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    List<Article> articles = (from panier in db.Panier
            //                              join article in db.Article on panier.ArticleId equals article.Id
            //                              where panier.ClientId == ClientId
            //                              select article).ToList();
            //    return articles;
            //}

            //TODO OMNIFW
            List<Article> articles = new List<Article>();
            return articles;
        }

        internal List<Article> SupprimerArticlesPasDansCarte()
        {
            List<Article> articles = ArticlesPanierClient();
            List<Article> articlesPasDansCarte = articles.FindAll(art => !art.DansCarte);

            //using (foodtruckEntities db = new foodtruckEntities())
            //{
            //    var paniersASupprimer = (from panier in db.Panier
            //                             join article in db.Article on panier.ArticleId equals article.Id
            //                             where panier.ClientId == ClientId && !article.DansCarte
            //                             select panier).ToList();

            //    db.Panier.RemoveRange(paniersASupprimer);
            //    db.SaveChanges();
            //}
            //return articlesPasDansCarte;


            //TODO OMNIFW
            return articles;
        }
    }
}