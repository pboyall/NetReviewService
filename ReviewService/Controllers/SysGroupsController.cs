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
    public class SysGroupsController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/SysGroups
        public IQueryable<SysGroup> GetSysGroups()
        {
            return db.SysGroups;
        }

        // GET: api/SysGroups/5
        [ResponseType(typeof(SysGroup))]
        public IHttpActionResult GetSysGroup(int id)
        {
            SysGroup sysGroup = db.SysGroups.Find(id);
            if (sysGroup == null)
            {
                return NotFound();
            }

            return Ok(sysGroup);
        }

        // PUT: api/SysGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSysGroup(int id, SysGroup sysGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sysGroup.GroupId)
            {
                return BadRequest();
            }

            db.Entry(sysGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysGroupExists(id))
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

        // POST: api/SysGroups
        [ResponseType(typeof(SysGroup))]
        public IHttpActionResult PostSysGroup(SysGroup sysGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SysGroups.Add(sysGroup);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sysGroup.GroupId }, sysGroup);
        }

        // DELETE: api/SysGroups/5
        [ResponseType(typeof(SysGroup))]
        public IHttpActionResult DeleteSysGroup(int id)
        {
            SysGroup sysGroup = db.SysGroups.Find(id);
            if (sysGroup == null)
            {
                return NotFound();
            }

            db.SysGroups.Remove(sysGroup);
            db.SaveChanges();

            return Ok(sysGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SysGroupExists(int id)
        {
            return db.SysGroups.Count(e => e.GroupId == id) > 0;
        }
    }
}