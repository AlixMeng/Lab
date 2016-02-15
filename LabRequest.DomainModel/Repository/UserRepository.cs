using System.Collections.Generic;
using System.Linq;
using Dapper;
using LabRequest.DomainModel.OraContext;
using LabRequest.DomainModel.Entities;
using LabRequest.DomainModel.Service;
using Microsoft.CSharp;
using Org.BouncyCastle.Asn1.X509;

namespace LabRequest.DomainModel.Repository
{
    public class UserRepository : IUser
    {
        private readonly OracleContext _context = new OracleContext();
        public List<User> GetAllUsers()
        {
            return _context
                .OracleContextConnectionString
                .Query<User>(@"SELECT * FROM CENTRAL_LAB.TBLPERSON")
                .ToList();
        }

        public User GetUser(int id, int corp)
        {
            return _context
                .OracleContextConnectionString
                .Query<User>(@"SELECT * FROM CENTRAL_LAB.TBLPERSON")
                .Single(x => x.PersonId.Equals(id) && x.Com_Id.Equals(corp));
        }

        public User GetUser(string name, int corp)
        {
            return _context
                .OracleContextConnectionString
                .Query<User>(@"SELECT * FROM CENTRAL_LAB.TBLPERSON")
                .Single(x => x.UserName.Equals(name) && x.Com_Id.Equals(corp));
        }

        public bool ValidateUser(string name, string password, int corp)
        {
            return _context
                .OracleContextConnectionString
                .Query<User>(@"SELECT * FROM CENTRAL_LAB.TBLPERSON")
                .Any(x => x.UserName.Equals(name) &&
                    x.PersonPass.Equals(password) &&
                    x.Com_Id.Equals(corp));
        }

        public bool PersonPermission(int userid, int formid)
        {
            return _context.OracleContextConnectionString
                .Query<bool>(@"SELECT DISTINCT PERSONFAMILY,PERENGLISHNAME 
                  FROM TBLPERMISSION,TBLPERSONPERM,
                  TBLPERSON,TBLPERSONUNIT 
                  WHERE TBLPERSONPERM.PERMISSIONID = TBLPERMISSION.PERMISSIONID 
                  AND TBLPERSONPERM.PERSONID = TBLPERSON.PERSONID 
                  TBLPERSONUNIT.PERSONID = TBLPERSON.PERSONID 
                  AND TBLPERMISSION.PERMISSIONID= :PERMISSIONID 
                  AND TBLPERSON.PERSONID = :PERSONID", new
                  {
                      PERMISSIONID = formid,
                      PERSONID = userid
                  }).Any();
        }
    }
}