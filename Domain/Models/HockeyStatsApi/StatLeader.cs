using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.HockeyStatsApi
{
    public class StatLeader
    {
        public Name? FirstName { get; set; }
        public Name? LastName { get; set; }
        public string? Position { get; set; }
        public double? Value { get; set; }
    }
}
