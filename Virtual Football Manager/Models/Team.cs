using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class Team  
    {
        [Display(Name = "Team ID")]
        [Required]
        public int id { get; set; }


        [Display(Name = "Team Name")]
        [Required]
        public String name{ get; set; }


        public virtual ICollection<FootballPlayer> footballPlayers { get; set; }

        public virtual ICollection<Match> matches { get; set; }


        public Team() {
            this.matches = new HashSet<Match>();
            this.footballPlayers = new HashSet<FootballPlayer>();
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Team p = (Team)obj;
            return (this.id == p.id);
        }

        public override string ToString()
        {
            return this.name;
        }


    }
}