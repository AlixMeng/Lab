using LabRequest.DomainModel.Service;
using Org.BouncyCastle.Asn1.X509.SigI;

namespace LabRequest.DomainModel.Repository
{
    public class DateTimeRepository : IDateTime
    {
        public string GetLocalDate { get { return PersianDateTime.Now.ToString(PersianDateTimeFormat.Date); } }
        public string GetLongLocalTime { get { return PersianDateTime.Now.ToString("hh:mm:ss"); } }
        public string GetShortLocalTime { get { return PersianDateTime.Now.ToString("hh:mm"); } }
        public string GetYear { get { return PersianDateTime.Now.Year.ToString(); } }
        public string GetDateFromYear { get { return string.Format("{0}/01/01", PersianDateTime.Now.Year); } }
    }
}