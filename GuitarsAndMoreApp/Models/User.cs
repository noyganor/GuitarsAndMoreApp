using System;
using System.Collections.Generic;
using System.Text;

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

        public virtual Gender Gender { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserFavoritePost> UserFavoritesPosts { get; set; }

    }
}
