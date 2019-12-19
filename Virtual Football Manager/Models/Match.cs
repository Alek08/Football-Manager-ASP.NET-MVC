using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class Match
    {
        [Display(Name = "Match ID")]
        [Required]
        public int id { get; set; }


        [Display(Name = "homeScore")]
        [Required]
        public int homeScore { get; set; }

        [Display(Name = "awayScore")]
        [Required]
        public int awayScore { get; set; }

        [Display(Name = "Match Fixture")]
        [Required]
        public int fixtureId { get; set; }

        [ForeignKey("fixtureId")]
        public Fixture fixture { get; set; }


        public int homeTeamId { get; set; }

        public int awayTeamId { get; set; }

        public virtual ICollection<Team> teams { get; set; }//[0]-home team [1]-away team
    
        public virtual ICollection<Goal> goals { get; set; }

        public virtual MatchStats matchStats { get; set; }

        public Match(int homeScore, int awayScore, int fixtureId, Team homeTeam, Team awayTeam) {
            this.teams = new HashSet<Team>();
            this.goals = new HashSet<Goal>();

            this.homeScore = homeScore;
            this.awayScore = awayScore;
            this.fixtureId = fixtureId;
            this.teams.Add(homeTeam);
            this.teams.Add(awayTeam);
            this.homeTeamId = homeTeam.id;
            this.awayTeamId = awayTeam.id;
        }
        public Match() {
        }

        public override string ToString()
        {
            return this.teams.ElementAt(0)+" vs "+this.teams.ElementAt(1);
        }
    }
}