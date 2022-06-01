using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class LookUpTables
    {
        public List<Gender> Genders { get; set; }
        public List<Area> Areas { get; set; }
        public List<Town> Towns { get; set; }
        public List<Producer> Producers { get; set; }
        public List<Model> Models { get; set; }
        public List<Category> Categories { get; set; }

        //public List<UserFavoritePost> UserFavoritePosts { get; set; }
    }
}
