using System.Web.Mvc;

namespace LabRequest.Web.Controllers
{
   
    public class LayoutController : Controller
    {
        [ChildActionOnly]
        [Authorize]
        public ActionResult Sidebar()
        {
            return View();
        }

    }
}
