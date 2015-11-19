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
            at.getTaskAssignment(2);


        }
    }
}
