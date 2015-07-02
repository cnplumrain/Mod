using Mod.DAL;
using Mod.IBLL.Member;
using Mod.Models.Member;

namespace Mod.BLL.Member
{
    public class RoleService:BaseService<Role>,INterfaceRoleService
    {
        public RoleService() : base(RepositoryFactory.RoleRepository)
        {
        }
    }
}
