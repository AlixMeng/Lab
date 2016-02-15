using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Service;

namespace LabRequest.DomainModel.Repository
{
    public class TestRequestGenerateRepository : ITestRequestGenerate
    {
        private readonly OracleContext _context = new OracleContext();
        private readonly DateTimeRepository _datetimerepository = new DateTimeRepository();

        public List<TestRequestGenerate> GetAllRequestGenerates()
        {
            return _context.OracleContextConnectionString
                .Query<TestRequestGenerate>(@"SELECT * FROM TBLREQUESTGEN")
                .OrderBy(x => x.RequestgenId)
                .ToList();
        }

        public TestRequestGenerate GetRequestGenerate(int id)
        {
            return _context.OracleContextConnectionString
                .Query<TestRequestGenerate>(@"SELECT * FROM TBLREQUESTGEN")
                .Single(x => x.TestRequestId == id);
        }

        public TestRequestGenerate GetRequestGenerate(string name)
        {
            return _context.OracleContextConnectionString
                .Query<TestRequestGenerate>(@"SELECT * FROM TBLREQUESTGEN")
                .Single(x => x.PCode.Equals(name));
        }

        public int MaxRequestGenId
        {
            get
            {
                return _context.OracleContextConnectionString
                    .Query<int>(@"SELECT MAX(REQUESTGENID) FROM TBLREQUESTGEN").Single() + 1;
                /* _context.OracleContextConnectionString
                    .Query<bool>("SELECT REQUESTGENID FROM TBLREQUESTGEN").Any() ?
                    _context.OracleContextConnectionString
                    .Query<int>("SELECT MAX(REQUESTGENID) FROM TBLREQUESTGEN").Single() + 1 : 1;*/
            }
        }

        public int GetCurrentShift(string currenttime, string currentdate)
        {
            var starttime = int.Parse(currenttime.Substring(0, 2));
            var startdate = "1385/01/10";
            var datediff = (PersianDateTime.Parse(startdate).ToDateTime() -
                            PersianDateTime.Parse(currentdate).ToDateTime()).Days % 16;
            datediff = Math.Abs(datediff);
            switch (datediff)
            {
                case 0: datediff = 0; break;
                case 1: datediff = 0; break;
                case 2: datediff = 0; break;
                case 3: datediff = 0; break;
                case 4: datediff = 1; break;
                case 5: datediff = 1; break;
                case 6: datediff = 1; break;
                case 7: datediff = 1; break;
                case 8: datediff = 2; break;
                case 9: datediff = 2; break;
                case 10: datediff = 2; break;
                case 11: datediff = 2; break;
                case 12: datediff = 3; break;
                case 13: datediff = 3; break;
                case 14: datediff = 3; break;
                case 15: datediff = 3; break;
            }

            if (starttime >= 6 && starttime < 14)
            { starttime = 0; }
            else if (starttime >= 14 && starttime < 22)
            { starttime = 1; }
            else if (starttime >= 22)
            { starttime = 2; }
            else if (starttime < 6)
            { starttime = 2; }

            switch (starttime)
            {
                case 0: return (1 + datediff) % 4;
                case 3: return (2 + datediff) % 4;
                case 2: return (3 + datediff) % 4;
                case 1: return (4 + datediff) % 4;
                default: return 0;
            }

        }

