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
    public class TaskNodesController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/TaskNodes
        public IQueryable<TaskNode> GetTaskNodes()
        {
            return db.TaskNodes;
        }

        // GET: api/TaskNodes/5
        [ResponseType(typeof(TaskNode))]
        public IHttpActionResult GetTaskNode(int id)
        {
            TaskNode taskNode = db.TaskNodes.Find(id);
            if (taskNode == null)
            {
                return NotFound();
            }

            return Ok(taskNode);
        }

        // PUT: api/TaskNodes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskNode(int id, TaskNode taskNode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskNode.TaskNodeId)
            {
                return BadRequest();
            }

            db.Entry(taskNode).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskNodeExists(id))
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

        // POST: api/TaskNodes
        [ResponseType(typeof(TaskNode))]
        public IHttpActionResult PostTaskNode(TaskNode taskNode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskNodes.Add(taskNode);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = taskNode.TaskNodeId }, taskNode);
        }

        // DELETE: api/TaskNodes/5
        [ResponseType(typeof(TaskNode))]
        public IHttpActionResult DeleteTaskNode(int id)
        {
            TaskNode taskNode = db.TaskNodes.Find(id);
            if (taskNode == null)
            {
                return NotFound();
            }

            db.TaskNodes.Remove(taskNode);
            db.SaveChanges();

            return Ok(taskNode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskNodeExists(int id)
        {
            return db.TaskNodes.Count(e => e.TaskNodeId == id) > 0;
        }
    }
}