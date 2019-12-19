using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelViewEarnedPointsForPlayer
    {

        public FootballPlayer footballPlayer { get; set; }
        public List<EarnedPointsPerFixture> earnedPointsPerFixture { get; set; }
        public ModelViewEarnedPointsForPlayer() {
            this.earnedPointsPerFixture = new List<EarnedPointsPerFixture>();
        }
    }
}