using System.Collections.Generic;
using LabRequest.DomainModel.Entities;
using System.Linq;

namespace LabRequest.DomainModel.Service
{
    public interface ITestRequest
    {
        List<Corporation> GetAllCorporation();
        List<TestRequest> GetAllNonRequest();
        List<Company> GetAllCompanies(int userid);
        List<Unit> GetAllUnits();
        List<Unit> GetAllUnits(int userid);
        List<Sample> GetAllSampleType(int id);
        List<TestRequest> GetAllSampleName(string date);
        List<TestRequestTitle> GetAllRequestTitles();
        List<TestRequestTitle> GetAllRequestTitles(int userid);
        List<TestRequestTitleDetails> GetAllTestRequestTitleDetails();
        List<TestRequestGenerateUsers> GetAllTestRequestGenerateUsers(int userid, string year, string reqno);
        List<RequestTestDetails> GetAllTestRequestDetails(int testid, int reqgenid);
        TestRequest GetOne(string name);
        TestRequest GetOne(int id);
        int MaxTextRequestId { get; }
        string RequestNewCode(EnumCollection.RequestGenRequestType recType);
        void Add(TestRequest model);
        void Delete(int id);
        void Delete(string name);
        void Edit(int id, TestRequest model);
        void Edit(TestRequest model);
        void Edit(int id);

    }
}
