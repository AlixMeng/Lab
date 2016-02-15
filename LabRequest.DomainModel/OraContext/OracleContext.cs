using System;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace LabRequest.DomainModel.OraContext
{
    public class OracleContext : IDisposable
    {
        public OracleConnection OracleContextConnectionString
        {
            get
            {
                 var oracleConnectionString = 
                     WebConfigurationManager.AppSettings["OracleConnection"];
                return new OracleConnection(oracleConnectionString);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(OracleContextConnectionString);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
                OracleContextConnectionString.Close();
        }
    }
}