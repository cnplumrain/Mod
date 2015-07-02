using Mod.DAL.Member;
using Mod.IDAL.Member;

namespace Mod.DAL
{
    public static class RepositoryFactory
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        public static INterfaceUserRepository UserRepository { get { return new UserRepository(); } }
        public static INterfaceRoleRepository RoleRepository { get { return new RoleRepository(); } }
        public static INterfacePrivilegeRepository PrivilegeRepository { get { return new PrivilegeRepository();} }
        public static INterfaceMenuRepository MenuRepository { get { return new MenuRepository(); } }
       
    }

   
}
