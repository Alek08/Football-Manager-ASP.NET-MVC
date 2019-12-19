using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Virtual_Football_Manager.Models;

//[Authorize(Roles = RoleNames.ADMIN + ", " + RoleNames.USER)]
namespace Virtual_Football_Manager.Controllers
{
    [Authorize]
    public class FixtureController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fixture
        public ActionResult Index()
        {
            return View(db.fixturesDB.ToList());
        }

        public ActionResult ViewTable()
        {
            List<Team> allTeams = db.teamDB.ToList();
            List<ModelViewTable> model = new List<ModelViewTable>();

            for (int k = 0; k < allTeams.Count; k++) {
                Team t = allTeams.ElementAt(k);

                int win = 0;
                int lost = 0;
                int draw = 0;
                int points = 0;
                for (int i = 0; i < t.matches.ToList().Count; i++){
                    if (t.id == t.matches.ToList().ElementAt(i).homeTeamId){
                        //t e homeTeam vo match i
                        if (t.matches.ToList().ElementAt(i).homeScore >
                            t.matches.ToList().ElementAt(i).awayScore)
                        {
                            win++;
                        }
                        if (t.matches.ToList().ElementAt(i).homeScore <
                            t.matches.ToList().ElementAt(i).awayScore)
                        {
                            lost++;
                        }
                        if (t.matches.ToList().ElementAt(i).homeScore ==
                          t.matches.ToList().ElementAt(i).awayScore)
                        {
                            draw++;
                        }
                    }
                    else {
                        //t e awayTeam vo match i
                        if (t.matches.ToList().ElementAt(i).awayScore >
                         t.matches.ToList().ElementAt(i).homeScore)
                        {
                            win++;
                        }
                        if (t.matches.ToList().ElementAt(i).awayScore <
                            t.matches.ToList().ElementAt(i).homeScore)
                        {
                            lost++;
                        }
                        if (t.matches.ToList().ElementAt(i).awayScore ==
                          t.matches.ToList().ElementAt(i).homeScore)
                        {
                            draw++;
                        }
                    }
                }

                points = win * 3 + draw;
                model.Add(new ModelViewTable(t, win, draw, lost, points));

            }

            List<ModelViewTable> sortedModel = model.OrderBy(m => m.points).ToList();
            sortedModel.Reverse();


            return View(sortedModel);
        }
        // GET: Fixture/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = db.fixturesDB.Find(id);

            ModelFixtureAndAllFixtures model = new ModelFixtureAndAllFixtures();
            model.fixture = fixture;
            model.fixtures = db.fixturesDB.ToList();
            for(int i = 0; i < db.fixturesDB.ToList().Count; i++){
                if (db.fixturesDB.ToList().IndexOf(fixture) == 0)
                {
                    model.hasLeft = false;
                }
                else {
                    model.hasLeft = true;
                }

                if (db.fixturesDB.ToList().IndexOf(fixture) == db.fixturesDB.ToList().Count - 1)
                {
                    model.hasRight = false;
                }
                else {
                    model.hasRight = true;
                }

            }



            if (fixture == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Fixture/Create
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Create()
        {
            if (db.teamDB.ToList().Count == 0) {
                return Content("No teams to create Fixture");
            }
            if (db.teamDB.ToList().Count % 2 != 0)
            {
                return Content("The number of teams should be even number");
            }
            int numOfTotalTeams = db.teamDB.ToList().Count;
            int numOfTotalMatches = db.matchDB.ToList().Count;
            if ((numOfTotalTeams * (numOfTotalTeams - 1)) / 2 == numOfTotalMatches) {
                return Content("No more fixtures to add!");
            }


            Fixture f = new Fixture();
            f.name ="Fixture " + (db.fixturesDB.ToList().Count+1);
            return View(f);
        }

        // POST: Fixture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Create([Bind(Include = "id,name")] Fixture fixture)
        {
            if (db.teamDB.ToList().Count == 0)
            {
                return Content("No teams to create Fixture");
            }
            if (db.teamDB.ToList().Count % 2 != 0)
            {
                return Content("The number of teams should be even number");
            }
            int numOfTotalTeams = db.teamDB.ToList().Count;
            int numOfTotalMatches = db.matchDB.ToList().Count;
            if ((numOfTotalTeams * (numOfTotalTeams - 1)) / 2 == numOfTotalMatches)
            {
                return Content("No more fixtures to add!");
            }

            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                //int month = rnd.Next(1, 13); // creates a number between 1 and 12

 
                List<Team> allTeams = db.teamDB.ToList();
                for (int j = 0; j < allTeams.Count; j++)
                {
                    Team homeTeam = allTeams.ElementAt(j);
                    for (int r = 0; r < allTeams.Count; r++)
                    {
                        Team awayTeam = allTeams.ElementAt(r);
                        if (homeTeam.Equals(awayTeam))
                        {
                            continue;
                        }
                        if (hasTeamPlayedInThisFixture(fixture, homeTeam) == false &&
                                hasTeamPlayedInThisFixture(fixture, awayTeam) == false)
                        {
                            if (hasTeamEverPlayedWithTeam(homeTeam, awayTeam) == false)
                            {
                                addMatchInFixture(fixture, homeTeam, awayTeam);
                            }
                            else {
                                continue;
                            }
                        }
                    }
                }


                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    System.Diagnostics.Debug.WriteLine("db.SaveChanges() exception");


                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }
               

                return RedirectToAction("Index");
            }

