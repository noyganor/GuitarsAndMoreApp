﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarsAndMoreApp.Models
{
    class UserFavoritesPost
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
