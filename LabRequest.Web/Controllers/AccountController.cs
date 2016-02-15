using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using LabRequest.DomainModel.Entities;
using LabRequest.Web.Infrastracture;
using LabRequest.Web.ViewModel;
using CaptchaMvc.HtmlHelpers;
using LabRequest.DomainModel.Repository;

namespace LabRequest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository =
            new UserRepository();
        private readonly TestRequestRepository _testRepository =
            new TestRequestRepository();
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                ModelState.AddModelError("",
                    "دسترسی به آدرس مورد نظر غیر مجاز است لطفا وارد شوید");
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Corporation =
                new SelectList(_testRepository.GetAllCorporation(),
                    "Com_Id", "Com_Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel user, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Corporation =
                    new SelectList(_testRepository.GetAllCorporation(),
                        "Com_Id", "Com_Title");
                return View(user);
            }
            //if (!this.IsCaptchaValid("جواب تصویر صحیح نمیباشد"))
            //    return View(user);

            if (_userRepository.ValidateUser(user.UserName,
                user.Password,
                int.Parse(user.Corporation)))
            {
                var cookie = new HttpCookie("LabRequest-Cookie",
                    CookieEncryptor.Encrypt(user.Corporation));
                Response.Cookies.Add(cookie);
                
                if (_userRepository.GetUser(user.UserName,
                    int.Parse(CookieEncryptor.Decrypt(Request.Cookies["LabRequest-Cookie"].Value)))
                    .UserType == 0)
                {
                    ModelState.AddModelError("", "نام کاربری شما مجوز استفاده از سامانه را ندارد");
                    ViewBag.Corporation =
                        new SelectList(_testRepository.GetAllCorporation(),
                            "Com_Id", "Com_Title");
                    return View(user);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return ReturnToLocal(returnUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "نام یا رمز کاربری و یا شرکت مورد انتخاب اشتباه است");
                ViewBag.Corporation =
                    new SelectList(_testRepository.GetAllCorporation(),
                        "Com_Id", "Com_Title");
                return View(user);
            }
        }


        public ActionResult ReturnToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("TestRequestList", "TestRequest");
        }



        [Authorize]
        public ActionResult Logout()
        {
            if (Request.Cookies.AllKeys.Contains("LabRequest-Cookie"))
            {
                var cookie = Request.Cookies["LabRequest-Cookie"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
