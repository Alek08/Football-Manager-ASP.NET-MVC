using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class FootballPlayer  
    {


        [Display(Name = "FootballPlayer ID")]
        [Required]
        public int id { get; set; }


        [Display(Name = "Player Name")]
        [Required]
        public String name { get; set; }

        [Display(Name = "FootballPlayer Teamid")]
        [Required]
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team team { get; set; }

       
        public virtual ICollection<Goal> goals { get; set; }

       
        public virtual ICollection<MyVirtualTeam> virtualTeams { get; set; }

        public FootballPlayer() {
            this.goals= new HashSet<Goal>();
        }


        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            FootballPlayer p = (FootballPlayer)obj;
            return (this.id == p.id);
        }

    }
}