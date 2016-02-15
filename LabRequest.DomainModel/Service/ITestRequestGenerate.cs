using System.Collections.Generic;
using LabRequest.DomainModel.Entities;

namespace LabRequest.DomainModel.Service
{
    public interface ITestRequestGenerate
    {
        List<TestRequestGenerate> GetAllRequestGenerates();
        TestRequestGenerate GetRequestGenerate(int id);
        TestRequestGenerate GetRequestGenerate(string name);
        int MaxRequestGenId { get; }
        int GetCurrentShift(string currenttime, string currentdate);
        void Add(TestRequestGenerate entity);
        void Edit(int id, TestRequestGenerate entity);
        void Delete(int id);
        void Delete(string name);
    }
}
