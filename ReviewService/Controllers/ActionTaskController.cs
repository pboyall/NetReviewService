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

    public class workflow
    {

        //Flip this around to be dependency injection later
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();
        public int ApprovalProcessId { get; set; }
        public int ApproverUserId { get; set; }
        public int TaskId { get; set; }

        public void ActionTask(string Action)
        {

        }

    }


        public class ActionTaskController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/ActionTask
        public IQueryable<TaskAssignment> GetTaskAssignments()
        {
            return db.TaskAssignments;
        }

        // GET: api/ActionTask/5
        [ResponseType(typeof(TaskAssignment))]
        public IHttpActionResult GetTaskAssignment(int id)
        {
            TaskAssignment taskAssignment = db.TaskAssignments.Find(id);
            if (taskAssignment == null)
            {
                return NotFound();
            }

            return Ok(taskAssignment);
        }

        // PUT: api/ActionTask/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskAssignment(int id, TaskAssignment taskAssignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskAssignment.TaskId)
            {
                return BadRequest();
            }

            db.Entry(taskAssignment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskAssignmentExists(id))
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

        // POST: api/ActionTask
        [ResponseType(typeof(TaskAssignment))]
        public IHttpActionResult PostTaskAssignment(TaskAssignment taskAssignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskAssignments.Add(taskAssignment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TaskAssignmentExists(taskAssignment.TaskId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = taskAssignment.TaskId }, taskAssignment);
        }

        // DELETE: api/ActionTask/5
        [ResponseType(typeof(TaskAssignment))]
        public IHttpActionResult DeleteTaskAssignment(int id)
        {
            TaskAssignment taskAssignment = db.TaskAssignments.Find(id);
            if (taskAssignment == null)
            {
                return NotFound();
            }

            db.TaskAssignments.Remove(taskAssignment);
            db.SaveChanges();

            return Ok(taskAssignment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskAssignmentExists(int id)
        {
            return db.TaskAssignments.Count(e => e.TaskId == id) > 0;
        }
    }
}