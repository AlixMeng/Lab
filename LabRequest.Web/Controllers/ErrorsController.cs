using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabRequest.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult NotFound(string source)
        {
            return View();
        }


        public ActionResult Error(string source)
        {
            return View();
        }

    }
}
