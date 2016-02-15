namespace LabRequest.DomainModel.Entities
{
    public class EnumCollection
    {
        public enum RequestGenRequestType
        {
            روتین = 1,
            غیررروتین = 2,
            Resample = 3,
            AsRequest = 4
        }

        public enum RequestGenStatus
        {
            عادی = 1,
            فوری = 2,
            اضطراری = 3,
            کنسل = 4
        }

        public enum ReqTestsTestState
        {
            Waiting = 0,
            Exe = 1,
            NotExe = 2,
            Confirmed = 3,
            ConfirmNotExe = 4,
            Repeat = 5
        }
    }
}