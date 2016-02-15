using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace LabRequest.Web.Infrastracture
{
    public class AppPath
    {
        public static string ApplicationPath
        { get { return IsInWeb ? HttpRuntime.AppDomainAppPath : Application.StartupPath; } }

        private static bool IsInWeb
        { get { return HttpContext.Current != null; } }
    }
}