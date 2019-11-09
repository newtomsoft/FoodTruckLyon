﻿using FoodTruck.Models;
using FoodTruck.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FoodTruck.DAL
{
    class CommandeDAL
    {
        internal Commande Detail(int id)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                Commande commande = (from cmd in db.Commande
                                     where cmd.Id == id
                                     select cmd).FirstOrDefault();
                return commande;
            }
        }

        public void Ajouter(Commande commande, List<ArticleViewModel> articles)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                db.Commande.Add(commande);
                db.SaveChanges();
                int idCommande = (from cmd in db.Commande
                                  where cmd.UtilisateurId == commande.UtilisateurId
                                  orderby cmd.Id descending
                                  select cmd.Id).FirstOrDefault();

                foreach (var article in articles)
                {
                    int quantite = article.Quantite;
                    double prixTotal = Math.Round(article.Article.Prix * quantite, 2);
                    Commande_Article cmdArt = new Commande_Article
                    {
                        CommandeId = idCommande,
                        ArticleId = article.Article.Id,
                        Quantite = quantite,
                        PrixTotal = prixTotal
                    };
                    db.Commande_Article.Add(cmdArt);
                }
                db.SaveChanges();
            }
        }
        internal void Annuler(int id)
        {
            MettreAJourStatut(id, false, true);
        }
        internal void Retirer(int id)
        {
            MettreAJourStatut(id, true, false);
        }

        private void MettreAJourStatut(int id, bool retrait, bool annule)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                Commande commande = (from cmd in db.Commande
                                     where cmd.Id == id
                                     select cmd).FirstOrDefault();
                if (commande != null)
                {
                    commande.Annulation = annule;
                    commande.Retrait = retrait;
                    Utilisateur utilisateur = (from u in db.Utilisateur
                                               where u.Id == commande.UtilisateurId
                                               select u).FirstOrDefault();
                    if (commande.Retrait)
                        utilisateur.Cagnotte += (int)commande.PrixTotal / 10;
                    if (commande.Annulation)
                        utilisateur.Cagnotte += (int)commande.RemiseFidelite;
                    db.SaveChanges();
                }
            }
        }

        internal List<Commande> CommandesAStatuer()
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                DateTime now = DateTime.Now;
                const int intervalleMax = 1;
                var commandes = (from cmd in db.Commande
                                 where !cmd.Retrait && !cmd.Annulation && DbFunctions.DiffHours(cmd.DateRetrait, now) >= intervalleMax
                                 orderby cmd.DateRetrait
                                 select cmd).ToList();
                return commandes;
            }
        }

        public List<Commande> CommandesEnCours(int fourchetteHeures)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                DateTime now = DateTime.Now;
                var commandes = (from cmd in db.Commande
                                 where !cmd.Retrait && !cmd.Annulation && Math.Abs((int)DbFunctions.DiffHours(now, cmd.DateRetrait)) < Math.Abs(fourchetteHeures)
                                 orderby cmd.DateRetrait
                                 select cmd).ToList();
                return commandes;
            }
        }

        internal List<Commande> CommandesUtilisateur(int id)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                DateTime now = DateTime.Now;
                List<Commande> commandes = (from cmd in db.Commande
                                            where cmd.UtilisateurId == id
                                            orderby cmd.Annulation, cmd.Retrait, Math.Abs((int)DbFunctions.DiffHours(now, cmd.DateRetrait))
                                            select cmd).ToList();
                return commandes;
            }
        }

        internal double RemiseTotaleUtilisateur(int id)
        {
            double remise = 0;
            List<Commande> commandes = CommandesUtilisateur(id);
            foreach (Commande commande in commandes)
            {
                remise += commande.RemiseCommerciale + commande.RemiseFidelite;
            }
            remise = Math.Round(remise, 2);
            return remise;
        }

        internal int NombreCommandes(DateTime date)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                int nbCommandes = (from cmd in db.Commande
                                   where cmd.DateRetrait == date && !cmd.Annulation && !cmd.Retrait
                                   select cmd.Id).Count();
                return nbCommandes;
            }
        }

        internal List<Commande> CommandesEnCoursUtilisateur(int id)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                DateTime now = DateTime.Now;
                List<Commande> commandes = (from cmd in db.Commande
                                            where cmd.UtilisateurId == id && !cmd.Annulation && !cmd.Retrait && DbFunctions.DiffHours(now, cmd.DateRetrait) > -1
                                            orderby Math.Abs((int)DbFunctions.DiffMinutes(now, cmd.DateRetrait))
                                            select cmd).ToList();
                return commandes;
            }
        }

        internal List<Commande> CommandesToutes(string recherche = "", DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            DateTime debut = dateDebut ?? DateTime.MinValue;
            DateTime fin = dateFin ?? DateTime.MaxValue;
            debut = debut.Date;
            fin = fin.Date;

            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Commande> commandes =
                    (from cmd in db.Commande
                     join u in db.Utilisateur on cmd.UtilisateurId equals u.Id
                     where DbFunctions.DiffDays(debut, cmd.DateRetrait) >= 0 && DbFunctions.DiffDays(cmd.DateRetrait, fin) >= 0 &&
                     (cmd.Id.ToString().Contains(recherche) || u.Nom.Contains(recherche) || u.Prenom.Contains(recherche) || u.Email.Contains(recherche))
                     orderby cmd.Id descending
                     select cmd).ToList();

                return commandes;
            }
        }

        internal List<Commande> CommandesFutures()
        {
            DateTime maintenant = DateTime.Now;
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Commande> commandes = (from cmd in db.Commande
                                            where DbFunctions.DiffMinutes(maintenant, cmd.DateRetrait) >= 0 && !cmd.Retrait && !cmd.Annulation
                                            orderby cmd.DateRetrait
                                            select cmd).ToList();
                return commandes;
            }
        }
        internal List<Commande> CommandesPendantFermetures()
        {
            List<Commande> commandesPendantFermetures = CommandesPendantFermeturesExceptionnelles();
            commandesPendantFermetures.AddRange(CommandesPendantFermeturesHabituelles());
            List<Commande> commandesPendantOuverturesExceptionnelles = CommandesPendantOuverturesExceptionnelles();
            return commandesPendantFermetures.Except(commandesPendantOuverturesExceptionnelles, new CommandeEqualityComparer()).ToList(); ;
        }

        private List<Commande> CommandesPendantFermeturesExceptionnelles()
        {
            DateTime maintenant = DateTime.Now;
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Commande> commandes = (from cmd in db.Commande
                                            where DbFunctions.DiffMinutes(maintenant, cmd.DateRetrait) >= 0 && !cmd.Retrait && !cmd.Annulation
                                            orderby cmd.DateRetrait
                                            select cmd).ToList();

                List<JourExceptionnel> fermetures = (from j in db.JourExceptionnel
                                                     where DbFunctions.DiffMinutes(maintenant, j.DateFin) >= 0 && !j.Ouvert
                                                     select j).ToList();

                List<Commande> commandesDansFermeture = new List<Commande>();
                foreach (JourExceptionnel fermeture in fermetures)
                {
                    commandesDansFermeture.AddRange(commandes.FindAll(c => fermeture.DateDebut <= c.DateRetrait && c.DateRetrait <= fermeture.DateFin));
                }
                return commandesDansFermeture;
            }
        }

        private List<Commande> CommandesPendantOuverturesExceptionnelles()
        {
            DateTime maintenant = DateTime.Now;
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Commande> commandes = (from cmd in db.Commande
                                            where DbFunctions.DiffMinutes(maintenant, cmd.DateRetrait) >= 0 && !cmd.Retrait && !cmd.Annulation
                                            orderby cmd.DateRetrait
                                            select cmd).ToList();

                List<JourExceptionnel> ouvertures = (from j in db.JourExceptionnel
                                                     where DbFunctions.DiffMinutes(maintenant, j.DateFin) >= 0 && j.Ouvert
                                                     select j).ToList();

                List<Commande> commandesDansOuvertures = new List<Commande>();
                foreach (JourExceptionnel fermeture in ouvertures)
                {
                    commandesDansOuvertures.AddRange(commandes.FindAll(c => fermeture.DateDebut <= c.DateRetrait && c.DateRetrait <= fermeture.DateFin));
                }
                return commandesDansOuvertures;
            }
        }

        private List<Commande> CommandesPendantFermeturesHabituelles()
        {
            DateTime maintenant = DateTime.Now;
            using (foodtruckEntities db = new foodtruckEntities())
            {
                List<Commande> commandes = (from cmd in db.Commande
                                            where DbFunctions.DiffMinutes(maintenant, cmd.DateRetrait) >= 0 && !cmd.Retrait && !cmd.Annulation
                                            orderby cmd.DateRetrait
                                            select cmd).ToList();

                List<OuvertureHebdomadaire> ouvertures = (from o in db.OuvertureHebdomadaire
                                                          select o).ToList();

                List<Commande> commandesDansOuvertures = new List<Commande>();
                foreach (OuvertureHebdomadaire ouverture in ouvertures)
                {
                    commandesDansOuvertures.AddRange(commandes.FindAll(c => (int)c.DateRetrait.DayOfWeek == ouverture.JourSemaineId && ouverture.Debut <= c.DateRetrait.TimeOfDay && c.DateRetrait.TimeOfDay <= ouverture.Fin));
                }
                List<Commande> commandesDansFermeture = commandes.Except(commandesDansOuvertures).ToList();
                return commandesDansFermeture;
            }
        }

        public List<ArticleViewModel> Articles(int commandeId)
        {
            using (foodtruckEntities db = new foodtruckEntities())
            {
                var listArticlesQuantites = (from cmd in db.Commande
                                             join ca in db.Commande_Article on cmd.Id equals ca.CommandeId
                                             join article in db.Article on ca.ArticleId equals article.Id
                                             where cmd.Id == commandeId
                                             orderby article.FamilleId, article.Nom
                                             select new { article, ca.Quantite }).ToList();

                List<ArticleViewModel> listArticles = new List<ArticleViewModel>();
                foreach (var articleQuantite in listArticlesQuantites)
                {
                    listArticles.Add(new ArticleViewModel(articleQuantite.article, articleQuantite.Quantite));
                }
                return listArticles;
            }
        }
    }
}
