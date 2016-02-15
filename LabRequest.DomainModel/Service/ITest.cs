using LabRequest.DomainModel.Entities;
using System.Collections.Generic;
namespace LabRequest.DomainModel.Service
{
    public interface ITest
    {
        Test GetPrice(int id);
        List<Test> GetAllTests();
    }
}