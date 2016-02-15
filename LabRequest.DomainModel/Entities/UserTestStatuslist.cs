namespace LabRequest.DomainModel.Entities
{
    public class UserTestStatuslist
    {
        private string _testState;
        public string ApplicantName { get; set; }
        public string Des { get; set; }
        public string LaboratoryId { get; set; }
        public string LaboratoryName { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }

        public string TestState
        {
            get
            {
                return (_testState == "0" ? "بررسی شده"
                    : _testState == "1" ? "انجام شده"
                    : _testState == "2" ? "عدم انجام"
                    : _testState == "3" ? "تایید"
                    : _testState == "1" ? "تایید عدم انجام"
                    : "بازکاری");
            }
            set { _testState = value; }
        }

        public string SampleName { get; set; }
        public string Pcode { get; set; }
        public string RequestDate { get; set; }
        public int RowNumber { get; set; }
    }
}