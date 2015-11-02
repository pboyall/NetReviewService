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
    public class AccessTypesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AccessTypes
        public IQueryable<AccessType> GetAccessTypes()
        {
            return db.AccessTypes;
        }

        // GET: api/AccessTypes/5
        [ResponseType(typeof(AccessType))]
        public IHttpActionResult GetAccessType(int id)
        {
            AccessType accessType = db.AccessTypes.Find(id);
            if (accessType == null)
            {
                return NotFound();
            }

            return Ok(accessType);
        }

        // PUT: api/AccessTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccessType(int id, AccessType accessType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accessType.AccessTypeId)
            {
                return BadRequest();
            }

            db.Entry(accessType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessTypeExists(id))
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

        // POST: api/AccessTypes
        [ResponseType(typeof(AccessType))]
        public IHttpActionResult PostAccessType(AccessType accessType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AccessTypes.Add(accessType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = accessType.AccessTypeId }, accessType);
        }

        // DELETE: api/AccessTypes/5
        [ResponseType(typeof(AccessType))]
        public IHttpActionResult DeleteAccessType(int id)
        {
            AccessType accessType = db.AccessTypes.Find(id);
            if (accessType == null)
            {
                return NotFound();
            }

            db.AccessTypes.Remove(accessType);
            db.SaveChanges();

            return Ok(accessType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccessTypeExists(int id)
        {
            return db.AccessTypes.Count(e => e.AccessTypeId == id) > 0;
        }
    }
}