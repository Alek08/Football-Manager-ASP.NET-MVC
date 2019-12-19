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
    public class MyVirtualTeamController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyVirtualTeam
        public ActionResult Index()
        {    
            var myVirtualTeamDB = db.myVirtualTeamDB.Include(m => m.user);

            List<ModelViewVirtualTeamTable> model = new List<ModelViewVirtualTeamTable>();
            List<MyVirtualTeam> allVirtualTeams = myVirtualTeamDB.ToList();

            for (int i = 0; i < allVirtualTeams.Count; i++) {
                if (allVirtualTeams.ElementAt(i).lockedTeam==false) {
                    continue;
                }
                ModelViewVirtualTeamTable m = new ModelViewVirtualTeamTable();
                m.virtualTeam = allVirtualTeams.ElementAt(i);
                m.points = countPointsForVirtualTeam(m.virtualTeam);
                model.Add(m);
            }

            List<ModelViewVirtualTeamTable> sortedModel = model.OrderBy(m => m.points).ToList();
            sortedModel.Reverse();

            return View(sortedModel);
        }

       

        // GET: MyVirtualTeam/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(id);
            if (myVirtualTeam == null)
            {
                return HttpNotFound();
            }
            return View(myVirtualTeam);
        }

        
        // GET: MyVirtualTeam/Create
        public ActionResult Create()
        {
            ModelAddFootballPlayersToVirtualTeam model = new ModelAddFootballPlayersToVirtualTeam();
            model.footballPlayers = db.footballPlayerDB.Include(m=>m.team).ToList();

            String userName = System.Web.HttpContext.Current.User.Identity.Name;
            ApplicationUser currUser = db.Users.First(m => m.UserName == userName);

            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(currUser.Id);
            model.virtualTeam = myVirtualTeam;

            if (myVirtualTeam != null) {
                model.myVirtualTeamName = myVirtualTeam.teamName;
            }
          


            return View(model);
        }

        // POST: MyVirtualTeam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "myVirtualTeamName")] ModelAddFootballPlayersToVirtualTeam model)
        {
            String userName = System.Web.HttpContext.Current.User.Identity.Name;

            MyVirtualTeam myVirtualTeam = new MyVirtualTeam();
            myVirtualTeam.user = db.Users.First(m => m.UserName == userName);
            myVirtualTeam.teamName = model.myVirtualTeamName;

            if (ModelState.IsValid)
            {
                db.myVirtualTeamDB.Add(myVirtualTeam);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(model);
        }


        public ActionResult AddPlayerToVirtualTeam(int? playerId)
        {

            String userName = System.Web.HttpContext.Current.User.Identity.Name;
            ApplicationUser currUser = db.Users.First(m => m.UserName == userName);

            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(currUser.Id);

            FootballPlayer f = db.footballPlayerDB.Find(playerId);
            if (myVirtualTeam == null) {
                return Content("Please create a team first");
            }

            if (myVirtualTeam.footballPlayers.Count >= 11) {
                return Content("You have 11(max) number of players");
            }
            myVirtualTeam.footballPlayers.Add(f);
            db.SaveChanges();

            return RedirectToAction("Create");
        }

        public ActionResult RemovePlayerFromVirtualTeam(int? playerId)
        {

            String userName = System.Web.HttpContext.Current.User.Identity.Name;
            ApplicationUser currUser = db.Users.First(m => m.UserName == userName);

            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.First(m => m.id == currUser.Id);

            FootballPlayer f = db.footballPlayerDB.Find(playerId);

            myVirtualTeam.footballPlayers.Remove(f);
            db.SaveChanges();

            return RedirectToAction("Create");
        }



        public ActionResult LockVirtualTeam()
        {

            String userName = System.Web.HttpContext.Current.User.Identity.Name;
            ApplicationUser currUser = db.Users.First(m => m.UserName == userName);

            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.First(m => m.id == currUser.Id);
            if (myVirtualTeam.footballPlayers.Count != 11)
            {
                return Content("You need to add 11 players to your virtual team");
            }

            myVirtualTeam.lockedTeam = true;

            if (ModelState.IsValid)
            {
                db.Entry(myVirtualTeam).State = EntityState.Modified;
                db.SaveChanges();
            }

            
            return RedirectToAction("Create");
        }

        public ActionResult RulesPoints() {
            return View();
        }


        private int countPointsForVirtualTeam(MyVirtualTeam virtualTeam)
        {
            int totalPoints = 0;
           
            List<Fixture> allFixtures = db.fixturesDB.ToList();

            List<ModelViewEarnedPointsForPlayer> model = new List<ModelViewEarnedPointsForPlayer>();
            for (int i = 0; i < virtualTeam.footballPlayers.Count; i++)
            {
                FootballPlayer player = virtualTeam.footballPlayers.ElementAt(i);
                ModelViewEarnedPointsForPlayer m = new ModelViewEarnedPointsForPlayer();
                m.footballPlayer = player;

                for (int j = 0; j < allFixtures.Count; j++)
                {
                    Fixture fixture = allFixtures.ElementAt(j);
                    EarnedPointsPerFixture e = new EarnedPointsPerFixture();
                    e.fixture = fixture;


                    for (int k = 0; k < fixture.matches.Count; k++)
                    {
                        Match match = fixture.matches.ElementAt(k);

                        for (int t = 0; t < match.goals.Count; t++)
                        {
                            if (match.goals.ElementAt(t).footballPlayers.ElementAt(0).id == player.id)
                            {
                                //scorer
                                e.numOfGoalsScored++;
                            }
                            if (match.goals.ElementAt(t).footballPlayers.ElementAt(1).id == player.id)
                            {
                                //goal assisted by
                                e.numOfGoalAssists++;
                            }
                        }

                        if (player.TeamId == match.homeTeamId)
                        {
                            if (match.homeScore > match.awayScore)
                            {
                                e.numOfTeamWinsForPlayer = 1;
                            }
                            if (match.matchStats.homeBallPosession > match.matchStats.awayBallPosession)
                            {
                                e.numOfPlayersWithMoreBallPosessionPerMatch = 1;
                            }
                            if (match.matchStats.homeTotalShoots > match.matchStats.awayTotalShoots)
                            {
                                e.numOfPlayersWithMoreBallTotalShootsPerMatch = 1;
                            }
                            if (match.matchStats.homePasses > match.matchStats.awayPasses)
                            {
                                e.numOfPlayersWithMorePassesPerMatch = 1;
                            }
                            if (match.matchStats.homeFouls > match.matchStats.awayFouls)
                            {
                                e.numOfPlayersWithMoreFoulsPerMatch = 1;
                            }
                            if (match.matchStats.homeCorners > match.matchStats.awayCorners)
                            {
                                e.numOfPlayersWithMoreCornersPerMatch = 1;
                            }
                            if (match.matchStats.homeYellowCards > match.matchStats.awayYellowCards)
                            {
                                e.numOfPlayersWithMoreYellowCardsPerMatch = 1;
                            }
                            if (match.matchStats.homeRedCards > match.matchStats.awayRedCards)
                            {
                                e.numOfPlayersWithMoreRedCardsPerMatch = 1;
                            }
                        }
                        if (player.TeamId == match.awayTeamId)
                        {
                            if (match.awayScore > match.homeScore)
                            {
                                e.numOfTeamWinsForPlayer = 1;
                            }
                            if (match.matchStats.awayBallPosession > match.matchStats.homeBallPosession)
                            {
                                e.numOfPlayersWithMoreBallPosessionPerMatch = 1;
                            }
                            if (match.matchStats.awayTotalShoots > match.matchStats.homeTotalShoots)
                            {
                                e.numOfPlayersWithMoreBallTotalShootsPerMatch = 1;
                            }
                            if (match.matchStats.awayPasses > match.matchStats.homePasses)
                            {
                                e.numOfPlayersWithMorePassesPerMatch = 1;
                            }
                            if (match.matchStats.awayFouls > match.matchStats.homeFouls)
                            {
                                e.numOfPlayersWithMoreFoulsPerMatch = 1;
                            }
                            if (match.matchStats.awayCorners > match.matchStats.homeCorners)
                            {
                                e.numOfPlayersWithMoreCornersPerMatch = 1;
                            }
                            if (match.matchStats.awayYellowCards > match.matchStats.homeYellowCards)
                            {
                                e.numOfPlayersWithMoreYellowCardsPerMatch = 1;
                            }
                            if (match.matchStats.awayRedCards > match.matchStats.homeRedCards)
                            {
                                e.numOfPlayersWithMoreRedCardsPerMatch = 1;
                            }
                        }
                        e.totalPoints = (e.numOfGoalsScored * 2) + e.numOfGoalAssists
                        + e.numOfTeamWinsForPlayer + e.numOfPlayersWithMoreBallPosessionPerMatch
                        + e.numOfPlayersWithMoreBallTotalShootsPerMatch + e.numOfPlayersWithMorePassesPerMatch
                        - e.numOfPlayersWithMoreFoulsPerMatch + e.numOfPlayersWithMoreCornersPerMatch
                        - e.numOfPlayersWithMoreYellowCardsPerMatch - e.numOfPlayersWithMoreRedCardsPerMatch;
                    }

                    m.earnedPointsPerFixture.Add(e);
                }

                model.Add(m);
            }

            for (int y = 0; y < model.Count; y++) {
                ModelViewEarnedPointsForPlayer mTmp = model.ElementAt(y);
                List<EarnedPointsPerFixture> eTmp = mTmp.earnedPointsPerFixture;
                for (int h = 0; h < eTmp.Count; h++) {
                    totalPoints += eTmp.ElementAt(h).totalPoints;
                }
                 
            }
             

            return totalPoints;
        }

        public ActionResult EarnedPoints(string userId)
        {

            List<Fixture> allFixtures = db.fixturesDB.ToList();

            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(userId);
            List<ModelViewEarnedPointsForPlayer> model = new List<ModelViewEarnedPointsForPlayer>();
            for (int i = 0; i < myVirtualTeam.footballPlayers.Count; i++) {
                FootballPlayer player = myVirtualTeam.footballPlayers.ElementAt(i);
                ModelViewEarnedPointsForPlayer m = new ModelViewEarnedPointsForPlayer();
                m.footballPlayer = player;

                for (int j = 0; j < allFixtures.Count; j++) {
                    Fixture fixture = allFixtures.ElementAt(j);
                    EarnedPointsPerFixture e = new EarnedPointsPerFixture();
                    e.fixture = fixture;
                    

                    for (int k = 0; k < fixture.matches.Count; k++) {
                        Match match = fixture.matches.ElementAt(k);

                        for (int t = 0; t < match.goals.Count; t++) {
                            if (match.goals.ElementAt(t).footballPlayers.ElementAt(0).id == player.id) {
                                //scorer
                                e.numOfGoalsScored++;
                            }
                            if (match.goals.ElementAt(t).footballPlayers.ElementAt(1).id == player.id){
                                //goal assisted by
                                e.numOfGoalAssists++;
                            }
                        }

                        if (player.TeamId == match.homeTeamId) {
                            if (match.homeScore > match.awayScore) {
                                e.numOfTeamWinsForPlayer = 1;
                            }
                            if (match.matchStats.homeBallPosession > match.matchStats.awayBallPosession) {
                                e.numOfPlayersWithMoreBallPosessionPerMatch = 1;
                            }
                            if (match.matchStats.homeTotalShoots > match.matchStats.awayTotalShoots){
                                e.numOfPlayersWithMoreBallTotalShootsPerMatch = 1;
                            }
                            if (match.matchStats.homePasses > match.matchStats.awayPasses){
                                e.numOfPlayersWithMorePassesPerMatch = 1;
                            }
                            if (match.matchStats.homeFouls > match.matchStats.awayFouls){
                                e.numOfPlayersWithMoreFoulsPerMatch =1;
                            }
                            if (match.matchStats.homeCorners > match.matchStats.awayCorners){
                                e.numOfPlayersWithMoreCornersPerMatch = 1;
                            }
                            if (match.matchStats.homeYellowCards > match.matchStats.awayYellowCards){
                                e.numOfPlayersWithMoreYellowCardsPerMatch = 1;
                            }
                            if (match.matchStats.homeRedCards > match.matchStats.awayRedCards){
                                e.numOfPlayersWithMoreRedCardsPerMatch = 1;
                            }
                        }
                        if (player.TeamId == match.awayTeamId){
                            if (match.awayScore >  match.homeScore){
                                e.numOfTeamWinsForPlayer = 1;
                            }
                            if (match.matchStats.awayBallPosession > match.matchStats.homeBallPosession){
                                e.numOfPlayersWithMoreBallPosessionPerMatch = 1;
                            }
                            if (match.matchStats.awayTotalShoots > match.matchStats.homeTotalShoots)
                            {
                                e.numOfPlayersWithMoreBallTotalShootsPerMatch = 1;
                            }
                            if (match.matchStats.awayPasses > match.matchStats.homePasses)
                            {
                                e.numOfPlayersWithMorePassesPerMatch = 1;
                            }
                            if (match.matchStats.awayFouls > match.matchStats.homeFouls)
                            {
                                e.numOfPlayersWithMoreFoulsPerMatch = 1;
                            }
                            if (match.matchStats.awayCorners > match.matchStats.homeCorners)
                            {
                                e.numOfPlayersWithMoreCornersPerMatch = 1;
                            }
                            if (match.matchStats.awayYellowCards > match.matchStats.homeYellowCards)
                            {
                                e.numOfPlayersWithMoreYellowCardsPerMatch = 1;
                            }
                            if (match.matchStats.awayRedCards > match.matchStats.homeRedCards)
                            {
                                e.numOfPlayersWithMoreRedCardsPerMatch = 1;
                            }
                        }
                        e.totalPoints = (e.numOfGoalsScored *2)+ e.numOfGoalAssists 
                        + e.numOfTeamWinsForPlayer + e.numOfPlayersWithMoreBallPosessionPerMatch 
                        + e.numOfPlayersWithMoreBallTotalShootsPerMatch + e.numOfPlayersWithMorePassesPerMatch 
                        - e.numOfPlayersWithMoreFoulsPerMatch + e.numOfPlayersWithMoreCornersPerMatch 
                        - e.numOfPlayersWithMoreYellowCardsPerMatch - e.numOfPlayersWithMoreRedCardsPerMatch;

                    }
                    
                    m.earnedPointsPerFixture.Add(e);
                }

                model.Add(m);
            }

            return View(model);
        }




        // GET: MyVirtualTeam/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(id);
            if (myVirtualTeam == null)
            {
                return HttpNotFound();
            }
           
            return View(myVirtualTeam);
        }

        // POST: MyVirtualTeam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,teamName,lockedTeam")] MyVirtualTeam myVirtualTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myVirtualTeam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(myVirtualTeam);
        }
        /*
        // GET: MyVirtualTeam/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(id);
            if (myVirtualTeam == null)
            {
                return HttpNotFound();
            }
            return View(myVirtualTeam);
        }

        // POST: MyVirtualTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MyVirtualTeam myVirtualTeam = db.myVirtualTeamDB.Find(id);
            db.myVirtualTeamDB.Remove(myVirtualTeam);
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
