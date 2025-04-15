﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.HockeyStatsApi
{
    public class Skater
    {
        public int? PlayerId { get; set; }
        public string? Headshot { get; set; }
        public Name? FirstName { get; set; }
        public Name? LastName { get; set; }
        public string? PositionCode { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Points { get; set; }
        public int? PlusMinus { get; set; }
        public int? PenaltyMinutes { get; set; }
        public int? PowerPlayGoals { get; set; }
        public int? ShorthandedGoals { get; set; }
        public int? GameWinningGoals { get; set; }
        public int? OvertimeGoals { get; set; }
        public int? Shots { get; set; }
        public double? ShootingPctg { get; set; }
        public double? AvgTimeOnIcePerGame { get; set; }
        public double? AvgShiftsPerGame { get; set; }
        public double? FaceoffWinPctg { get; set; }
    }
}
