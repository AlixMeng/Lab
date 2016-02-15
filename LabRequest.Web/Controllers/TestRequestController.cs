using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using LabRequest.Web.Infrastracture;
using LabRequest.Web.ViewModel;
using LabRequest.DomainModel.Repository;
using LabRequest.DomainModel.Entities;
using PagedList;
using PagedList.Mvc;
using System;

namespace LabRequest.Web.Controllers
{

    public class TestRequestController : Controller
    {
        private readonly TestRequestRepository _testrequestrepository = new TestRequestRepository();
        private readonly UserRepository _userrepository = new UserRepository();
        private readonly TestRequestGenerateRepository _testrequestgenrepository = new TestRequestGenerateRepository();
        private readonly DateTimeRepository _datetimerepository = new DateTimeRepository();
        private readonly RequestTestRepository _requesttestrepository = new RequestTestRepository();
        private readonly TestRepository _testrepository = new TestRepository();


        [Authorize]
        [OutputCache(Location = OutputCacheLocation.Client,
            Duration = 0,
            VaryByParam = "*")]
        public ActionResult TestRequestList(string searchstring,
            string currentfilter,
            string sortorder, int? page)
        {
            ViewBag.CurrentSort = sortorder;

            ViewBag.RequestNoSortParm = sortorder == "RequestNo" ? "RequestNoDesc" : "RequestNo";
            ViewBag.ReqDateSortParm = sortorder == "ReqDate" ? "ReqDateDesc" : "ReqDate";
            ViewBag.SampleNameSortParm = sortorder == "SampleName" ? "SampleNameDesc" : "SampleName";
            ViewBag.RequestTimeSortParm = sortorder == "RequestTime" ? "RequestTimeDesc" : "RequestTime";
            ViewBag.LatNoSortParm = sortorder == "LatNo" ? "LatNoDesc" : "LatNo";
            ViewBag.PersonFamilySortParm = sortorder == "PersonFamily" ? "PersonFamilyDesc" : "PersonFamily";

            if (!string.IsNullOrEmpty(searchstring))
            {
                page = 1;
            }
            else
            {
                searchstring = currentfilter;
            }
            ViewBag.CurrentFilter = searchstring;


            const int pagesize = 10;
            var pagrnumber = (page ?? 1);
            return View(GetListOfRequest(sortorder, searchstring)
                .ToPagedList(pagrnumber, pagesize));
        }


        [OutputCache(Location = OutputCacheLocation.Client,
        Duration = 0,
        VaryByParam = "reqno")]
        public ActionResult TestRequestResult(string reqno)
        {
            ViewBag.reqno = reqno;
            return View();
        }

