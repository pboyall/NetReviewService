using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewService;
namespace TestReviewService
{
    [TestClass]
    public class TestActionTaskController
    {
        [TestMethod]
        public void TestMethod1()
        {
            ReviewService.Controllers.actiontask at = new ReviewService.Controllers.actiontask();
            at.getTaskAssignment(2);


        }
    }
}
