using System.Collections.Generic;
using LabRequest.DomainModel.Entities;

namespace LabRequest.DomainModel.Service
{
    public interface IUser
    {
        List<User> GetAllUsers();
        User GetUser(int id,int corp);
        User GetUser(string name,int corp);
        bool ValidateUser(string name, string password,int corp);
        bool PersonPermission(int userid, int formid);
    }
}