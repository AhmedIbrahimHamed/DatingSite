using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Dtos {
    public class MessageForReadDto {

         public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }
    }
}
