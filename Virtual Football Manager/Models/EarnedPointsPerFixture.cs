using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class EarnedPointsPerFixture
    {
        public Fixture fixture { get; set; }
        public int totalPoints { get; set; }
        public int numOfGoalsScored { get; set; }
        public int numOfGoalAssists { get; set; }
        public int numOfTeamWinsForPlayer { get; set; }
        public int numOfPlayersWithMoreBallPosessionPerMatch { get; set; }
        public int numOfPlayersWithMoreBallTotalShootsPerMatch { get; set; }
        public int numOfPlayersWithMorePassesPerMatch { get; set; }
        public int numOfPlayersWithMoreFoulsPerMatch { get; set; }
        public int numOfPlayersWithMoreCornersPerMatch { get; set; }
        public int numOfPlayersWithMoreYellowCardsPerMatch { get; set; }
        public int numOfPlayersWithMoreRedCardsPerMatch { get; set; }

        public EarnedPointsPerFixture() {
            this.totalPoints = 0;
            this.numOfGoalsScored = 0;
            this.numOfGoalAssists = 0;
            this.numOfTeamWinsForPlayer = 0;
            this.numOfPlayersWithMoreBallPosessionPerMatch = 0;
            this.numOfPlayersWithMoreBallTotalShootsPerMatch = 0;
            this.numOfPlayersWithMorePassesPerMatch = 0;
            this.numOfPlayersWithMoreFoulsPerMatch = 0;
            this.numOfPlayersWithMoreCornersPerMatch = 0;
            this.numOfPlayersWithMoreYellowCardsPerMatch = 0;
            this.numOfPlayersWithMoreRedCardsPerMatch = 0;
        }
    }
}