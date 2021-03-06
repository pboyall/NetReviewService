﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReviewService.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
namespace ReviewService.Controllers
{

    public class workflow {

        //Flip this around to be dependency injection later
        private ReviewProjectEntities2 db = new ReviewProjectEntities2();
        public int ApprovalProcessId { get; set; }
        public int RaiserUserId { get; set; }
        public int MasterGroupId { get; set; }

        public void InitialiseWorkflow() {

            //TODO: Error Checking

            int StartNodeId;
            int NodeId;
            int AccessType;
            int RelationType;
            int RelativeGroupId;                //This will need to be an array in later iterations, to allow for multiple branches
            String StartNodeText = "Start";

            ApprovalProcessType TypeDefinition;
            BranchNode NodeDef;
            GroupRoleRelation GroupRels;
            TaskAssignment TA;
            TaskNode TN;
            ReviewTask reviewTask;          //Can't be called Task as that causes interesting things to happen since .net defines a Task type already

            //Get Start Node for this Workflow Process Type
            TypeDefinition = db.ApprovalProcessTypes.FirstOrDefault(a => a.ApprovalProcessId.Equals(ApprovalProcessId));
            StartNodeId = (int)TypeDefinition.StartNodeId;
            //Get Relationship to use to send this Node off for approval 
            NodeDef = db.BranchNodes.FirstOrDefault(a => a.NodeId.Equals(StartNodeId)  && a.Type.Equals(StartNodeText));
            //Note forcing one row returned from NodeDef 
            NodeId = NodeDef.NodeId;
            AccessType = (int)NodeDef.AccessType;
            RelationType = NodeDef.RelationTypeId;
            //Just done one group for demonstration purposes
            //&& a.RelationTypeId.Equals(RelationType)
            //Has value
            GroupRels = db.GroupRoleRelations.FirstOrDefault(a => a.ApprovalProcessId ==ApprovalProcessId && a.MasterGroupId == MasterGroupId && a.RelationTypeId.Equals(RelationType));
            RelativeGroupId = (int)GroupRels.RelativeGroupId;


            reviewTask = new ReviewTask();
            TA = new TaskAssignment();
            TN = new TaskNode();


            reviewTask.ApprovalProcessType = ApprovalProcessId;
            reviewTask.RaiserUserId = RaiserUserId;
            reviewTask.Status = "I";
            reviewTask.DateUpdated = System.DateTime.Today;

            db.ReviewTasks.Add(reviewTask);
            db.SaveChanges();

            TA.AccessType = AccessType.ToString();
            TA.DateAssigned = System.DateTime.Today;
            TA.GroupId = RelativeGroupId;
            TA.TaskId = reviewTask.TaskId;
            TA.NodeId = NodeId;
            db.TaskAssignments.Add(TA);

            TN.NodeId = NodeId;
            TN.GroupId = RelativeGroupId;
            TN.TaskId = reviewTask.TaskId;
            TN.DateUpdated = System.DateTime.Today;
            db.TaskNodes.Add(TN);

            db.SaveChanges();

            //Should really return a success/fail indicator here

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
        [ResponseType(typeof(workflow))]
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
            //Am returning the wrong type!
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