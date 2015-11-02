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
using ReviewService.Models;

namespace ReviewService.Controllers
{
    public class BranchConditionsController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/BranchConditions
        public IQueryable<BranchCondition> GetBranchConditions()
        {
            return db.BranchConditions;
        }

        // GET: api/BranchConditions/5
        [ResponseType(typeof(BranchCondition))]
        public IHttpActionResult GetBranchCondition(int id)
        {
            BranchCondition branchCondition = db.BranchConditions.Find(id);
            if (branchCondition == null)
            {
                return NotFound();
            }

            return Ok(branchCondition);
        }

        // PUT: api/BranchConditions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchCondition(int id, BranchCondition branchCondition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branchCondition.ConditionId)
            {
                return BadRequest();
            }

            db.Entry(branchCondition).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchConditionExists(id))
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

        // POST: api/BranchConditions
        [ResponseType(typeof(BranchCondition))]
        public IHttpActionResult PostBranchCondition(BranchCondition branchCondition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BranchConditions.Add(branchCondition);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = branchCondition.ConditionId }, branchCondition);
        }

        // DELETE: api/BranchConditions/5
        [ResponseType(typeof(BranchCondition))]
        public IHttpActionResult DeleteBranchCondition(int id)
        {
            BranchCondition branchCondition = db.BranchConditions.Find(id);
            if (branchCondition == null)
            {
                return NotFound();
            }

            db.BranchConditions.Remove(branchCondition);
            db.SaveChanges();

            return Ok(branchCondition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchConditionExists(int id)
        {
            return db.BranchConditions.Count(e => e.ConditionId == id) > 0;
        }
    }
}