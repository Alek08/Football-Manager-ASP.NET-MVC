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
    public class FootballPlayerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FootballPlayer
        public ActionResult Index()
        {
            var footballPlayerDB = db.footballPlayerDB.Include(f => f.team);
            return View(footballPlayerDB.ToList());
        }

        // GET: FootballPlayer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootballPlayer footballPlayer = db.footballPlayerDB
                                     .Include(m => m.team)
                                     .Include(m => m.goals.Select(g=>g.match.fixture))
                                     .First(m => m.id == id);
                                                    
            if (footballPlayer == null)
            {
                return HttpNotFound();
            }
            return View(footballPlayer);
        }

        // GET: FootballPlayer/Create
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Create(int? teamId)
        {
            Team t = db.teamDB.Find(teamId);
            if (t == null) {
                return Content("Team not found");
            }

            ModelAddFootballPlayerToTeam model = new ModelAddFootballPlayerToTeam();
           
            model.footballPlayer = new FootballPlayer();
            model.footballPlayer.TeamId = t.id;
            model.team = t;
            return View(model);
        }

        // POST: FootballPlayer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Create([Bind(Include = "footballPlayer,team")] ModelAddFootballPlayerToTeam model)
        {
            Team t = db.teamDB.Find(model.footballPlayer.TeamId);
            if (t.footballPlayers.ToList().Count == 11) {
                return Content("The team " + t.name + " has 11 players. Adding "+model.footballPlayer.name+" failed");
            }
            if (ModelState.IsValid)
            {
                
                db.footballPlayerDB.Add(model.footballPlayer);
                db.SaveChanges();
                return RedirectToAction("Details", "Team", new { id =  model.footballPlayer.TeamId });
            }

            
            return View(model);
        }

        // GET: FootballPlayer/Edit/5
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootballPlayer footballPlayer = db.footballPlayerDB.Find(id);

            if (footballPlayer == null)
            {
                return HttpNotFound();
            }
            
            return View(footballPlayer);
        }

        // POST: FootballPlayer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Edit([Bind(Include = "id,name,TeamId")] FootballPlayer footballPlayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footballPlayer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(footballPlayer);
        }

        // GET: FootballPlayer/Delete/5
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootballPlayer footballPlayer = db.footballPlayerDB.Find(id);
            if (footballPlayer == null)
            {
                return HttpNotFound();
            }
            return View(footballPlayer);
        }

        // POST: FootballPlayer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public ActionResult DeleteConfirmed(int id)
        {
            FootballPlayer footballPlayer = db.footballPlayerDB.Find(id);
            db.footballPlayerDB.Remove(footballPlayer);
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
