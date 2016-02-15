using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LabRequest.Web.Controllers;
using LabRequest.Web.ViewModel;
using NUnit.Framework;

namespace LabRequest.Test
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void TestLogin()
        {
            var user = new UserLoginViewModel()
            {
                UserName = "ITI",
                Password = "+-*333",
                Corporation = "1"
            };

            var viewModel = (RedirectToRouteResult)new AccountController()
                .Login(user, "TestRequestList");
            Assert.AreEqual("TestRequestList",
                viewModel.RouteValues["TestRequestList"]);
        }
    }
}
