using LabRequest.DomainModel.Repository;
using LabRequest.DomainModel.Entities;

namespace LabRequest.Web.ViewModel
{
    public class TestRequestGenerateUsersViewModel
    {
        public PagedData<TestRequestGenerateUsers> Tests { get; set; }
    }
}