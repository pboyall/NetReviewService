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
    public class BranchNodesController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/BranchNodes
        public IQueryable<BranchNode> GetBranchNodes()
        {
            return db.BranchNodes;
        }

        // GET: api/BranchNodes/5
        [ResponseType(typeof(BranchNode))]
        public IHttpActionResult GetBranchNode(int id)
        {
            BranchNode branchNode = db.BranchNodes.Find(id);
            if (branchNode == null)
            {
                return NotFound();
            }

            return Ok(branchNode);
        }

        // PUT: api/BranchNodes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchNode(int id, BranchNode branchNode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branchNode.GuardId)
            {
                return BadRequest();
            }

            db.Entry(branchNode).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchNodeExists(id))
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

        // POST: api/BranchNodes
        [ResponseType(typeof(BranchNode))]
        public IHttpActionResult PostBranchNode(BranchNode branchNode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BranchNodes.Add(branchNode);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = branchNode.GuardId }, branchNode);
        }

        // DELETE: api/BranchNodes/5
        [ResponseType(typeof(BranchNode))]
        public IHttpActionResult DeleteBranchNode(int id)
        {
            BranchNode branchNode = db.BranchNodes.Find(id);
            if (branchNode == null)
            {
                return NotFound();
            }

            db.BranchNodes.Remove(branchNode);
            db.SaveChanges();

            return Ok(branchNode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchNodeExists(int id)
        {
            return db.BranchNodes.Count(e => e.GuardId == id) > 0;
        }
    }
}