using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewService;
namespace ReviewServiceTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestgetTaskAssignment()
        {
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            ReviewService.Models.TaskAssignment TA;

            int TaskId = 9;
            int GrpId = 3;

            TA = at.getTaskAssignment(GrpId, TaskId);

            Assert.AreNotEqual("Closed", TA.Task.Status);
            Assert.AreEqual( TaskId, TA.TaskId);
            Assert.AreEqual(GrpId, TA.GroupId);
            //What other asserts can be done?
        }
//check valid data
        [TestMethod]
        public void TestgetNodeId()
        {
            int testNodeId = 1;
            int nodeId = 0;
            int TaskId = 9;
            int GrpId = 3;
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            nodeId = at.getNodeId(TaskId, GrpId);

            Assert.AreEqual(nodeId, testNodeId);


        }
        //Check invalid data
        [TestMethod]
        public void TestgetNodeIdINvalid()
        {
            int testNodeId = 0;
            int nodeId = 0;
            int TaskId = 3;
            int GrpId = 9;
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            nodeId = at.getNodeId(TaskId, GrpId);

            Assert.AreEqual(nodeId, testNodeId);


        }

        [TestMethod]
        public void testgetOutput()
        {
            int testNodeId = 1;
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            var outputlist = at.getOutput(testNodeId);
            //Not sure what test to do yet
            Assert.AreEqual(2, outputlist.Count);

        }

    }
}
