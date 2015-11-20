using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewService;
namespace ReviewServiceTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            ReviewService.Models.TaskAssignment TA;

            int TaskId = 9;
            int GrpId = 3;

            at.TaskId = TaskId;
            
            TA = at.getTaskAssignment(GrpId);

            Assert.AreNotEqual("Closed", TA.Task.Status);
            Assert.AreEqual( TaskId, TA.TaskId);
            Assert.AreEqual(GrpId, TA.GroupId);
            //What other tests can be done?


        }
    }
}
