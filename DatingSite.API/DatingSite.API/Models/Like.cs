﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Models {
    public class Like {

        public int Id { get; set; }

        public int LikerId { get; set; }

        public int LikeeId { get; set; }

        public User Liker { get; set; }

        public User Likee { get; set; }
    }
}
