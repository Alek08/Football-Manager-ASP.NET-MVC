using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelAddFootballPlayersToVirtualTeam
    {
        public List<FootballPlayer> footballPlayers { get; set; }

        public MyVirtualTeam virtualTeam { get; set; }

        [Display(Name = "Virtual Team Name")]
        public string myVirtualTeamName { get; set; }

        public bool virtualTeamCreated{ get; set; }

        public ModelAddFootballPlayersToVirtualTeam() {
            this.footballPlayers = new List<FootballPlayer>();
        }
    }
}