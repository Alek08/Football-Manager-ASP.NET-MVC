using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class APIModelFootballPlayer
    {

        public int playerId { get; set; }
 
        public String playerName { get; set; }
 
        public String  teamName { get; set; }

        public int teamId { get; set; }

        public APIModelFootballPlayer(int playerId, String playerName, int teamId, String teamName) {
            this.playerId = playerId;
            this.playerName = playerName;
            this.teamId = teamId;
            this.teamName = teamName;
        }

    }
}