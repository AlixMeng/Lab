namespace LabRequest.DomainModel.Entities
{
    public class TestRequest
    {
        public int TestRequestId { get; set; }
        public string RequestNo { get; set; }
        public string ReqDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ApplicantId { get; set; }
        public int ReqType { get; set; }
        public int CreateId { get; set; }
        public int ConfirmId { get; set; }
        public string Confirm { get; set; }
        public bool Enable { get; set; }
        public string SampleName { get; set; }
        public string Contract { get; set; }
        public int Com_Id { get; set; }
    }
}
