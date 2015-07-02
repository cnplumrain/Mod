using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Mod.DAL;
using Mod.IBLL.Member;
using Mod.Models.Member;

namespace Mod.BLL.Member
{
    public class UserService : BaseService<User>, INterfaceUserService
    {
        public UserService() : base(RepositoryFactory.UserRepository) { }

        public ClaimsIdentity CreateIdentity(User user, string authenticationType)
        {
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
            return identity;
        }

        public bool Exist(string userName) { return CurrentRepository.Exist(u => u.UserName == userName); }

        public User Find(int userId) { return CurrentRepository.Find(u => u.Id == userId); }

        public User Find(string userName) { return CurrentRepository.Find(u => u.UserName == userName); }

        public IQueryable<User> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order)
        {
            bool isAsc = true;
            string orderName;
            switch (order)
            {
                case 0:
                    isAsc = true;
                    orderName = "Id";
                    break;
                case 1:
                    isAsc = false;
                    orderName = "Id";
                    break;
                case 2:
                    isAsc = true;
                    orderName = "RegistrationTime";
                    break;
                case 3:
                    isAsc = false;
                    orderName = "RegistrationTime";
                    break;
                case 4:
                    isAsc = true;
                    orderName = "LoginTime";
                    break;
                case 5: isAsc = false;
                    orderName = "LoginTime";
                    break;
                default:
                    isAsc = false;
                    orderName = "Id";
                    break;
            }
            return CurrentRepository.FindPageList<User>(pageIndex, pageSize, out totalRecord, u => true, orderName, isAsc);


        }
    }

}
