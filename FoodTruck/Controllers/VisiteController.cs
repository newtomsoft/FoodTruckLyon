﻿using FoodTruck.DAL;
using FoodTruck.Models;
using System;
using System.Web.Mvc;

namespace FoodTruck.Controllers
{
    public class VisiteController : Controller
    {
        public void Enregistrer(int lUtilisateurId)
        {
            string adresseIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            string navigateur = System.Web.HttpContext.Current.Request.Browser.Browser;
            string UrlOrigine="";
            if (System.Web.HttpContext.Current.Request.UrlReferrer!=null)
                UrlOrigine = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            Visite laVisite = new Visite
            {
                Url = url,
                DateTimeVisite = DateTime.Now,
                AdresseIp = adresseIP,
                UtilisateurId = lUtilisateurId,
                Navigateur = navigateur,
                UrlOrigine = UrlOrigine
            };
            VisiteDAL laVisiteDAL = new VisiteDAL(laVisite);
        }
    }
}