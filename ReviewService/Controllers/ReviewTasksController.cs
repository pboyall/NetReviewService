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
    public class ReviewTasksController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/ReviewTasks
        public IQueryable<ReviewTask> GetReviewTasks()
        {
            return db.ReviewTasks;
        }

        // GET: api/ReviewTasks/5
        [ResponseType(typeof(ReviewTask))]
        public IHttpActionResult GetReviewTask(int id)
        {
            ReviewTask reviewTask = db.ReviewTasks.Find(id);
            if (reviewTask == null)
            {
                return NotFound();
            }

            return Ok(reviewTask);
        }

        // PUT: api/ReviewTasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReviewTask(int id, ReviewTask reviewTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reviewTask.TaskId)
            {
                return BadRequest();
            }

            db.Entry(reviewTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewTaskExists(id))
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

        // POST: api/ReviewTasks
        [ResponseType(typeof(ReviewTask))]
        public IHttpActionResult PostReviewTask(ReviewTask reviewTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReviewTasks.Add(reviewTask);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reviewTask.TaskId }, reviewTask);
        }

        // DELETE: api/ReviewTasks/5
        [ResponseType(typeof(ReviewTask))]
        public IHttpActionResult DeleteReviewTask(int id)
        {
            ReviewTask reviewTask = db.ReviewTasks.Find(id);
            if (reviewTask == null)
            {
                return NotFound();
            }

            db.ReviewTasks.Remove(reviewTask);
            db.SaveChanges();

            return Ok(reviewTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewTaskExists(int id)
        {
            return db.ReviewTasks.Count(e => e.TaskId == id) > 0;
        }
    }
}