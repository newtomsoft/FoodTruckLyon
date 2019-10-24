﻿using FoodTruck.DAL;
using FoodTruck.Models;
using FoodTruck.ViewModels;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace FoodTruck.Controllers
{
    public class AdministrationController : ControllerParent
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CommandesEnCours()
        {
            ListeCommandesViewModel listeCommandesViewModel = null;
            if (AdminCommande)
            {
                listeCommandesViewModel = new ListeCommandesViewModel(new CommandeDAL().ListerCommandesEnCours());
            }
            return View(listeCommandesViewModel);
        }

        [HttpPost]
        public ActionResult CommandesEnCours(int id, string statut)
        {
            bool retire = false;
            bool annule = false;
            if (statut == "retire")
                retire = true;
            else if (statut == "annule")
                annule = true;
            new CommandeDAL().MettreAJourStatut(id, retire, annule);
            return RedirectToAction(ActionNom);
        }

        [HttpGet]
        public ActionResult CommandesAStatuer()
        {
            ListeCommandesViewModel listeCommandesViewModel = null;
            if (AdminCommande)
            {
                CommandeDAL commandeDAL = new CommandeDAL();
                var commandes = commandeDAL.ListerCommandesAStatuer();
                listeCommandesViewModel = new ListeCommandesViewModel(commandes);
            }
            return View(listeCommandesViewModel);
        }

        [HttpPost]
        public ActionResult CommandesAStatuer(int id, string statut)
        {
            bool retire = false;
            bool annule = false;
            if (statut == "retire")
                retire = true;
            else if (statut == "annule")
                annule = true;
            new CommandeDAL().MettreAJourStatut(id, retire, annule);
            return RedirectToAction(ActionNom);
        }

        [HttpGet]
        public ActionResult Commandes()
        {
            ListeCommandesViewModel listeCommandesViewModel = null;
            if (AdminCommande)
            {
                CommandeDAL commandeDAL = new CommandeDAL();
                var commandes = commandeDAL.ListerCommandesToutes();
                listeCommandesViewModel = new ListeCommandesViewModel(commandes);
            }
            return View(listeCommandesViewModel);
        }

        [HttpGet]
        public ActionResult AjouterArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjouterArticle(string nom, string description, string prix, int? grammage, int? litrage, string allergenes, int familleId, bool dansCarte, HttpPostedFileBase file)
        {
            if (AdminArticle)
            {
                string nomOk = nom.NomAdmis();
                double prixOk = Math.Abs(Math.Round(float.Parse(prix, CultureInfo.InvariantCulture.NumberFormat), 2));
                int grammageOk = Math.Abs(grammage ?? 0);
                int litrageOk = Math.Abs(litrage ?? 0);
                string descriptionOk = description;
                string allergenesOk = allergenes ?? "";
                int familleIdOk = familleId;
                bool dansCarteOk = dansCarte;
                Article lArticle = new Article
                {
                    Nom = nomOk,
                    Description = descriptionOk,
                    Prix = prixOk,
                    Grammage = grammageOk,
                    Litrage = litrageOk,
                    Allergenes = allergenesOk,
                    FamilleId = familleIdOk,
                    DansCarte = dansCarteOk,
                };
                try
                {
                    string dossierImage = ConfigurationManager.AppSettings["PathImagesArticles"];
                    string fileName = nomOk.ToUrl() + Path.GetExtension(file.FileName);
                    string chemin = Path.Combine(Server.MapPath(dossierImage), fileName);
                    Image image = Image.FromStream(file.InputStream);
                    int tailleImage = Int32.Parse(ConfigurationManager.AppSettings["ImagesArticlesSize"]);
                    var nouvelleImage = new Bitmap(image, tailleImage, tailleImage);
                    nouvelleImage.Save(chemin);
                    nouvelleImage.Dispose();
                    image.Dispose();
                    lArticle.Image = fileName;
                    new ArticleDAL().Ajouter(lArticle);
                    TempData["AjoutOK"] = "Votre article a bien été ajouté";
                }
                catch (Exception ex)
                {
                    TempData["Erreur"] = ex.Message;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ModifierArticle()
        {
            if (AdminArticle)
                return View(new ArticleDAL().ListerTout());
            else
                return View();
        }

        [HttpPost]
        public ActionResult ModifierArticle(int id)
        {
            if (AdminArticle)
            {
                ArticleDAL articleDAL = new ArticleDAL();
                ViewBag.ArticleAModifier = articleDAL.Details(id);
                return View(articleDAL.ListerTout());
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult ModifierArticleEtape2(Article article, string prix, HttpPostedFileBase file)
        {
            if (AdminArticle)
            {
                double prixOk = Math.Abs(Math.Round(float.Parse(prix, CultureInfo.InvariantCulture.NumberFormat), 2));
                article.Prix = prixOk;
                article.Nom = article.Nom.NomAdmis();
                article.Grammage = Math.Abs(article.Grammage);
                article.Litrage = Math.Abs(article.Litrage);
                article.Allergenes = article.Allergenes ?? "";

                ArticleDAL articleDAL = new ArticleDAL();
                if (articleDAL.NomExiste(article.Nom, article.Id))
                {
                    TempData["Erreur"] = "Le nom de l'article existe déjà. Merci de choisir un autre nom ou bien de renommer d'abord l'article en doublon.";
                }
                else
                {
                    try
                    {
                        if (file != null)
                        {
                            string dossierImage = ConfigurationManager.AppSettings["PathImagesArticles"];
                            string fileName = article.Nom.ToUrl() + Path.GetExtension(file.FileName);
                            string chemin = Path.Combine(Server.MapPath(dossierImage), fileName);
                            Image image = Image.FromStream(file.InputStream);
                            int tailleImage = int.Parse(ConfigurationManager.AppSettings["ImagesArticlesSize"]);
                            var nouvelleImage = new Bitmap(image, tailleImage, tailleImage);
                            nouvelleImage.Save(chemin);
                            nouvelleImage.Dispose();
                            image.Dispose();
                            article.Image = fileName;
                        }
                        else
                        {
                            Article ancienArticle = articleDAL.Details(article.Id);
                            article.Image = ancienArticle.Image;
                        }
                        articleDAL.Modifier(article);
                        TempData["ModifOK"] = "Votre article a bien été modifié";

                    }
                    catch (Exception ex)
                    {
                        TempData["Erreur"] = ex.Message;
                    }
                }
            }
            return View();
        }
    }
}