using System.Collections.Generic;
using System.Linq;
using Dapper;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Service;

namespace LabRequest.DomainModel.Repository
{
    public class TestRequestRepository : ITestRequest
    {
        private readonly OracleContext _context = new OracleContext();
        private readonly DateTimeRepository _datetimerepo = new DateTimeRepository();


        public List<Corporation> GetAllCorporation()
        {
            var query = @"SELECT COM_ID,
                        COM_TITLE,
                        SUMMERY_COM
                        FROM CENTRAL_LAB.TBLCOMPANY ORDER BY COM_ID";
            return _context.OracleContextConnectionString
                  .Query<Corporation>(query).ToList();
        }

        public List<TestRequest> GetAllNonRequest()
        {
            return _context
                .OracleContextConnectionString
                .Query<TestRequest>(@"SELECT * FROM TBLTESTREQUEST")
                .OrderBy(x => x.TestRequestId)
                .Take(10)
                .ToList();
        }

        public List<Company> GetAllCompanies(int userid)
        {
            return _context.OracleContextConnectionString
                  .Query<Company>(@"SELECT DISTINCT(t3.IDREC),t3.TYPEREC,t3.DES,t.USERID 
                    FROM CENTRAL_LAB.TBLUSERUNIT t,
                    CENTRAL_LAB.TBLPERSON t1,
                    CENTRAL_LAB.TBLAPPLICANT t2,
                    CENTRAL_LAB.TBLBASEINFO t3 
                    WHERE t.USERID = t1.PERSONID 
                    AND t.APPLICANTID = t2.APPLICANTID 
                    AND t2.COMPANYID = t3.IDREC")
                  .Where(x => x.UserId == userid && x.TypeRec == 5)
                  .OrderBy(x => x.Des)
                  .ToList();
        }



        public List<Unit> GetAllUnits()
        {
            return _context.OracleContextConnectionString
                .Query<Unit>(@"SELECT * FROM CENTRAL_LAB.TBLAPPLICANT")
                .OrderBy(x => x.ApplicantName)
                .ToList();
        }

        public List<RequestTestDetails> GetAllTestRequestDetails(int testid, int reqgenid)
        {
            var sqlquery = @"SELECT t.TESTREQUESTID,t.ReqTestsID,
                           t2.TESTNAME,t.CONFIRM,t.EXEDATE,t.EXETIME 
                           FROM TBLREQTESTS t,TBLTEST t2 
                           WHERE t.TESTID = t2.TESTID 
                           AND t.TESTREQUESTID = :TESTREQUESTID 
                           AND t.REQUESTGENID = :REQUESTGENID";
            ;
            return _context.OracleContextConnectionString
                .Query<RequestTestDetails>(sqlquery, new
                {
                    TESTREQUESTID = testid,
                    REQUESTGENID = reqgenid
                })
                .ToList();
        }

        public TestRequest GetOne(string name)
        {
            return _context
                .OracleContextConnectionString
                .Query<TestRequest>(@"SELECT * FROM TBLTESTREQUEST")
                .Single(x => x.RequestNo.Equals(name));
        }

        public TestRequest GetOne(int id)
        {
            return _context
                .OracleContextConnectionString
                .Query<TestRequest>(@"SELECT * FROM TBLTESTREQUEST")
                .Single(x => x.RequestNo.Equals(id));
        }

        public void Add(TestRequest model)
        {
            var sqlquery = @"INSERT INTO TBLTESTREQUEST
                (TESTREQUESTID,REQUESTNO,REQDATE,STARTDATE,
                ENDDATE,APPLICANTID,REQTYPE,CREATEID,CONFIRMID,
                CONFIRM,ENABLE,SAMPLENAME,CONTRACT,COM_ID)
                 VALUES(:TESTREQUESTID,:REQUESTNO,:REQDATE,
                :STARTDATE,:ENDDATE,:APPLICANTID,:REQTYPE,
                :CREATEID,:CONFIRMID,:CONFIRM,:ENABLE,
                :SAMPLENAME,:CONTRACT,:COM_ID)";
            _context.OracleContextConnectionString
                .Execute(sqlquery,
                new
                {
                    model.TestRequestId,
                    model.RequestNo,
                    model.ReqDate,
                    model.StartDate,
                    model.EndDate,
                    model.ApplicantId,
                    model.ReqType,
                    model.CreateId,
                    model.ConfirmId,
                    model.Confirm,
                    model.Enable,
                    model.SampleName,
                    model.Contract,
                    model.Com_Id
                });
        }

        public void Delete(int id)
        {
            _context.OracleContextConnectionString
                .Execute(@"DELETE FROM TBLTESTREQUEST WHERE
                 TESTREQUESTID=:TESTREQUESTID",
                new
                {
                    TESTREQUESTID = id
                });
        }

        public void Delete(string name)
        {
            _context.OracleContextConnectionString
                .Execute(@"DELETE FROM TBLTESTREQUEST WHERE
                  REQUESTNO=:REQUESTNO",
                new
                {
                    REQUESTNO = name
                });
        }


        public void Edit(int id, TestRequest model)
        {
            var sqlquery = @"UPDATE TBLTESTREQUEST SET 
                            TESTREQUESTID= :TESTREQUESTID ,REQUESTNO = :REQUESTNO,
                            REQDATE =: REQDATE,STARTDATE = :STARTDATE,
                            ENDDATE =:ENDDATE,APPLICANTID =:APPLICANTID,
                            REQTYPE =:REQTYPE,CREATEID =:CREATEID,CONFIRMID = :CONFIRMID,
                            CONFIRM =:CONFIRM,ENABLE = :ENABLE,
                            SAMPLENAME = :SAMPLENAME,CONTRACT= :CONTRACT 
                            WHERE TESTREQUESTID=:TESTREQUESTID";
            _context.OracleContextConnectionString
                .Execute(sqlquery,
                new
                {
                    model.TestRequestId,
                    model.RequestNo,
                    model.ReqDate,
                    model.StartDate,
                    model.EndDate,
                    model.ApplicantId,
                    model.ReqType,
                    model.CreateId,
                    model.ConfirmId,
                    model.Confirm,
                    model.Enable,
                    model.SampleName,
                    model.Contract,
                    TESTREQUESTID = id
                });
        }

        public void Edit(TestRequest model)
        {
            var sqlquery = @"UPDATE TBLTESTREQUEST SET 
                TESTREQUESTID= :TESTREQUESTID ,REQUESTNO = :REQUESTNO,
                REQDATE =: REQDATE,STARTDATE = :STARTDATE,
                ENDDATE =:ENDDATE,APPLICANTID =:APPLICANTID,
                REQTYPE =:REQTYPE,CREATEID =:CREATEID,CONFIRMID = :CONFIRMID,
                CONFIRM =:CONFIRM,ENABLE = :ENABLE,
                SAMPLENAME = :SAMPLENAME,CONTRACT= :CONTRACT 
                WHERE TESTREQUESTID=:TESTREQUESTID";
            _context.OracleContextConnectionString
                .Execute(sqlquery,
                new
                {
                    model.TestRequestId,
                    model.RequestNo,
                    model.ReqDate,
                    model.StartDate,
                    model.EndDate,
                    model.ApplicantId,
                    model.ReqType,
                    model.CreateId,
                    model.ConfirmId,
                    model.Confirm,
                    model.Enable,
                    model.SampleName,
                    model.Contract,
                    TESTREQUESTID = model.TestRequestId
                });
        }


        public List<TestRequestTitle> GetAllRequestTitles()
        {
            var sqlquery = @"SELECT APPLICANTID,IDREC,TYPEREC,DES,REQUESTNAMEID 
                FROM TBLBASEINFO, TBLREQUESTUNIT 
                WHERE  TBLBASEINFO.IDREC = TBLREQUESTUNIT.REQUESTNAMEID 
                AND TBLBASEINFO.TYPEREC = 20";
            return _context.OracleContextConnectionString
                .Query<TestRequestTitle>(sqlquery)
                .OrderBy(x => x.Des).ToList();
        }


        public List<TestRequestTitle> GetAllRequestTitles(int userid)
        {
            var sqlquery = @"SELECT DISTINCT (IDREC),
                TBLREQUESTUNIT.APPLICANTID,
                TYPEREC,DES,REQUESTNAMEID
                FROM TBLBASEINFO, TBLREQUESTUNIT, tbluserunit
                WHERE TBLBASEINFO.IDREC = TBLREQUESTUNIT.REQUESTNAMEID
                AND TBLUSERUNIT.APPLICANTID = TBLREQUESTUNIT.APPLICANTID
                AND TBLBASEINFO.TYPEREC = 20
                AND TBLUSERUNIT.USERID = :USERID";
            return _context.OracleContextConnectionString
                .Query<TestRequestTitle>(sqlquery, new { userid })
                .ToList();
        }


        public List<TestRequestTitleDetails> GetAllTestRequestTitleDetails()
        {
            var sqlquery = @"SELECT * FROM TBLTEST, TBLREQUESTTEST 
                 WHERE TBLTEST.TESTID = TBLREQUESTTEST.TESTID
                  AND TBLTEST.ACTIVE=0";
            return _context.OracleContextConnectionString
                .Query<TestRequestTitleDetails>(sqlquery)
                .OrderBy(x => x.TestId).ToList();
        }



        public string RequestNewCode(EnumCollection.RequestGenRequestType recType)
        {
            var calcTime = _datetimerepo.GetLongLocalTime.Substring(6, 2);
            calcTime += _datetimerepo.GetLongLocalTime.Substring(3, 2);
            calcTime += _datetimerepo.GetLongLocalTime.Substring(0, 2);
            var calcDate = _datetimerepo.GetLocalDate.Substring(2, 2);
            calcDate += _datetimerepo.GetLocalDate.Substring(5, 2);
            calcDate += _datetimerepo.GetLocalDate.Substring(8, 2);
            return ((int)recType).ToString() + calcDate + calcTime;
        }


        public int MaxTextRequestId
        {
            get
            {
                return _context.OracleContextConnectionString
                    .Query<int>(@"SELECT MAX(TestRequestID) FROM tblTestRequest")
                    .Single() + 1;
                /*_context.OracleContextConnectionString
                   .Query<bool>("SELECT TestRequestID FROM tblTestRequest").Any() ?
                _context.OracleContextConnectionString
                .Query<int>("SELECT MAX(TestRequestID) FROM tblTestRequest").Single() + 1 : 1;*/
            }
        }


        public List<TestRequestGenerateUsers> GetAllTestRequestGenerateUsers(int userid, string year, string reqno)
        {
            var sqlquery = @"SELECT A.REQUESTNO,
           A.TESTREQUESTID,A.REQDATE,A.SAMPLENAME,
           B.REQUESTTIME,B.LATNO,C.PERSONFAMILY,B.REQUESTGENID,A.CONFIRM 
           FROM tbltestrequest a, tblrequestgen b, tblperson c 
           WHERE A.TESTREQUESTID = B.TESTREQUESTID 
               AND A.CREATEID = C.PERSONID 
               AND SUBSTR(A.REQDATE,1,4) = :YEAR 
               AND a.CREATEID = :CREATEID";
            sqlquery += !string.IsNullOrEmpty(reqno) ? @" AND A.REQUESTNO = :REQUESTNO " : string.Empty;
            return _context.OracleContextConnectionString
                .Query<TestRequestGenerateUsers>(sqlquery,
                new
                {
                    CREATEID = userid,
                    //update code
                    Year = year,
                    //YEAR = "1393",
                    REQUESTNO = reqno
                }).ToList();
        }


        public void Edit(int id)
        {
            var sqlquery = @"UPDATE tblReqTests SET  Confirm=1,ConfirmID=1 WHERE ReqTestsID=:ReqTestsID";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new
                {
                    ReqTestsID = id
                });
        }


        public List<Sample> GetAllSampleType(int id)
        {
            return _context.OracleContextConnectionString
                .Query<Sample>(@"select * from tblBaseInfo where typerec =:typerec",
                new { typerec = id })
                .OrderBy(x => x.Des)
                .ToList();
        }

        public List<TestRequest> GetAllSampleName(string date)
        {
            return _context.OracleContextConnectionString
                .Query<TestRequest>(@"SELECT TESTREQUESTID,SAMPLENAME FROM TBLTESTREQUEST WHERE REQDATE >= :REQDATE",
                new { REQDATE = date })
                .OrderBy(x => x.SampleName)
                .ToList();
        }



        public List<Unit> GetAllUnits(int userid)
        {
            return _context.OracleContextConnectionString
               .Query<Unit>(@"SELECT DISTINCT TBLAPPLICANT.*
                              FROM TBLAPPLICANT,TBLUSERUNIT
                              WHERE TBLAPPLICANT.APPLICANTID = TBLUSERUNIT.APPLICANTID
                              AND USERID =:USERID", new { userid })
               .OrderBy(x => x.ApplicantName)
               .ToList();

        }

    }
}