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

    public class actiontask
    {

        //Flip this around to be dependency injection later
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();
        public int ApproverUserId { get; set; }
        public int TaskId { get; set; }
        public string Action { get; set; }

        //TODO - error checks on everything
        //Sort out the boxing

        private int getApprovalProcessType(int TaskId)
        {
            ReviewTask reviewTask;
            //            select ApprovalProcessType from Task where TaskId = 1
            //--3
            reviewTask = db.ReviewTasks.FirstOrDefault(a => a.TaskId.Equals(TaskId));
            return (int)reviewTask.ApprovalProcessType;
        }

        private int getUserGroup(int ApproverUserId)
        {
            //select GroupId from UserGroup where personId = 13
            //--2
            UserGroup userGroup;
            //May need to return all groups to which this person belongs?
            userGroup = db.UserGroups.FirstOrDefault(a => a.PersonId.Equals(ApproverUserId));
            //Yes, but this is a Proof of Concept so assume one group per person for now
            return (int)userGroup.GroupId;
        }
        //Made public while working on it
        public TaskAssignment getTaskAssignment(int GroupId)
        {
            ApprovalProcessType TypeDefinition;
            BranchNode NodeDef;
            GroupRoleRelation GroupRels;
            TaskAssignment TA;
            TaskNode TN;
            ReviewTask reviewTask;

            TA = new TaskAssignment();
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            //reviewTask = db.ReviewTasks.Where(r => r.TaskAssignments.Any(ta => ta.TaskId == TaskId && ta.GroupId == GroupId && r.Status != "Closed")).FirstOrDefault();
            /*
                        var TaskAssignments1 = from tas in db.TaskAssignments
                                              where tas.TaskId == TaskId
                                              select new
                                              {
                                                  TId = tas.TaskId,
                                                  AT = tas.AccessType,
                                                  Grp = tas.GroupId,
                                                  Stat = tas.Task.Status
                                              };


                        System.Diagnostics.Debug.WriteLine(TaskAssignments1.ToString());
            */
            IEnumerable<TaskAssignment> TaskAssignments = db.TaskAssignments.Where(
                    r => 
                    r.Task.Status != "Closed" && r.GroupId == GroupId 
                    && r.TaskId == TaskId);

            System.Diagnostics.Debug.WriteLine(TaskAssignments.ToString());
            //QUick for demo - no iteration or collection used
            TA = TaskAssignments.First();

            //"Select A.TaskId, A.AccessType, A.GroupId 
            //from TaskAssignment A 
            //Join Task T 
            //on T.TaskId = A.TaskId 
            //and Status <> 'Closed' 
            //where GroupId in (2) and TaskId = TaskId "
            //--1
            return TA;

        }
        public void ProcessAction()
        {
            int ApprovalProcessType = 0;
            int GroupId = 0;        //later make this a dictionary (or array or whatever)
            ApprovalProcessType = getApprovalProcessType(TaskId);
            //Change this later to handle multiple groups for the person
            GroupId = getUserGroup(ApproverUserId);



        }
    }
/*
Select NodeId from TaskNode where TaskId = 1 and GroupId = 2
--1

select OutputNodeId, ConditionTest, ConditionDescription, RelationTypeId, Type, AccessType
from BranchNode B
join
BranchCondition C on B.ConditionId = C.ConditionId
Where NodeId IN
(
select OutputNodeId from BranchNode where NodeId = 1
)

--9 Success Submission Success  1   FirstApprovalNode   1
--7 Reject Rejection   0   End - Rejected    1
        }

    }
*/

        public class ActionTaskController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();

        // GET: api/ActionTask
        public IEnumerable<string> Get()
        {
            return new string[] { "1" };
        }

        // GET: api/ActionTask/5
        
        public string Get(int id)
        {
            return "0";
        }

        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id.Equals(null))
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ActionTask
        [ResponseType(typeof(actiontask))]
        public IHttpActionResult Post([FromBody]actiontask at)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            at.ProcessAction();



            return CreatedAtRoute("DefaultApi", new { id = at.TaskId }, at);
        }

        // DELETE: api/ActionTask/5
    
        public void Delete(int id)
        {
    
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