﻿using System;
using System.Collections.Generic;
using System.Text;
using GuitarsAndMoreApp.Services;

namespace GuitarsAndMoreApp.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int? ReviewId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int? ModelId { get; set; }
        public int TownId { get; set; }
        public double Price { get; set; }
        public string Pdescription { get; set; }
        public string Link { get; set; }

        //additional
        public string ImageUrl 
        { 
            get
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                return proxy.GetPhotoUri() + this.PostId + ".jpg";
            }
 
        }

        public string Producer { get; set; }
        public string PhoneNum { get; set; }

        public virtual Category Category { get; set; }
        public virtual Model Model { get; set; }

        public virtual Town Town { get; set; }
        public virtual User User { get; set; }

        //additional 
        public Post() { }
        public Post(Post p)
        {
            this.PostId = p.PostId;
            this.ReviewId = p.ReviewId;
            this.CategoryId = p.CategoryId;
            this.UserId = p.UserId;
            this.ModelId = p.ModelId;
            this.TownId = p.TownId;
            this.Price = p.Price;
            this.Pdescription = p.Pdescription;
            this.Link = p.Link;
            this.Producer = p.Producer;
            this.PhoneNum = p.PhoneNum;
            
        }

        private const string FAV_OFF_URL = "outline_favorite_border_black_24dp.png";
        private const string FAV_ON_URL = "fullHeart.png";

        public string FavoriteImageUrl
        {
            get
            {
                if (IsFavByUser())
                    return FAV_ON_URL;
                else
                    return FAV_OFF_URL;
            }
        }

        private const string FAV_ACTION_ON_URL = "addToFavorites.png";
        private const string FAV_ACTION_OFF_URL = "fullHeart.png";

        public string FavoriteActionImageUrl
        {
            get
            {
                if (IsFavByUser())
                    return FAV_ACTION_ON_URL;
                else
                    return FAV_ACTION_OFF_URL;
            }
        }

        public bool IsFavByUser()
        {
            App app = (App)App.Current;
            User user = app.CurrentUser;
            if (user == null)
                return false;
            foreach (UserFavoritePost uf in user.UserFavoritesPosts)
            {
                if (this.PostId == uf.PostId)
                    return true;
            }
            return false;
        }
    }
}