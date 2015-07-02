using System.Collections.Generic;
using System.Web.Mvc;
using Mod.BLL;
using Mod.Models.Member;
using Mod.Web.Controllers;

namespace Mod.Web.Areas.Member.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Member/Role/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RoleDatas()
        {
            var roles = ServiceFactory.RoleService.FindList(m => true);
            return View(roles);
        }

        public string SaveDatas(IEnumerable<Role> datas)
        {
            var roles = datas;
            foreach (var role in roles)
            {
                if (role.Id != 0)
                {
                    ServiceFactory.RoleService.Update(role);
                }
                else
                {
                    ServiceFactory.RoleService.Add(role);
                }
            }
            return "成功!";
        }
	}
}