using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Virtual_Football_Manager.Models
{
    public class ModelFixtureAndAllFixtures
    {
        public Fixture fixture;
        public List<Fixture> fixtures;
        public bool hasLeft;
        public bool hasRight;

        public ModelFixtureAndAllFixtures() {
            this.fixtures = new List<Fixture>();

            this.hasLeft = false;
            this.hasRight = false;
        }

    }
}