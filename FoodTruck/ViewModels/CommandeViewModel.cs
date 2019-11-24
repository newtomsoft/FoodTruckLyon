﻿using FoodTruck.DAL;
using SelectPdf;
using System;
using System.Collections.Generic;

namespace FoodTruck.ViewModels
{
    public class CommandeViewModel
    {
        public Commande Commande { get; set; }
        public Client Client { get; set; }
        public List<ArticleViewModel> ListArticlesVM { get; set; }

        public CommandeViewModel(Commande commande, Client client)
        {
            Commande = commande;
            Client = client;
            if (commande != null)
            {
                ListArticlesVM = new CommandeDAL().Articles(commande.Id);
            }
        }
    }
}