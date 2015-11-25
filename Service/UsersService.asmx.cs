using Models;
using Service.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ScriptService allows request from client-side
    [System.Web.Script.Services.ScriptService]
    public class UsersService : System.Web.Services.WebService
    {
        [WebMethod]
        public User GetUser(int id)
        {
            var user = UsersCollection.GetUsers().FirstOrDefault(u => u.Id == id);

            return user;
        }
    }
}