using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int CategoryId { get; set; }
        public string Category1 { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
