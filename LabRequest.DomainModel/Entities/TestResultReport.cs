using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabRequest.DomainModel.Entities
{
    public class TestResultReport
    {
        public string ApplicantName { get; set; }
        public string Des { get; set; }
        public string OutputName { get; set; }
        public string OutputValue { get; set; }
        public string RequestNameDes { get; set; }
        public string MeasurDes { get; set; }
        public string Rate { get; set; }
        public string ExeDate { get; set; }
        public string SampleName { get; set; }
        public string Pcode { get; set; }
        public string TestState { get; set; }
    }
}
