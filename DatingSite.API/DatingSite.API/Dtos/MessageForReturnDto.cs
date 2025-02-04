﻿using DatingSite.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Dtos {
    public class MessageForReturnDto {

        public int SenderId { get; set; }

        public int RecipientId { get; set; }

        public DateTime MessageSent { get; set; }

        public string Content { get; set; }

        public MessageForReturnDto() {
            MessageSent = DateTime.Now;
        }

    }
}
