using System;
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
        public string ImageUrl 
        { 
            get
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                return proxy.GetPhotoUri() + this.PostId + ".jpg";
            }
 
        }

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
            
        }
    }
}