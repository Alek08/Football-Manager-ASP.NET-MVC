using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelMatchAndAllTeamsAllFixtures
    {
        public Match match { get; set; }
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public List<Team> teams { get; set; }
        public List<Fixture> fixtures { get; set; }
        public ModelMatchAndAllTeamsAllFixtures() {
            this.teams = new List<Team>();
            this.fixtures = new List<Fixture>();
        }
    }
}