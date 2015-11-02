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
    public class TaskAssignmentHistoriesController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/TaskAssignmentHistories
        public IQueryable<TaskAssignmentHistory> GetTaskAssignmentHistories()
        {
            return db.TaskAssignmentHistories;
        }

        // GET: api/TaskAssignmentHistories/5
        [ResponseType(typeof(TaskAssignmentHistory))]
        public IHttpActionResult GetTaskAssignmentHistory(int id)
        {
            TaskAssignmentHistory taskAssignmentHistory = db.TaskAssignmentHistories.Find(id);
            if (taskAssignmentHistory == null)
            {
                return NotFound();
            }

            return Ok(taskAssignmentHistory);
        }

        // PUT: api/TaskAssignmentHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskAssignmentHistory(int id, TaskAssignmentHistory taskAssignmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskAssignmentHistory.TaskAssignmentHistoryId)
            {
                return BadRequest();
            }

            db.Entry(taskAssignmentHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskAssignmentHistoryExists(id))
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

        // POST: api/TaskAssignmentHistories
        [ResponseType(typeof(TaskAssignmentHistory))]
        public IHttpActionResult PostTaskAssignmentHistory(TaskAssignmentHistory taskAssignmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskAssignmentHistories.Add(taskAssignmentHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = taskAssignmentHistory.TaskAssignmentHistoryId }, taskAssignmentHistory);
        }

        // DELETE: api/TaskAssignmentHistories/5
        [ResponseType(typeof(TaskAssignmentHistory))]
        public IHttpActionResult DeleteTaskAssignmentHistory(int id)
        {
            TaskAssignmentHistory taskAssignmentHistory = db.TaskAssignmentHistories.Find(id);
            if (taskAssignmentHistory == null)
            {
                return NotFound();
            }

            db.TaskAssignmentHistories.Remove(taskAssignmentHistory);
            db.SaveChanges();

            return Ok(taskAssignmentHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskAssignmentHistoryExists(int id)
        {
            return db.TaskAssignmentHistories.Count(e => e.TaskAssignmentHistoryId == id) > 0;
        }
    }
}