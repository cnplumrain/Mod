using Mod.BLL.Member;
using Mod.IBLL.Member;

namespace Mod.BLL
{
    public static class ServiceFactory
    {
        public static INterfaceUserService UserService { get { return  new UserService();} }
        public static INterfaceRoleService RoleService { get { return new RoleService(); } }
        public static INterfacePrivilegeService PrivilegeService { get { return new PrivilegeService(); } }
        public static INterfaceMenuService MenuService { get { return new MenuService(); } }
    }
}
