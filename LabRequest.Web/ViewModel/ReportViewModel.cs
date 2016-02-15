using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LabRequest.Web.ViewModel
{
    public class ReportViewModel
    {
        [Display(Name = "نام شرکت")]
        public string ReportCompany { get; set; }
        [Display(Name = "نام واحد")]
        public string ReportUnit { get; set; }
        [Display(Name = "عنوان درخواست")]
        public string ReportRequestTitle { get; set; }
        [Display(Name = "نام تست")]
        public string ReportTestName { get; set; }
        [Display(Name = "نوع نمونه")]
        public string ReportSampleType { get; set; }
        [Display(Name = "نام نمونه")]
        public string ReportSampleName { get; set; }
        [Display(Name = "کد پیگیری")]
        public string ReportRequestNo { get; set; }
        [Display(Name = "وضعیت")]
        public string ReportRequestState { get; set; }
        [Display(Name = "تاریخ شروع")]
        public string ReportFromDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        public string ReportToDate { get; set; }
        public string ReportType { get; set; }

        public bool HasAllEmptyProperties()
        {
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var hasProperty = properties
                .Where(x => !x.Name.Contains("ReportType"))
                .Select(x => x.GetValue(this, null))
                .Any(y => y != null && !String.IsNullOrWhiteSpace(y.ToString()));
            return !hasProperty;
        }

    }
}