using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Repository;
using LabRequest.Web.Controllers;
using LabRequest.Web.ViewModel;
using Moq;
using NUnit.Framework;

namespace LabRequest.Test
{

    [TestFixture]
    public class LabRequestTest
    {
        private readonly TestRequestRepository _requestRepository =
            new TestRequestRepository();

        [Test]
        public void LabRequestList()
        {
            var viewResult = (ViewResult)new TestRequestController()
                .TestRequestList(string.Empty, string.Empty, string.Empty, 1);
            Assert.AreEqual(_requestRepository.
                GetAllTestRequestGenerateUsers(156, "1394", string.Empty)
                .Count,
                viewResult.ViewData.Count);
        }

        [Test]
        public void LabRequestAdd()
        {
            var request = new TestRequestViewModel()
            {
                Company = "1",
                LotNumber = "test",
                RequestDate = "1394/09/01",
                Unit = "1",
                SampleName = "test",
                RequestType = EnumCollection.RequestGenRequestType.Resample,
                RequestDetail = new[] { "1", "2", "3" },
                RequestPersonName = "Admin",
                RequestPriority = EnumCollection.RequestGenStatus.فوری,
                RequestTitle = "test"
            };
            var add = (RedirectToRouteResult)new TestRequestController()
                .TestRequestAdd(request);
            Assert.AreEqual("TestRequestResult",
                add.RouteValues["action"]);
        }

        [Test]
        public void LabRequestJsonCheck()
        {
            var viewModel = new TestRequestController()
                .GetRequestTitles(10);
            Assert.AreEqual(viewModel.Data,
                new SelectList(_requestRepository
                    .GetAllRequestTitles(10)));
        }
    }
}