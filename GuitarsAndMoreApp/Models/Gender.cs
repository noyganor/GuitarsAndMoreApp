using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }

        public int GenderId { get; set; }
        public string Gender1 { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
