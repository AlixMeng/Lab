using System.Collections.Generic;
using LabRequest.DomainModel.Entities;

namespace LabRequest.DomainModel.Service
{
    public interface IRequestTest
    {
        List<RequestTest> GetAllRequestTest();
        RequestTest GetRequestTest(int id);
        int Max { get; }
        void Add(RequestTest entity);
        void Delete(int id);
        void Edit(RequestTest entity, int id);
    }
}