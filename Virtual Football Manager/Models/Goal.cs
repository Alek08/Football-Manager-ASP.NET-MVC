using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class Goal
    {

        [Display(Name = "Goal ID")]
        [Required]
        public int id { get; set; }

 

        public virtual ICollection<FootballPlayer> footballPlayers { get; set; }//[0]-scorer [1]-assisted by


        [Display(Name = "Goal matchId")]
        [Required]
        public int matchId { get; set; }
        [ForeignKey("matchId")]
        public virtual Match match { get; set; }



        [Display(Name = "Goal minuteScored")]
        [Required]
        public int minuteScored { get; set; }

      

        public Goal() {
            this.footballPlayers = new HashSet<FootballPlayer>();
        }

    }
}