        [Authorize]
        public ActionResult TestRequestAdd()
        {
            var userid = _userrepository.GetUser(User.Identity.Name,
                int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId;
            ViewBag.Companies = new SelectList(_testrequestrepository.GetAllCompanies(userid),
                "IdRec",
                "Des");
            ViewBag.Units = new SelectList(new List<string>(), "ApplicantId", "ApplicantName");
            ViewBag.RequestTitles = new SelectList(new List<string>(), "IdRec", "Des");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestRequestAdd(TestRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = new SelectList(_testrequestrepository.GetAllCompanies(
                    _userrepository.GetUser(User.Identity.Name, int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId),
                 "IdRec", "Des", int.Parse(model.Company));
                ViewBag.Units = new SelectList(new List<string>(), "ApplicantId", "ApplicantName");
                ViewBag.RequestTitles = new SelectList(new List<string>(), "IdRec", "Des");
                return View(model);
            }


            if (model.RequestDetail == null)
            {
                ModelState.AddModelError("", "حداقل یک عنوان درخواست از جزئیات میبایست وارد شود");
                ViewBag.Companies = new SelectList(_testrequestrepository.GetAllCompanies(
                    _userrepository.GetUser(User.Identity.Name,
                    int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId),
                 "IdRec", "Des", int.Parse(model.Company));
                ViewBag.Units = new SelectList(new List<string>(), "ApplicantId", "ApplicantName");
                ViewBag.RequestTitles = new SelectList(new List<string>(), "IdRec", "Des");
                return View(model);
            }

            var user = _userrepository.GetUser(User.Identity.Name,
                int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value)));


            var testreq = new TestRequest
            {
                TestRequestId = _testrequestrepository.MaxTextRequestId,
                RequestNo = _testrequestrepository.RequestNewCode(model.RequestType),
                ReqDate = model.RequestDate,
                StartDate = model.RequestDate,
                EndDate = model.RequestDate,
                ApplicantId = int.Parse(model.Unit),
                ReqType = 1,
                CreateId = user.PersonId,
                ConfirmId = 0,
                Confirm = "N",
                Enable = false,
                SampleName = model.SampleName,
                Contract = _testrequestrepository.RequestNewCode(model.RequestType),
                Com_Id = user.Com_Id
            };
            _testrequestrepository.Add(testreq);



            var testreqgen = new TestRequestGenerate
            {
                RequestgenId = _testrequestgenrepository.MaxRequestGenId,
                TestRequestId = testreq.TestRequestId,
                PCode = testreq.RequestNo,
                PCodeDate = model.RequestDate,
                PCodeTime = _datetimerepository.GetShortLocalTime,
                RequestDate = model.RequestDate,
                RequestTime = _datetimerepository.GetShortLocalTime,
                UnitSendDate = model.RequestDate,
                UnitSendTime = _datetimerepository.GetShortLocalTime,
                UnitSendConfirm = 1,
                UnitNotSendDes = string.Empty,
                TestType = 1,
                UnitControl = 0,
                RequestType = (int)model.RequestType,
                CreateId = testreq.CreateId,
                ConfirmId = 0,
                Confirm = (int)model.RequestPriority,
                Status = 0,
                TestCount = 0,
                RequestFinish = 0,
                TestExe = 0,
                TestReject = 0,
                ReceiveDate = string.Empty,
                ReceiveTime = string.Empty,
                Shift = _testrequestgenrepository
                    .GetCurrentShift(_datetimerepository.GetLongLocalTime
                        , _datetimerepository.GetLocalDate),
                SampleNo = string.Empty,
                VeselNo = string.Empty,
                LatNo = model.LotNumber,
                SampleMan = model.SampleName
            };
            _testrequestgenrepository.Add(testreqgen);



            foreach (var t in model.RequestDetail)
            {
                var reqtest = new RequestTest
                {
                    ReqTestsId = _requesttestrepository.Max,
                    RequestGenId = testreqgen.RequestgenId,
                    TestRequestId = testreq.TestRequestId,
                    RequestNameId = int.Parse(model.RequestTitle),
                    TestId = int.Parse(t),
                    ConfirmId = 0,
                    Confirm = 0,
                    ExeDate = string.Empty,
                    ExeTime = string.Empty,
                    ExeManId = 0,
                    TestState = 0,
                    TestDes = string.Empty,
                    ReTestCount = 0,
                    ConfirmDate = string.Empty,
                    ConfirmTime = string.Empty,
                    MachinId = 0,
                    Price = _testrepository.GetPrice(int.Parse(t)).Price,
                    PersonShift = 0,
                    ExeShift = 0
                };
                _requesttestrepository.Add(reqtest);
            }


            return RedirectToAction("TestRequestResult", new { reqno = testreq.RequestNo });
        }


        [OutputCache(Location = OutputCacheLocation.Client,
        Duration = 0,
        VaryByParam = "id")]
        public JsonResult GetRequestTitles(int id)
        {
            return Json(new SelectList(_testrequestrepository.GetAllRequestTitles()
                .Where(x => x.ApplicantId == id)
                .ToList(), "IdRec", "Des"), JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.Client,
        Duration = 0,
        VaryByParam = "id")]
        public JsonResult GetUnitName(int id)
        {
            var userid = _userrepository.GetUser(User.Identity.Name,
                int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId;
            return Json(new SelectList(_testrequestrepository.GetAllUnits(userid)
                .Where(x => x.CompanyId == id),
                "ApplicantId", "ApplicantName").ToList(),
                JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.Client,
        Duration = 0,
        VaryByParam = "id")]
        public JsonResult GetDetailsOfRequestTitle(int id)
        {
            return Json(new SelectList(_testrequestrepository.GetAllTestRequestTitleDetails()
                .Where(x => x.RequestNameId == id)
                , "TestId", "TestName"), JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.Client,
        Duration = 0,
        VaryByParam = "*")]
        public JsonResult GetAllRequestDetails(int id, int reqgenid)
        {
            return Json(
                _testrequestrepository.GetAllTestRequestDetails(id, reqgenid)
                .Select(x => new
            {
                test = x.TestName,
                date = x.ExeDate,
                time = x.ExeTime,
                reqtestid = x.ReqTestsID,
                confirm =
                x.Confirm == 0 ? "بررسي نشده"
                : x.Confirm == 1 ? "انجام شد"
                : x.Confirm == 2 ? "عدم انجام"
                : x.Confirm == 3 ? "تائيد"
                : x.Confirm == 4 ? "تائيد عدم انجام"
                : "بازکاري"
            }),
                JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.Client,
         Duration = 0,
         VaryByParam = "reqtestid")]
        public JsonResult ConfirmReqTests(int[] reqtestid)
        {
            foreach (var req in reqtestid)
            {
                _testrequestrepository.Edit(req);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        public List<TestRequestGenerateUsers> GetListOfRequest(string sortorder, string searchstring)
        {
            var user = _userrepository.GetUser(User.Identity.Name,
                int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value))).PersonId;
            switch (sortorder)
            {
                case "RequestNo":
                    ViewBag.RequestNoFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.RequestNo).ToList();

                case "RequestNoDesc":
                    ViewBag.RequestNoFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.RequestNo).ToList();

                case "ReqDate": ViewBag.ReqDateFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.ReqDate).ToList();

                case "ReqDateDesc":
                    ViewBag.ReqDateFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.ReqDate).ToList();

                case "SampleName":
                    ViewBag.SampleNameFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.SampleName).ToList();

                case "SampleNameDesc":
                    ViewBag.SampleNameFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.SampleName).ToList();

                case "RequestTime":
                    ViewBag.RequestTimeFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.RequestTime).ToList();

                case "RequestTimeDesc":
                    ViewBag.RequestTimeFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.RequestTime).ToList();

                case "LatNo":
                    ViewBag.LatNoFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.LatNo).ToList();

                case "LatNoDesc":
                    ViewBag.LatNoFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.LatNo).ToList();

                case "PersonFamily":
                    ViewBag.PersonFamilyFaIcon = "fa-sort-amount-asc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderBy(x => x.PersonFamily).ToList();

                case "PersonFamilyDesc":
                    ViewBag.PersonFamilyFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.PersonFamily).ToList();

                default:
                    ViewBag.ReqDateFaIcon = "fa-sort-amount-desc";
                    return _testrequestrepository.GetAllTestRequestGenerateUsers(user,
                        _datetimerepository.GetYear, searchstring)
                        .OrderByDescending(x => x.ReqDate).ToList();
            }
        }

    }
}
