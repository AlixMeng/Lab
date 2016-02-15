using System.Collections.Generic;
using System.Linq;
using Dapper;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Service;

namespace LabRequest.DomainModel.Repository
{
    public class RequestTestRepository : IRequestTest
    {
        private readonly OracleContext _context = new OracleContext();

        public List<RequestTest> GetAllRequestTest()
        {
            return _context.OracleContextConnectionString
                .Query<RequestTest>(@"SELECT * FROM CENTRAL_LAB.TBLREQTESTS")
                .OrderBy(x => x.ReqTestsId).Take(100).ToList();
        }

        public RequestTest GetRequestTest(int id)
        {
            return _context.OracleContextConnectionString
                .Query<RequestTest>(@"SELECT * FROM CENTRAL_LAB.TBLREQTESTS")
                .Single(x => x.ReqTestsId == id);

        }

        public int Max
        {
            get
            {
                return _context.OracleContextConnectionString.
                    Query<int>(@"SELECT MAX(REQTESTSID) FROM CENTRAL_LAB.TBLREQTESTS")
                    .Single() + 1;                    
                    /*_context.OracleContextConnectionString
                    .Query<bool>("SELECT REQTESTSID FROM CENTRAL_LAB.TBLREQTESTS").Any() ?
                     _context.OracleContextConnectionString
                    .Query<int>("SELECT MAX(REQTESTSID) FROM CENTRAL_LAB.TBLREQTESTS")
                    .Single() + 1 : 1;?*/
                        
            }
        }



        public void Add(RequestTest entity)
        {
            var sqlquery = @"INSERT INTO CENTRAL_LAB.TBLREQTESTS 
            (REQTESTSID,REQUESTGENID,TESTREQUESTID,REQUESTNAMEID 
            ,TESTID,CONFIRMID,CONFIRM,EXEDATE,EXETIME,EXEMANID 
            ,TESTSTATE,TESTDES,RETESTCOUNT,CONFIRMDATE,CONFIRMTIME 
            ,MACHINID,PRICE,PERSONSHIFT,EXESHIFT)  
            VALUES(:REQTESTSID,:REQUESTGENID,:TESTREQUESTID,:REQUESTNAMEID 
            ,:TESTID,:CONFIRMID,:CONFIRM,:EXEDATE,:EXETIME,:EXEMANID 
            ,:TESTSTATE,:TESTDES,:RETESTCOUNT,:CONFIRMDATE,:CONFIRMTIME 
            ,:MACHINID,:PRICE,:PERSONSHIFT,:EXESHIFT)";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new
                {
                    entity.ReqTestsId,
                    entity.RequestGenId,
                    entity.TestRequestId,
                    entity.RequestNameId,
                    entity.TestId,
                    entity.ConfirmId,
                    entity.Confirm,
                    entity.ExeDate,
                    entity.ExeTime,
                    entity.ExeManId,
                    entity.TestState,
                    entity.TestDes,
                    entity.ReTestCount,
                    entity.ConfirmDate,
                    entity.ConfirmTime,
                    entity.MachinId,
                    entity.Price,
                    entity.PersonShift,
                    entity.ExeShift,
                });
        }

        public void Delete(int id)
        {
            var sqlquery = @"DELETE FROM CENTRAL_LAB.TBLREQTESTS WHERE  REQTESTSID = :REQTESTSID";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new { REQTESTSID = id });
        }


        public void Edit(RequestTest entity, int id)
        {
            var sqlquery = @"UPDATE CENTRAL_LAB.TBLREQTESTS 
            SET REQUESTGENID =:REQUESTGENID,TESTREQUESTID = :TESTREQUESTID,REQUESTNAMEID = :REQUESTNAMEID 
            ,TESTID = :TESTID,CONFIRMID = :CONFIRMID,CONFIRM = :CONFIRM,EXEDATE = :EXEDATE,EXETIME = :EXETIME 
            ,EXEMANID = :EXEMANID,TESTSTATE = :TESTSTATE,TESTDES = :TESTDES,RETESTCOUNT = :RETESTCOUNT
            ,CONFIRMDATE = :CONFIRMDATE,CONFIRMTIME = :CONFIRMTIME,MACHINID = :MACHINID,PRICE = :PRICE
            ,PERSONSHIFT = :PERSONSHIFT,EXESHIFT = :EXESHIFT WHERE REQTESTSID = :REQTESTSID";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new
                {
                    ReqTestId = entity.ReqTestsId,
                    entity.RequestGenId,
                    entity.TestRequestId,
                    entity.RequestNameId,
                    entity.TestId,
                    entity.ConfirmId,
                    entity.Confirm,
                    entity.ExeDate,
                    entity.ExeTime,
                    entity.ExeManId,
                    entity.TestState,
                    entity.TestDes,
                    entity.ReTestCount,
                    entity.ConfirmDate,
                    entity.ConfirmTime,
                    entity.MachinId,
                    entity.Price,
                    entity.PersonShift,
                    entity.ExeShift,
                    REQTESTSID = id
                });
        }
    }
}