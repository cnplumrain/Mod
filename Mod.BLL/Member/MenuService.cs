using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mod.DAL;
using Mod.IBLL.Member;
using Mod.IDAL;
using Mod.Models.Member;

namespace Mod.BLL.Member
{
    public class MenuService:BaseService<Menu>,INterfaceMenuService
    {
        public MenuService() : base(RepositoryFactory.MenuRepository)
        {
        }
    }
}
