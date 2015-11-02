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
    public class NodesController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/Nodes
        public IQueryable<Node> GetNodes()
        {
            return db.Nodes;
        }

        // GET: api/Nodes/5
        [ResponseType(typeof(Node))]
        public IHttpActionResult GetNode(int id)
        {
            Node node = db.Nodes.Find(id);
            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }

        // PUT: api/Nodes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNode(int id, Node node)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != node.NodeId)
            {
                return BadRequest();
            }

            db.Entry(node).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NodeExists(id))
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

        // POST: api/Nodes
        [ResponseType(typeof(Node))]
        public IHttpActionResult PostNode(Node node)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Nodes.Add(node);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = node.NodeId }, node);
        }

        // DELETE: api/Nodes/5
        [ResponseType(typeof(Node))]
        public IHttpActionResult DeleteNode(int id)
        {
            Node node = db.Nodes.Find(id);
            if (node == null)
            {
                return NotFound();
            }

            db.Nodes.Remove(node);
            db.SaveChanges();

            return Ok(node);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NodeExists(int id)
        {
            return db.Nodes.Count(e => e.NodeId == id) > 0;
        }
    }
}