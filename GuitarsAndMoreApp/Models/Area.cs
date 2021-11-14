using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class Area
    {
        public Area()
        {
            Towns = new HashSet<Town>();
        }

        public int AreaId { get; set; }
        public string Area1 { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
