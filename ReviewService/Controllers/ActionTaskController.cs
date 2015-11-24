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
        public TaskAssignment getTaskAssignment(int GroupId, int TaskId)
        {
            //"Select A.TaskId, A.AccessType, A.GroupId 
            //from TaskAssignment A 
            //Join Task T 
            //on T.TaskId = A.TaskId 
            //and Status <> 'Closed' 
            //where GroupId in (2) and TaskId = TaskId "
            //--1
            ApprovalProcessType TypeDefinition;
            BranchNode NodeDef;
            GroupRoleRelation GroupRels;
            TaskAssignment TA;
            TaskNode TN;
            ReviewTask reviewTask;

            TA = new TaskAssignment();
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            IEnumerable<TaskAssignment> TaskAssignments = db.TaskAssignments.Where(
                    r => 
                    r.Task.Status != "Closed" && r.GroupId == GroupId 
                    && r.TaskId == TaskId);

            System.Diagnostics.Debug.WriteLine(TaskAssignments.ToString());
            //QUick for demo - no iteration or collection used
            TA = TaskAssignments.First();

  
            return TA;

        }

        public int getNodeId(int TaskId, int GroupId)
        {
            //Select NodeId from TaskNode where TaskId = 1 and GroupId = 2   --1
            int nodeId;
            //Again just grabbing the first one and not worrying if we get multiple hits
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //Need to handle null reference exceptions
            try
            {
                nodeId =
                    (from a in db.TaskNodes
                     where (a.TaskId == TaskId && a.GroupId == GroupId)
                     select new { a.NodeId }).FirstOrDefault().NodeId;
            }
            catch
            {
                nodeId = 0;
                //Silent consume?
            }
            return nodeId;
        }

        public void ProcessAction()
        {
            int ApprovalProcessType = 0;
            int GroupId = 0;        //later make this a dictionary (or array or whatever)
            TaskAssignment TA;
            ApprovalProcessType = getApprovalProcessType(TaskId);
            //Change this later to handle multiple groups for the person
            GroupId = getUserGroup(ApproverUserId);  //Update later to be method chained
            TA = getTaskAssignment(GroupId, TaskId);
        }

        public class outputsettings
        {
           /* public outputsettings(int OutputNodeId, int ConditionTest, int ConditionDescription, int RelationTypeId, int Type, int AccessType)
            {
                OutputNodeId = OutputNodeId;
                ConditionTest, ConditionDescription, RelationTypeId, Type, AccessType, ConditionTest, ConditionDescription, RelationTypeId, Type, AccessType

            }*/
            //int? is nullable int- not sure if should block it or not really
           public int? OutputNodeId, RelationTypeId, AccessType;
            public string ConditionTest, ConditionDescription, Type;
        }

        public List<outputsettings> getOutput(int NodeId)
        {

            List<outputsettings> ret = new List<outputsettings>();

            List <int?> outputnodeid;

            outputnodeid = (from a in db.BranchNodes where (a.NodeId == NodeId) select a.OutputNodeId).ToList();

            //ret.Add(new outputsettings());
            try
            {
                ret =
                    (
                    from a in db.BranchNodes
                    where outputnodeid.Contains(a.NodeId)
                    select 
                    new outputsettings
                    {
                        OutputNodeId = (int)a.OutputNodeId,
                        ConditionTest = (string)a.BranchCondition.ConditionTest,
                        ConditionDescription = (string)a.BranchCondition.ConditionDescription,
                        RelationTypeId = a.RelationTypeId,
                        Type = (string)a.Type,
                        AccessType = (int)a.AccessType
                    }
                     ).ToList();
                     }
            catch { }


/*select OutputNodeId, ConditionTest, ConditionDescription, RelationTypeId, Type, AccessType
from BranchNode B
join
BranchCondition C on B.ConditionId = C.ConditionId
Where NodeId IN
(
select OutputNodeId from BranchNode where NodeId = 1
)*/

            return ret;
        }

private List<int> getRelativeGroupId(int ApproverUserId, int iTaskId, int ApprovalProcessType)
        {

            List<int> origGroupIds;
            int raiserUserId;
            List<int> UserGroupIds;
            List<int> relativeGroupIds;

            //Abusing FirstOrDefault
            //Call to value avoids the nulling of the integer
            //Implict fail if null though!
            raiserUserId = 
                (from a 
                in db.ReviewTasks
                where a.TaskId == iTaskId
                select a.RaiserUserId).FirstOrDefault().Value;

            UserGroupIds = (from a
               in db.UserGroups
                            where a.PersonId == ApproverUserId
                            select a.GroupId).ToList();

            origGroupIds = (from a
                    in db.UserGroups
                            where a.PersonId == raiserUserId
                            select a.GroupId).ToList();
            int relTypeApprove = 1;
            //Have to cast away the nullable ints
                    relativeGroupIds =
             (
             from a in db.GroupRoleRelations
             where a.RelationType.RelationTypeId == relTypeApprove 
             && UserGroupIds.Contains(a.RelativeGroupId.Value)
             && origGroupIds.Contains(a.MasterGroupId.Value)
             && a.ApprovalProcessId.Value == ApprovalProcessType
             select a.RelativeGroupId
              ).Cast<int>().ToList();
            return relativeGroupIds;
        }
                        
            
            /*
            --Work backwards to original user to determine what relation we have to them
            --From the above output nodes we have RelationTypeId = 1 (Approve).  We are approver.  
            --So identify which of the groups to which we belong is the one 
            --that gives us our approver role

        select relativeGroupId from GroupRoleRelation where RelationTypeId = 1 and 
            RelativeGroupId In 
            (select GroupId from UserGroup where personId = 13)
            and 
            MasterGroupId In 
            (select GroupId from UserGroup where personId = (select raiserUserId from Task where taskId = 1))
            and ApprovalProcessId = 3

            --2  (i.e. we, the appover, are in Group 2 and that is the group that was assigned the task)
            */

         

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