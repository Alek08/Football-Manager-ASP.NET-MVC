using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class MatchStats
    {

        [Display(Name = "MatchStats ID")]
        [ForeignKey("Match")]
        public int id { get; set; }


        public virtual Match Match { get; set; }




        [Display(Name = "Ball Posession")]
        [Required]
        public int homeBallPosession { get; set; }

        [Display(Name = "Ball Posession")]
        [Required]
        public int awayBallPosession { get; set; }

        [Display(Name = "Total Shoots")]
        [Required]
        public int homeTotalShoots { get; set; }

        [Display(Name = "Total Shoots")]
        [Required]
        public int awayTotalShoots { get; set; }

        [Display(Name = "Passes")]
        [Required]
        public int homePasses { get; set; }

        [Display(Name = "Passes")]
        [Required]
        public int awayPasses { get; set; }

        [Display(Name = "Fouls")]
        [Required]
        public int homeFouls { get; set; }

        [Display(Name = "Fouls")]
        [Required]
        public int awayFouls { get; set; }

        [Display(Name = "Corners")]
        [Required]
        public int homeCorners { get; set; }

        [Display(Name = "Corners")]
        [Required]
        public int awayCorners { get; set; }

        [Display(Name = "Yellow Cards")]
        [Required]
        public int homeYellowCards { get; set; }

        [Display(Name = "Yellow Cards")]
        [Required]
        public int awayYellowCards { get; set; }

        [Display(Name = "Red Cards")]
        [Required]
        public int homeRedCards { get; set; }

        [Display(Name = "Red Cards")]
        [Required]
        public int awayRedCards { get; set; }


    }
}