            return View(fixture);
        }


        private bool hasTeamEverPlayedWithTeam(Team homeTeam, Team awayTeam) {
            bool res = false;
            List<Match> allHomeTeamMatches = homeTeam.matches.ToList();
            for (int i = 0; i < allHomeTeamMatches.Count; i++) {
                if (allHomeTeamMatches.ElementAt(i).teams.Contains(awayTeam)) {
                    res = true;
                }
            }
            return res;
        }

        private bool hasTeamPlayedInThisFixture(Fixture fixture,Team team) {
            List<Match> allMatchesInThisFixture = fixture.matches.ToList();
            bool hasPlayed = false;

            for (int i = 0; i < allMatchesInThisFixture.Count; i++) {
                if (allMatchesInThisFixture.ElementAt(i).teams.Contains(team)) {
                    hasPlayed = true;
                }
            }
            return hasPlayed;
        }


        private void addMatchInFixture(Fixture fixture, Team homeTeam, Team awayTeam) {
         

            Random rnd = new Random();

            Match match = new Match(rnd.Next(0, 4), rnd.Next(0, 4), fixture.id,homeTeam,awayTeam);
        

            //add goals scored by home team
            for (int i = 0; i < match.homeScore; i++)
            {
                Goal goal1 = new Goal();
                FootballPlayer scorer = homeTeam.footballPlayers.ToList().ElementAt(
                    rnd.Next(0, homeTeam.footballPlayers.ToList().Count)
                    );
                FootballPlayer assist = null;
                while (true)
                {
                    assist = homeTeam.footballPlayers.ToList().ElementAt(
                    rnd.Next(0, homeTeam.footballPlayers.ToList().Count)
                    );
                    if (assist.Equals(scorer) == false)
                    {
                        break;
                    }
                }
                goal1.footballPlayers.Add(scorer);
                goal1.footballPlayers.Add(assist);
                goal1.matchId = match.id;
                goal1.minuteScored = rnd.Next(1,90);

                match.goals.Add(goal1);
                db.goalsDB.Add(goal1);
            }

            //add goals scored by away team
            for (int i = 0; i < match.awayScore; i++)
            {
                Goal goal1 = new Goal();
                FootballPlayer scorer = awayTeam.footballPlayers.ToList().ElementAt(
                    rnd.Next(0, awayTeam.footballPlayers.ToList().Count)
                    );
                FootballPlayer assist = null;
                while (true)
                {
                    assist = awayTeam.footballPlayers.ToList().ElementAt(
                    rnd.Next(0, awayTeam.footballPlayers.ToList().Count)
                    );
                    if (assist.Equals(scorer) == false)
                    {
                        break;
                    }
                }
                goal1.footballPlayers.Add(scorer);
                goal1.footballPlayers.Add(assist);
                goal1.matchId = match.id;
                goal1.minuteScored = rnd.Next(1, 90);

                match.goals.Add(goal1);
                db.goalsDB.Add(goal1);
            }







            db.matchDB.Add(match);
             
            //stats
            MatchStats matchStats = new MatchStats();
            match.matchStats = matchStats;
            
            matchStats.homeBallPosession = rnd.Next(20,81);
            matchStats.awayBallPosession = 100 - matchStats.homeBallPosession;


            matchStats.homeTotalShoots = rnd.Next(match.homeScore, 15);
            matchStats.awayTotalShoots = rnd.Next(match.awayScore, 15);
            matchStats.homePasses = rnd.Next(200,700);
            matchStats.awayPasses = rnd.Next(200, 700);
            matchStats.homeFouls = rnd.Next(5, 15);
            matchStats.awayFouls = rnd.Next(5, 15);
            matchStats.homeCorners = rnd.Next(3, 8);
            matchStats.awayCorners = rnd.Next(3, 8);
            matchStats.homeYellowCards = rnd.Next(0, 3);
            matchStats.awayYellowCards = rnd.Next(0, 3);
            matchStats.homeRedCards = rnd.Next(0, 2);
            matchStats.awayRedCards = rnd.Next(0, 2);


            db.matchStatsDB.Add(matchStats);
            
            //stats

           
            fixture.matches.Add(match);
            
           
            db.fixturesDB.Add(fixture);

            
        }

        /*
        // GET: Fixture/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = db.fixturesDB.Find(id);
            if (fixture == null)
            {
                return HttpNotFound();
            }
            return View(fixture);
        }

        // POST: Fixture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fixture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fixture);
        }
        */

        // GET: Fixture/Delete/5
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = db.fixturesDB.Find(id);
            if (fixture == null)
            {
                return HttpNotFound();
            }
            return View(fixture);
        }

        // POST: Fixture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult DeleteConfirmed(int id)
        {
            Fixture fixture = db.fixturesDB.Find(id);

            //moj kod
            List<Match> matches = fixture.matches.ToList();
            for (int i = 0; i < matches.Count; i++)
            {
                Match m = matches.ElementAt(i);
                MatchStats ms = m.matchStats;
                db.matchStatsDB.Remove(ms);

                db.matchDB.Remove(m);
            }
            //moj kod


            db.fixturesDB.Remove(fixture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
