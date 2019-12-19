using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelViewTable
    {
        public Team team { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lost { get; set; }
        public int points { get; set; }

        public ModelViewTable(Team team,int win,int draw,int lost,int points) {
            this.team = team;
            this.win = win;
            this.draw = draw;
            this.lost = lost;
            this.points = points;
        }

    }
}