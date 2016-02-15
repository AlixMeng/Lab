using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LabRequest.Web.Controllers;
using LabRequest.Web.Infrastracture;

namespace LabRequest.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterFilters.RegisterGlobalFilters(GlobalFilters.Filters);
            RoutingSystem.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //var exception = Server.GetLastError();
            //if (exception == null)
            //    return;
            ////var mail = new MailMessage { From = new MailAddress("automated@contoso.com") };
            ////mail.To.Add(new MailAddress("administrator@contoso.com"));
            ////mail.Subject = "Site Error at " + DateTime.Now;
            ////mail.Body = "Error Description: " + exception.Message;
            ////var server = new SmtpClient { Host = "your.smtp.server" };
            ////server.Send(mail);

            //// Clear the error
            //Server.ClearError();

            //// Redirect to a landing page
            //Response.Redirect("~/Error/NotFound");
        }
    }
}