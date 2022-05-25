using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanUserAccessLayer.DTO;
using TalanUserAccessLayer.Models;

namespace TalanUserAccessLayer.BusinessLogic
{
    public interface IUsersBL
    {
        Task<User> LogIn(AuthenticationUser authenticationUser);
        Task<User> SignUp(CreationUser creationUser);  
    }
}
