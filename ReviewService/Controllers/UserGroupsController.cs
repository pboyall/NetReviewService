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
    public class UserGroupsController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/UserGroups
        public IQueryable<UserGroup> GetUserGroups()
        {
            return db.UserGroups;
        }

        // GET: api/UserGroups/5
        [ResponseType(typeof(UserGroup))]
        public IHttpActionResult GetUserGroup(int id)
        {
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            return Ok(userGroup);
        }

        // PUT: api/UserGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserGroup(int id, UserGroup userGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userGroup.GroupId)
            {
                return BadRequest();
            }

            db.Entry(userGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGroupExists(id))
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

        // POST: api/UserGroups
        [ResponseType(typeof(UserGroup))]
        public IHttpActionResult PostUserGroup(UserGroup userGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserGroups.Add(userGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserGroupExists(userGroup.GroupId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userGroup.GroupId }, userGroup);
        }

        // DELETE: api/UserGroups/5
        [ResponseType(typeof(UserGroup))]
        public IHttpActionResult DeleteUserGroup(int id)
        {
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            db.UserGroups.Remove(userGroup);
            db.SaveChanges();

            return Ok(userGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserGroupExists(int id)
        {
            return db.UserGroups.Count(e => e.GroupId == id) > 0;
        }
    }
}