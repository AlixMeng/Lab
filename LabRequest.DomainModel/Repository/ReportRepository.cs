using System.Collections.Generic;
using System.Linq;
using LabRequest.DomainModel.Service;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using Dapper;


namespace LabRequest.DomainModel.Repository
{
    public class ReportRepository : IReport
    {
        private readonly OracleContext _context = new OracleContext();
        public List<TestResultReport> TestResult(Report report, string year)
        {
            var sqlquery = @"SELECT TBLAPPLICANT.APPLICANTNAME,TBLBASEINFO.DES, 
                            TBLTESTOUTPUT.OUTPUTNAME,TBLOUTPUTVALUE.OUTPUTVALUE,
                            TBLBASEINFO_RequestName.DES RequestNameDes, 
                            TBLBASEINFO_MeaSur.DES MeaSurDes ,
                            TBLTESTOUTPUT.RATE,TBLREQTESTS.EXEDATE, 
                            TBLTESTREQUEST.SAMPLENAME,TBLREQUESTGEN.PCODE,TBLREQTESTS.TESTSTATE  
                            FROM (((((((CENTRAL_LAB.TBLTESTREQUEST TBLTESTREQUEST  
                            INNER JOIN CENTRAL_LAB.TBLAPPLICANT TBLAPPLICANT  
                            ON TBLTESTREQUEST.APPLICANTID = TBLAPPLICANT.APPLICANTID)  
                            INNER JOIN CENTRAL_LAB.TBLREQUESTGEN TBLREQUESTGEN  
                            ON TBLTESTREQUEST.TESTREQUESTID = TBLREQUESTGEN.TESTREQUESTID)  
                            INNER JOIN CENTRAL_LAB.TBLREQTESTS TBLREQTESTS  
                            ON TBLREQUESTGEN.REQUESTGENID = TBLREQTESTS.REQUESTGENID)  
                            INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO_RequestName  
                            ON TBLREQTESTS.REQUESTNAMEID = TBLBASEINFO_RequestName.IDREC)  
                            INNER JOIN CENTRAL_LAB.TBLOUTPUTVALUE TBLOUTPUTVALUE  
                            ON TBLREQTESTS.REQTESTSID = TBLOUTPUTVALUE.REQTESTSID)  
                            INNER JOIN CENTRAL_LAB.TBLTESTOUTPUT TBLTESTOUTPUT  
                            ON(TBLOUTPUTVALUE.OUTPUTTESTID = TBLTESTOUTPUT.TESTOUTPUTID)  
                            AND (TBLOUTPUTVALUE.TESTID = TBLTESTOUTPUT.TESTID))  
                            INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO_MeaSur  
                            ON TBLOUTPUTVALUE.MEASUREUNITID = TBLBASEINFO_MeaSur.IDREC)  
                            INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO  
                            ON TBLAPPLICANT.COMPANYID = TBLBASEINFO.IDREC
                            WHERE TBLREQUESTGEN.PCODE iS NOT NULL";
            //WHERE TBLTESTREQUEST.REQDATE >="+ string.Format("'{0}/01/01'", year);
            sqlquery += !string.IsNullOrEmpty(report.ReportCompany)
                ? @" AND TBLBASEINFO.IDREC=" + int.Parse(report.ReportCompany)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportUnit)
                ? @" AND TBLTESTREQUEST.APPLICANTID=" + int.Parse(report.ReportUnit)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestTitle)
                ? @" AND TBLREQTESTS.REQUESTNAMEID=" + int.Parse(report.ReportRequestTitle)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleType)
                ? @" AND TBLTEST.MEASURETYPE=" + int.Parse(report.ReportSampleType)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportTestName)
                ? @" AND TBLREQTESTS.TESTID =" + int.Parse(report.ReportTestName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleName)
                ? @" AND tbltestrequest.SAMPLENAME=" + string.Format("'{0}'", report.ReportSampleName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportFromDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format(">='{0}'", report.ReportFromDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportToDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format("<='{0}'", report.ReportToDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestState)
                ? @" AND TBLREQTESTS.TESTSTATE=" + int.Parse(report.ReportRequestState)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestNo)
                ? @" AND TBLREQUESTGEN.PCODE=" + string.Format("'{0}'", report.ReportRequestNo)
                : string.Empty;
            sqlquery += @" ORDER BY TBLREQUESTGEN.PCODE, TBLTESTOUTPUT.RATE";
            return _context.OracleContextConnectionString
            .Query<TestResultReport>(sqlquery).ToList();
        }
        public List<UserTestResultListReport> TestResultlist(Report report, string year)
        {
            var sqlquery = @"SELECT TBLTESTOUTPUT.OUTPUTNAME,
                            TBLOUTPUTVALUE.OUTPUTVALUE,
                            TBLBASEINFO_MeaSur.DES,
                            TBLREQTESTS.EXEDATE,
                            TBLTESTREQUEST.SAMPLENAME,
                            TBLREQUESTGEN.PCODE,
                            TBLTESTOUTPUT.TESTOUTPUTID,
                            TBLREQTESTS.TESTSTATE,
                            TBLREQTESTS.CONFIRMDATE,
                            TBLREQTESTS.CONFIRMTIME,
                            TBLTEST.SPEC,
                            ROW_NUMBER() OVER (ORDER BY TBLTESTOUTPUT.OUTPUTNAME) ROWNUMBER
                            FROM (((((CENTRAL_LAB.TBLTESTREQUEST TBLTESTREQUEST
                            INNER JOIN CENTRAL_LAB.TBLREQUESTGEN TBLREQUESTGEN
                            ON TBLTESTREQUEST.TESTREQUESTID = TBLREQUESTGEN.TESTREQUESTID)
                            INNER JOIN CENTRAL_LAB.TBLREQTESTS TBLREQTESTS
                            ON TBLREQUESTGEN.REQUESTGENID = TBLREQTESTS.REQUESTGENID)
                            INNER JOIN CENTRAL_LAB.TBLTEST TBLTEST
                            ON TBLREQTESTS.TESTID = TBLTEST.TESTID)
                            INNER JOIN CENTRAL_LAB.TBLOUTPUTVALUE TBLOUTPUTVALUE
                            ON (TBLREQTESTS.REQTESTSID = TBLOUTPUTVALUE.REQTESTSID)
                            AND (TBLTEST.TESTID = TBLOUTPUTVALUE.TESTID))
                            INNER JOIN CENTRAL_LAB.TBLTESTOUTPUT TBLTESTOUTPUT
                            ON (TBLOUTPUTVALUE.OUTPUTTESTID = TBLTESTOUTPUT.TESTOUTPUTID)
                            AND (TBLOUTPUTVALUE.TESTID = TBLTESTOUTPUT.TESTID))
                            INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO_MeaSur
                            ON TBLOUTPUTVALUE.MEASUREUNITID = TBLBASEINFO_MeaSur.IDREC
                            WHERE TBLREQUESTGEN.PCODE iS NOT NULL";
            //WHERE TBLTESTREQUEST.REQDATE >="+ string.Format("'{0}/01/01'", year);
            //sqlquery += !string.IsNullOrEmpty(report.ReportCompany)
            //    ? @" AND TBLBASEINFO_MeaSur.IDREC=" + int.Parse(report.ReportCompany)
            //    : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportUnit)
                ? @" AND TBLTESTREQUEST.APPLICANTID=" + int.Parse(report.ReportUnit)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestTitle)
                ? @" AND TBLREQTESTS.REQUESTNAMEID=" + int.Parse(report.ReportRequestTitle)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleType)
                ? @" AND TBLTEST.MEASURETYPE=" + int.Parse(report.ReportSampleType)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportTestName)
                ? @" AND TBLREQTESTS.TESTID =" + int.Parse(report.ReportTestName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleName)
                ? @" AND tbltestrequest.SAMPLENAME=" + string.Format("'{0}'", report.ReportSampleName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportFromDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format(">='{0}'", report.ReportFromDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportToDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format("<='{0}'", report.ReportToDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestState)
                ? @" AND TBLREQTESTS.TESTSTATE=" + int.Parse(report.ReportRequestState)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestNo)
                ? @" AND TBLREQUESTGEN.PCODE=" + string.Format("'{0}'", report.ReportRequestNo)
                : string.Empty;
            //sqlquery += @" ORDER BY TBLTESTOUTPUT.TESTOUTPUTID,TBLREQUESTGEN.PCODE";
            return _context.OracleContextConnectionString
                .Query<UserTestResultListReport>(sqlquery).ToList();
        }

        public List<UserTestStatuslist> TestStatusList(Report report, string year)
        {
            var sqlquery = @"SELECT DISTINCT TBLAPPLICANT.APPLICANTNAME,
                TBLBASEINFO.DES,
                TBLLABORATORY.LABORATORYID,
                TBLLABORATORY.LABORATORYNAME,
                TBLUNIT.UNITID,
                TBLUNIT.UNITNAME,
                TBLREQTESTS.TESTSTATE,
                TBLTESTREQUEST.SAMPLENAME,
                TBLREQUESTGEN.PCODE,
                TBLREQUESTGEN.REQUESTDATE--,ROWNUM ROWNUMBER
                FROM ((((((CENTRAL_LAB.TBLTESTREQUEST TBLTESTREQUEST
                INNER JOIN CENTRAL_LAB.TBLAPPLICANT TBLAPPLICANT
                  ON TBLTESTREQUEST.APPLICANTID = TBLAPPLICANT.APPLICANTID)
                INNER JOIN CENTRAL_LAB.TBLREQUESTGEN TBLREQUESTGEN
                  ON TBLTESTREQUEST.TESTREQUESTID = TBLREQUESTGEN.TESTREQUESTID)
                INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO
                  ON TBLAPPLICANT.COMPANYID = TBLBASEINFO.IDREC)
                INNER JOIN CENTRAL_LAB.TBLREQTESTS TBLREQTESTS
                  ON TBLREQUESTGEN.REQUESTGENID = TBLREQTESTS.REQUESTGENID)
                INNER JOIN CENTRAL_LAB.TBLTEST TBLTEST
                  ON TBLREQTESTS.TESTID = TBLTEST.TESTID)
                INNER JOIN CENTRAL_LAB.TBLUNIT TBLUNIT
                  ON TBLTEST.UNITID = TBLUNIT.UNITID)
                INNER JOIN CENTRAL_LAB.TBLLABORATORY TBLLABORATORY
                  ON TBLUNIT.LABORATORYID = TBLLABORATORY.LABORATORYID
                   WHERE TBLREQUESTGEN.PCODE iS NOT NULL";
            //WHERE TBLTESTREQUEST.REQDATE >=" + string.Format("'{0}/01/01'", year);
            //WHERE  ROWNUM >= 1 AND ROWNUM <= 1000000";
            //ORDER BY TBLLABORATORY.LABORATORYID,TBLUNIT.UNITID";
            sqlquery += !string.IsNullOrEmpty(report.ReportCompany)
                ? @" AND TBLBASEINFO.IDREC=" + int.Parse(report.ReportCompany)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportUnit)
                ? @" AND TBLTESTREQUEST.APPLICANTID=" + int.Parse(report.ReportUnit)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestTitle)
                ? @" AND TBLREQTESTS.REQUESTNAMEID=" + int.Parse(report.ReportRequestTitle)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleType)
                ? @" AND TBLTEST.MEASURETYPE=" + int.Parse(report.ReportSampleType)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportTestName)
                ? @" AND TBLREQTESTS.TESTID =" + int.Parse(report.ReportTestName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleName)
                ? @" AND tbltestrequest.SAMPLENAME=" + string.Format("'{0}'", report.ReportSampleName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportFromDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format(">='{0}'", report.ReportFromDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportToDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format("<='{0}'", report.ReportToDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestState)
                ? @" AND TBLREQTESTS.TESTSTATE=" + int.Parse(report.ReportRequestState)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestNo)
                ? @" AND TBLREQUESTGEN.PCODE=" + string.Format("'{0}'", report.ReportRequestNo)
                : string.Empty;
            //sqlquery += @" ORDER BY ROWNUM ASC";
            return _context.OracleContextConnectionString
                .Query<UserTestStatuslist>(sqlquery).ToList();
        }

        public List<UserCompanyOperation> CompanyOperationList(Report report, string year)
        {
            var sqlquery = @"SELECT DISTINCT TBLTEST.TESTNAME,
                TBLTEST.PRICE,
                TBLBASEINFO_1.DES,
                TBLAPPLICANT.COMPANYID,
                TBLAPPLICANT.APPLICANTNAME,
                TBLBASEINFO.DES DESCRIPTION,
                TBLTESTREQUEST.SAMPLENAME,
                TBLREQUESTGEN.PCODE
                FROM (((((CENTRAL_LAB.TBLAPPLICANT TBLAPPLICANT
                INNER JOIN CENTRAL_LAB.TBLTESTREQUEST TBLTESTREQUEST
                    ON TBLAPPLICANT.APPLICANTID = TBLTESTREQUEST.APPLICANTID)
                INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO_1
                    ON TBLAPPLICANT.COMPANYID = TBLBASEINFO_1.IDREC)
                INNER JOIN CENTRAL_LAB.TBLREQUESTGEN TBLREQUESTGEN
                    ON TBLTESTREQUEST.TESTREQUESTID = TBLREQUESTGEN.TESTREQUESTID)
                INNER JOIN CENTRAL_LAB.TBLREQTESTS TBLREQTESTS
                    ON TBLREQUESTGEN.REQUESTGENID = TBLREQTESTS.REQUESTGENID)
                INNER JOIN CENTRAL_LAB.TBLTEST TBLTEST
                    ON TBLREQTESTS.TESTID = TBLTEST.TESTID)
                INNER JOIN CENTRAL_LAB.TBLBASEINFO TBLBASEINFO
                    ON TBLTEST.MEASUREUNIT = TBLBASEINFO.IDREC
                WHERE TBLREQUESTGEN.PCODE IS NOT NULL";
            //"ORDER BY TBLAPPLICANT.COMPANYID,TBLAPPLICANT.APPLICANTNAME"
            sqlquery += !string.IsNullOrEmpty(report.ReportCompany)
                ? @" AND TBLAPPLICANT.COMPANYID=" + int.Parse(report.ReportCompany)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportUnit)
                ? @" AND TBLAPPLICANT.APPLICANTID=" + int.Parse(report.ReportUnit)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportSampleName)
                ? @" AND tbltestrequest.SAMPLENAME=" + string.Format("'{0}'", report.ReportSampleName)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportFromDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format(">='{0}'", report.ReportFromDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportToDate)
                ? @" AND TBLREQUESTGEN.PCODEDATE" + string.Format("<='{0}'", report.ReportToDate)
                : string.Empty;
            sqlquery += !string.IsNullOrEmpty(report.ReportRequestState)
                ? @" AND TBLREQTESTS.TESTSTATE=" + int.Parse(report.ReportRequestState)
                : string.Empty;
            return _context.OracleContextConnectionString
                .Query<UserCompanyOperation>(sqlquery)
                .ToList();
        }
    }
}
