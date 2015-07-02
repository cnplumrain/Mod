using Mod.DAL;
using Mod.IBLL.Member;
using Mod.Models.Member;

namespace Mod.BLL.Member
{
    public class PrivilegeService:BaseService<Privilege>,INterfacePrivilegeService
    {
        public PrivilegeService() : base(RepositoryFactory.PrivilegeRepository)
        {
        }
    }
}
