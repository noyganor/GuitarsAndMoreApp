using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class Producer
    {
        public Producer()
        {
            Posts = new HashSet<Post>();
        }

        public int ProducerId { get; set; }
        public string Producer1 { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
