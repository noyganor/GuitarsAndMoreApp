using GuitarsAndMoreApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GuitarsAndMoreApp.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Pass { get; set; }
        public string VerPassword { get; set; }
        public string PhoneNum { get; set; }
        public int GenderId { get; set; }
        public string FavBand { get; set; }
        public DateTime JoinDate { get; set; }

        public bool? IsManager { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserFavoritePost> UserFavoritePosts { get; set; }

        //Additional to the server user class
        public string ImageUrl
        {
            get
            {
                //We use cache busting to avoid image caching on the phone
                Random r = new Random();
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                return proxy.GetPhotoUri() + "U" + this.UserId + ".jpg?" + r.Next(10000);
            }

        }

        public bool CheckIsManager()
        {
            return (IsManager.HasValue && IsManager.Value);
        }

        [JsonIgnore]
        public bool IsManagerProp
        {
            get
            {
                return (IsManager.HasValue && IsManager.Value);
            }
        }

    }
}
