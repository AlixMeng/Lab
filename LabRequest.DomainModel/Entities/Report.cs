using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabRequest.DomainModel.Entities
{
    public class Report
    {
        public string ReportCompany { get; set; }
        public string ReportUnit { get; set; }
        public string ReportRequestTitle { get; set; }
        public string ReportTestName { get; set; }
        public string ReportSampleType { get; set; }
        public string ReportSampleName { get; set; }
        public string ReportRequestNo { get; set; }
        public string ReportRequestState { get; set; }
        public string ReportFromDate { get; set; }
        public string ReportToDate { get; set; }
        public string ReportType { get; set; }
    }
}
