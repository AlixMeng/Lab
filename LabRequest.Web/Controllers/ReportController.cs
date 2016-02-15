using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using LabRequest.DomainModel.Repository;
using LabRequest.Web.ViewModel;
using LabRequest.DomainModel.Entities;
using LabRequest.Web.Infrastracture;
using AutoMapper;

namespace LabRequest.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly TestRequestRepository _requestrepository = new TestRequestRepository();
        private readonly UserRepository _userrepository = new UserRepository();
        private readonly TestRepository _testrepository = new TestRepository();
        private readonly DateTimeRepository _daterepository = new DateTimeRepository();


        [Authorize]
        [OutputCache(Location = OutputCacheLocation.Client,
            Duration = 60,
            VaryByParam = "none")]

        public ActionResult PdfReport()
        {
            PopulateDropDownList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PdfReport(ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDownList();
                return View();
            }
            if (model.HasAllEmptyProperties())
            {
                ModelState.AddModelError("", "برای نمایش گزارش حداقل یک ایتم انتخاب شود");
                PopulateDropDownList();
                return View();
            }

            var report = new GeneratePdfReport();
            report.CreatePdfReport(AutoMapperHelper.Map<ReportViewModel, Report>(model));
            ModelState.Clear();
            PopulateDropDownList();
            return RedirectToAction("PdfReport");
        }


        public void PopulateDropDownList()
        {
            var userid = _userrepository.GetUser(User.Identity.Name,
               int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId;

            ViewBag.Companies = new SelectList(_requestrepository.GetAllCompanies(userid),
                "IdRec",
                "Des", 0);

            ViewBag.Units = new SelectList(_requestrepository.GetAllUnits(userid),
                "ApplicantId",
                "ApplicantName");

            ViewBag.RequstTitles = new SelectList(_requestrepository.GetAllRequestTitles(userid),
                "IdRec",
                "Des");

            ViewBag.Tests = new SelectList(_testrepository.GetAllTests().Where(x=>x.Active == 0), "TestId", "TestName");

            ViewBag.SampleType = new SelectList(_requestrepository.GetAllSampleType(9),
                "IdRec",
                "Des");

            ViewBag.SampleName = new SelectList(_requestrepository
                .GetAllSampleName(_daterepository.GetDateFromYear),
                "TESTREQUESTID",
                "SAMPLENAME");

            var states = new List<StateViewModel> { 
            new StateViewModel{StateId =0 ,StateName = "بررسی نشده"},
            new StateViewModel{StateId =1 ,StateName = "انجام شد"},
            new StateViewModel{StateId =2 ,StateName = "عدم انجام"},
            new StateViewModel{StateId =3 ,StateName = "تایید"},
            new StateViewModel{StateId =4 ,StateName = "تایید عدم انجام"},
            new StateViewModel{StateId =5 ,StateName = "بازکاری"},
            };
            ViewBag.RequestState = new SelectList(states.OrderBy(x => x.StateName),
                "StateId",
                "StateName");
        }

    }
}
