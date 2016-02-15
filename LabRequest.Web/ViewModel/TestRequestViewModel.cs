using System.ComponentModel.DataAnnotations;
using LabRequest.DomainModel.Entities;

namespace LabRequest.Web.ViewModel
{
    public class TestRequestViewModel
    {
        [Display(Name = "نوع درخواست")]
        public EnumCollection.RequestGenRequestType RequestType { get; set; }

        [Display(Name = "تاریخ درخواست")]
        [Required(ErrorMessage="تاریخ درخواست را وارد کنید")]
        public string RequestDate { get; set; }

        [Display(Name = "اولویت درخواست")]
        [Required(ErrorMessage = "اولویت درخواست را وارد کنید")]
        public EnumCollection.RequestGenStatus RequestPriority { get; set; }

        [Display(Name = "نام شرکت")]
        [Required(ErrorMessage = "نام شرکت درخواست کننده را وارد کنید")]
        public string Company { get; set; }

        [Display(Name = "واحد درخواست کننده")]
        [Required(ErrorMessage = "نام واحد درخواست کننده را وارد کنید")]
        public string Unit { get; set; }

        [Display(Name = "نام نمونه")]
        [Required(ErrorMessage = "نام نمونه درخواست را وارد کنید")]
        public string SampleName { get; set; }

        [Display(Name = "مشخصات نمونه گیر")]
        [Required(ErrorMessage = "مشخصات نمونه گیر درخواست را وارد کنید")]
        public string RequestPersonName { get; set; }

        [Display(Name = "LotNo")]
        [Required(ErrorMessage = "شماره لات و یا شماره تولید را وارد کنید")]
        public string LotNumber { get; set; }

        [Display(Name = "عنوان داخواست")]
        [Required(ErrorMessage = "عنوان درخواست انتخاب شود")]
        public string RequestTitle { get; set; }

        public string[] RequestDetail { get; set; }
    }
}