        public void Add(TestRequestGenerate entity)
        {
            var sqlquery = @"INSERT INTO CENTRAL_LAB.TBLREQUESTGEN
            (REQUESTGENID,TESTREQUESTID,PCODE,PCODEDATE,PCODETIME,REQUESTDATE
            ,REQUESTTIME,UNITSENDDATE,UNITSENDTIME,UNITSENDCONFIRM,UNITNOTSENDDES
            ,TESTTYPE,UNITCONTROL,REQUESTTYPE,CREATEID,CONFIRMID,CONFIRM,STATUS
            ,TESTCOUNT,REQUESTFINISH,TESTEXE,TESTREJECT,RECEIVEDATE,RECEIVETIME
            ,SHIFT,SAMPLENO,VESELNO,LATNO,SAMPLEMAN) 
             VALUES(:REQUESTGENID,:TESTREQUESTID,:PCODE,:PCODEDATE,:PCODETIME,:REQUESTDATE
             ,:REQUESTTIME,:UNITSENDDATE,:UNITSENDTIME,:UNITSENDCONFIRM,:UNITNOTSENDDES
             ,:TESTTYPE,:UNITCONTROL,:REQUESTTYPE,:CREATEID,:CONFIRMID,:CONFIRM,:STATUS
             ,:TESTCOUNT,:REQUESTFINISH,:TESTEXE,:TESTREJECT,:RECEIVEDATE,:RECEIVETIME
             ,:SHIFT,:SAMPLENO,:VESELNO,:LATNO,:SAMPLEMAN)";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new
            {
                entity.RequestgenId,
                entity.TestRequestId,
                entity.PCode,
                entity.PCodeDate,
                entity.PCodeTime,
                entity.RequestDate,
                entity.RequestTime,
                entity.UnitSendDate,
                entity.UnitSendTime,
                entity.UnitSendConfirm,
                entity.UnitNotSendDes,
                entity.TestType,
                entity.UnitControl,
                entity.RequestType,
                entity.CreateId,
                entity.ConfirmId,
                entity.Confirm,
                entity.Status,
                entity.TestCount,
                entity.RequestFinish,
                entity.TestExe,
                entity.TestReject,
                entity.ReceiveDate,
                entity.ReceiveTime,
                entity.Shift,
                entity.SampleNo,
                entity.VeselNo,
                entity.LatNo,
                entity.SampleMan
            });
        }

        public void Edit(int id, TestRequestGenerate entity)
        {
            var sqlquery = @"UPDATE TBLREQUESTGEN 
             SET TESTREQUESTID =:TESTREQUESTID ,PCODE =:PCODE,PCODEDATE = :PCODEDATE
            ,PCODETIME = :PCODETIME,REQUESTDATE = :REQUESTDAT,REQUESTTIME = :REQUESTTIME
            ,UNITSENDDATE = :UNITSENDDATE,UNITSENDTIME = :UNITSENDTIME
            ,UNITSENDCONFIRM =:UNITSENDCONFIR,UNITNOTSENDDES = :UNITNOTSENDDES
            ,TESTTYPE =:TESTTYPE,UNITCONTROL = :UNITCONTROL,REQUESTTYPE =:REQUESTTYPE
            ,CREATEID = :CREATEID  ,CONFIRMID = :CONFIRMID,
            ,CONFIRM = :CONFIRM ,STATUS = :STATUS,TESTCOUNT = :TESTCOUNT
            ,REQUESTFINISH =:REQUESTFINISH,TESTEXE = :TESTEXE,TESTREJECT = :TESTREJECT
            ,RECEIVEDATE =:RECEIVEDATE,RECEIVETIME =:RECEIVETIME,SHIFT = :SHIFT
            ,SAMPLENO = :SAMPLENO,VESELNO = :VESELNO ,LATNO =  :LATNO
            ,SAMPLEMAN = :SAMPLEMAN WHERE REQUESTGENID = :REQUESTGENID";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new
                {
                    entity.TestRequestId,
                    entity.PCode,
                    entity.PCodeDate,
                    entity.PCodeTime,
                    entity.RequestDate,
                    entity.RequestTime,
                    entity.UnitSendDate,
                    entity.UnitSendTime,
                    entity.UnitSendConfirm,
                    entity.UnitNotSendDes,
                    entity.TestType,
                    entity.UnitControl,
                    entity.RequestType,
                    entity.CreateId,
                    entity.ConfirmId,
                    entity.Confirm,
                    entity.Status,
                    entity.TestCount,
                    entity.RequestFinish,
                    entity.TestExe,
                    entity.TestReject,
                    entity.ReceiveDate,
                    entity.ReceiveTime,
                    entity.VeselNo,
                    entity.LatNo,
                    entity.SampleMan,
                    REQUESTGENID = id
                });
        }

        public void Delete(int id)
        {
            var sqlquery = @"DELETE FROM CENTRAL_LAB.TBLREQUESTGEN WHERE REQUESTGENID =:REQUESTGENID ";
            _context.OracleContextConnectionString
                .Execute(sqlquery, new { REQUESTGENID = id });
        }

        public void Delete(string name)
        {
            var sqlquery = @"DELETE FROM CENTRAL_LAB.TBLREQUESTGEN WHERE TESTREQUESTID =:TESTREQUESTID ";
            _context.OracleContextConnectionString
            .Execute(sqlquery, new { TESTREQUESTID = name });
        }
    }
}