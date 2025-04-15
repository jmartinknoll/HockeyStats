using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Player
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string TeamCode { get; set; }
        public string Position { get; set; }
    }
}
