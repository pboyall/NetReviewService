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
    public class ApprovalProcessTypesController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/ApprovalProcessTypes
        public IQueryable<ApprovalProcessType> GetApprovalProcessTypes()
        {
            return db.ApprovalProcessTypes;
        }

        // GET: api/ApprovalProcessTypes/5
        [ResponseType(typeof(ApprovalProcessType))]
        public IHttpActionResult GetApprovalProcessType(int id)
        {
            ApprovalProcessType approvalProcessType = db.ApprovalProcessTypes.Find(id);
            if (approvalProcessType == null)
            {
                return NotFound();
            }

            return Ok(approvalProcessType);
        }

        // PUT: api/ApprovalProcessTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutApprovalProcessType(int id, ApprovalProcessType approvalProcessType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != approvalProcessType.ApprovalProcessId)
            {
                return BadRequest();
            }

            db.Entry(approvalProcessType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalProcessTypeExists(id))
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

        // POST: api/ApprovalProcessTypes
        [ResponseType(typeof(ApprovalProcessType))]
        public IHttpActionResult PostApprovalProcessType(ApprovalProcessType approvalProcessType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ApprovalProcessTypes.Add(approvalProcessType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = approvalProcessType.ApprovalProcessId }, approvalProcessType);
        }

        // DELETE: api/ApprovalProcessTypes/5
        [ResponseType(typeof(ApprovalProcessType))]
        public IHttpActionResult DeleteApprovalProcessType(int id)
        {
            ApprovalProcessType approvalProcessType = db.ApprovalProcessTypes.Find(id);
            if (approvalProcessType == null)
            {
                return NotFound();
            }

            db.ApprovalProcessTypes.Remove(approvalProcessType);
            db.SaveChanges();

            return Ok(approvalProcessType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApprovalProcessTypeExists(int id)
        {
            return db.ApprovalProcessTypes.Count(e => e.ApprovalProcessId == id) > 0;
        }
    }
}