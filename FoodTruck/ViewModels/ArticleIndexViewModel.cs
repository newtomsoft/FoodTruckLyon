﻿using FoodTruck.DAL;
using System.Collections.Generic;

namespace FoodTruck.ViewModels
{
    public class ArticleIndexViewModel
    {
        public List<ArticleViewModel> Articles { get; set; }

        /// <summary>
        /// retourne les articles dans carte ou non selon dansCarte
        /// </summary>
        public ArticleIndexViewModel(bool dansCarte)
        {
            Articles = new List<ArticleViewModel>();
            foreach (Article article in new ArticleDAL().Articles(dansCarte))
            {
                Articles.Add(new ArticleViewModel(article));
            }
        }
        /// <summary>
        /// retourne tous les articles qu'ils soient ou non dans la carte
        /// </summary>
        public ArticleIndexViewModel()
        {
            Articles = new List<ArticleViewModel>();
            foreach (Article article in new ArticleDAL().TousArticles())
            {
                Articles.Add(new ArticleViewModel(article));
            }
        }
    }
}