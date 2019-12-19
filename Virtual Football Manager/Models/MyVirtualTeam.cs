using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class MyVirtualTeam
    {
        [Display(Name = "MyVirtualTeam ID")]

        [ForeignKey("user")]
        public string id { get; set; }
 
        [Display(Name = "Virtual Team")]
        public string teamName{ get; set; }

        public bool lockedTeam { get; set; }

        public virtual ApplicationUser user   { get; set; }

        public virtual ICollection<FootballPlayer> footballPlayers { get; set; }

        public MyVirtualTeam(){
            this.footballPlayers = new HashSet<FootballPlayer>();
            this.lockedTeam = false;
        }
    }
}