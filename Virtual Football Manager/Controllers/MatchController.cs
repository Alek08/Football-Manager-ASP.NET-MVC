using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Virtual_Football_Manager.Models;

namespace Virtual_Football_Manager.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Match
        public ActionResult Index()
        {
            return View(db.matchDB.Include(m=>m.fixture).ToList());
        }

        // GET: Match/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.matchDB.Include(m=>m.fixture).First(m=>m.id==id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // GET: Match/Create
        /*  public ActionResult Create()
          {
              ModelMatchAndAllTeamsAllFixtures model = new ModelMatchAndAllTeamsAllFixtures();
              model.match = new Match();
              model.teams = db.teamDB.ToList();
              model.fixtures = db.fixturesDB.ToList();
              return View(model);
          }

          // POST: Match/Create
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Create([Bind(Include = "match,teams,homeTeamId,awayTeamId,fixtures")] ModelMatchAndAllTeamsAllFixtures model)
          {
              if (ModelState.IsValid)
              {
                  MatchStats matchStats = new MatchStats();

                  //stats
                  matchStats.id = model.match.id;
                  matchStats.homeBallPosession = -1;
                  matchStats.awayBallPosession = -1;
                  matchStats.homeTotalShoots = -1;
                  matchStats.awayTotalShoots  = -1;
                  matchStats.homePasses = -1;
                  matchStats.awayPasses = -1;
                  matchStats.homeFouls = -1;
                  matchStats.awayFouls = -1;
                  matchStats.homeCorners = -1;
                  matchStats.awayCorners = -1;
                  matchStats.homeYellowCards = -1;
                  matchStats.awayYellowCards = -1;
                  matchStats.homeRedCards = -1;
                  matchStats.awayRedCards = -1;

                  db.matchStatsDB.Add(matchStats);
                  //stats


                  //goals
                  Goal goal = new Goal();

                  goal.footballPlayers.Add(db.footballPlayerDB.Find(1));
                  goal.footballPlayers.Add(db.footballPlayerDB.Find(2));


                  goal.matchId = model.match.id;
                  goal.minuteScored = 50;

                  db.goalsDB.Add(goal);
                  model.match.goals.Add(goal);
                  //goals




                  model.match.teams.Add(db.teamDB.First(m => m.id == model.homeTeamId));
                  model.match.teams.Add(db.teamDB.First(m => m.id == model.awayTeamId));

                  db.matchDB.Add(model.match);
                  db.SaveChanges();
                  return RedirectToAction("Index");
              }

              return View(model);
          }


          // GET: Match/Edit/5
          public ActionResult Edit(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Match match = db.matchDB.Find(id);
              if (match == null)
              {
                  return HttpNotFound();
              }
              return View(match);
          }

          // POST: Match/Edit/5
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit([Bind(Include = "id,homeScore,awayScore")] Match match)
          {
              if (ModelState.IsValid)
              {
                  db.Entry(match).State = EntityState.Modified;
                  db.SaveChanges();
                  return RedirectToAction("Index");
              }
              return View(match);
          }

          // GET: Match/Delete/5
          public ActionResult Delete(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Match match = db.matchDB.Find(id);
              if (match == null)
              {
                  return HttpNotFound();
              }
              return View(match);
          }
          

        // POST: Match/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Match match = db.matchDB.Find(id);
            db.matchDB.Remove(match);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         */
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
