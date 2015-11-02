using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReviewService.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.Description;
namespace ReviewService.Controllers
{

    public class workflow {

        //Flip this around to be dependency injection later
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();
        public int ApprovalProcessId { get; set; }
        public int RaiserUserId { get; set; }

        public void InitialiseWorkflow() {

            ApprovalProcessType TypeDefinition = db.ApprovalProcessTypes.FirstOrDefault(a => a.ApprovalProcessId.Equals(ApprovalProcessId));

            var StartNodeId = TypeDefinition.StartNodeId;

            ReviewTask reviewTask = new ReviewTask();

            reviewTask.ApprovalProcessType = ApprovalProcessId;
            reviewTask.RaiserUserId = RaiserUserId;
            reviewTask.Status = "I";
            reviewTask.DateUpdated = System.DateTime.Today;

            db.ReviewTasks.Add(reviewTask);

            /*

            Select StartNodeId from ApprovalProcessTypes where ApprovalProcessId = '3'   -- 3,4,5,6  Sequential, Hierarchical, Voting, Unanimous
--1
insert into Task(DateUpdated, Status, RaiserUserId, ApprovalProcessType) values(getdate(), 'I', 1, 3)
--Check code
select * from TASK where DateUpdated > dateadd(d, -1, getdate())
--Task Id = 1 (not got a group yet)
select BranchNode.RelationTypeId, BranchNode.AccessType from BranchNode where 
NodeId = 1
and Type='Start'

--1, 1
--For each node returned get relation
select RelativeGroupId from GroupRoleRelation where 
	ApprovalProcessId = 3 AND
	MasterGroupId IN (1) AND -- My group
	RelationTypeId  = 1   --From BrachNode

--2
--Access Type still 1, NodeId still 1
--Iterative in code for each group
	insert into TaskAssignment (TaskId, DateAssigned, GroupId, AccessType, NodeId) 
								VALUES(1, getdate(), 2, 1, 1)
	insert into TaskNode(TaskId, NodeId, DateUpdated, GroupId) values(1, 1, GETDATE(), 2)



    */


            db.SaveChanges();
        }

    }


    public class WorkflowController : ApiController
    {
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        // Create a new workflow for the given Approval Process and User ID
        //No response type yet
        [ResponseType(typeof(ReviewTask))]
        public IHttpActionResult Post([FromBody]workflow wf)
        {
            //Initialise Workflow
            //RaiserUserId, ApprovalProcessType
            //Call task 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            wf.InitialiseWorkflow();

            return CreatedAtRoute("DefaultApi", new { id = wf.ApprovalProcessId }, wf);

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}