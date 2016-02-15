using System.Linq;
using Dapper;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Service;

namespace LabRequest.DomainModel.Repository
{
    public class TestRepository : ITest
    {
        private readonly OracleContext _context = new OracleContext();

        public Test GetPrice(int id)
        {
            return _context.OracleContextConnectionString
                .Query<Test>(@"SELECT TESTID,PRICE,Active FROM tblTest")
                .Single(x => x.TestId == id);
        }




        public System.Collections.Generic.List<Test> GetAllTests()
        {
            return _context.OracleContextConnectionString
                .Query<Test>(@"SELECT TestId, TestName,Price,Active FROM tbltest")
                //.OrderBy(x=>x.TestName)
                .ToList();
        }
    }
}