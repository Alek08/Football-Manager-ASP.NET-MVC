using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class Fixture
    {

        [Display(Name = "Fixture ID")]
        [Required]
        public int id { get; set; }

        [Display(Name = "Fixture name")]
        //[Required]
        public String name { get; set; }


        public virtual ICollection<Match> matches { get; set; }


        public Fixture()
        {
            this.matches = new HashSet<Match>();
        }


        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Fixture p = (Fixture)obj;
            return (this.id == p.id);
        }

    }
}