using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LabRequest.DomainModel.Entities;

namespace LabRequest.DomainModel.Service
{
    public interface IReport
    {
        List<TestResultReport> TestResult(Report report,string year);
        List<UserTestResultListReport> TestResultlist(Report report,string year);
        List<UserTestStatuslist> TestStatusList(Report report, string year);
        List<UserCompanyOperation> CompanyOperationList(Report report, string year);
    }
}
