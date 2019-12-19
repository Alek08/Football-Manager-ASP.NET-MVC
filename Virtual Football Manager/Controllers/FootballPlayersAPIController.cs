using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Virtual_Football_Manager.Models;

namespace Virtual_Football_Manager.Controllers
{
    public class FootballPlayersAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FootballPlayersAPI
        public List<APIModelFootballPlayer> GetfootballPlayerDB()
        {
            
            List<FootballPlayer> allPlayers = db.footballPlayerDB.Include(f=>f.team).ToList();
            List<APIModelFootballPlayer> model = new List<APIModelFootballPlayer>();

            for (int i = 0; i < allPlayers.Count; i++) {
                FootballPlayer p = allPlayers.ElementAt(i);
                model.Add(new APIModelFootballPlayer(p.id, p.name,p.team.id, p.team.name));
            }

            return model;
        }

        // GET: api/FootballPlayersAPI/5
        [ResponseType(typeof(FootballPlayer))]
        public IHttpActionResult GetFootballPlayer(int id)
        {
            FootballPlayer footballPlayer = db.footballPlayerDB.Find(id);
            if (footballPlayer == null)
            {
                return NotFound();
            }

            return Ok(footballPlayer);
        }

        // PUT: api/FootballPlayersAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFootballPlayer(int id, FootballPlayer footballPlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != footballPlayer.id)
            {
                return BadRequest();
            }

            db.Entry(footballPlayer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FootballPlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FootballPlayersAPI
        [ResponseType(typeof(FootballPlayer))]
        public IHttpActionResult PostFootballPlayer(FootballPlayer footballPlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.footballPlayerDB.Add(footballPlayer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = footballPlayer.id }, footballPlayer);
        }

        // DELETE: api/FootballPlayersAPI/5
        [ResponseType(typeof(FootballPlayer))]
        public IHttpActionResult DeleteFootballPlayer(int id)
        {
            FootballPlayer footballPlayer = db.footballPlayerDB.Find(id);
            if (footballPlayer == null)
            {
                return NotFound();
            }

            db.footballPlayerDB.Remove(footballPlayer);
            db.SaveChanges();

            return Ok(footballPlayer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FootballPlayerExists(int id)
        {
            return db.footballPlayerDB.Count(e => e.id == id) > 0;
        }
    }
}