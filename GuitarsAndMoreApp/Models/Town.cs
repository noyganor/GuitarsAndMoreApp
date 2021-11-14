using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    public class Town
    {

        public int TownId { get; set; }
        public string Town1 { get; set; }
        public int? AreaId { get; set; }

        public virtual Area Area { get; set; }
    }
}
