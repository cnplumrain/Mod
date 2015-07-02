using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mod.BLL;
using Mod.Models;
using Mod.Models.Member;
using Mod.Web.Areas.Member.Models;
using Mod.Web.Controllers;

namespace Mod.Web.Areas.Member.Controllers
{
    public class RelationRolePrivilegeController : BaseController
    {
        //
        // GET: /Member/RelationRolePrivilege/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RelationDatas()
        {
            var users = new List<Role>();

            var tmp = ServiceFactory.RoleService.FindList(m => true);
            if (tmp.Any())
            {
                users.AddRange(tmp);
            }
            return View(users);
        }

        public ActionResult RelateRolePrivilege()
        {
            var roles = new List<Role>();
            IQueryable<Privilege> privileges;
            
            var tmp = ServiceFactory.RoleService.FindList(m => true);
            if (tmp.Any())
            {
                roles.AddRange(tmp);
            }
            privileges = ServiceFactory.PrivilegeService.FindList(m => true).OrderBy(m=>m.ControllerName).ThenBy(m=>m.ActionName);

            ViewData["Privileges"] = privileges.ToList();
            return View(roles);
        }

        [HttpPost]
        public string RelateRolePrivilege(RolePrivilegeViewModel data)
        {
            var role =
                ServiceFactory.RoleService.Find(m => m.Id == data.RoleId);
            var privilege = ServiceFactory.PrivilegeService.Find(m => m.Id == data.PrivilegeId);
            if (role.Privileges.Contains(privilege))
            {
                return "权限已存在";
            }
            role.Privileges.Add(privilege);
            ServiceFactory.RoleService.Update(role);
            return "成功!";
        }
        public string DeleteRelation(IEnumerable<RolePrivilegeViewModel> datas)
        {
            var roles = new List<Role>();
            foreach (var item in datas)
            {
                var data = item;
                var role =ServiceFactory.RoleService.Find(m => m.Id == data.RoleId);
                var privilege = ServiceFactory.PrivilegeService.Find(m => m.Id == data.PrivilegeId);
                role.Privileges.Remove(privilege);
                roles.Add(role);
            }
            ServiceFactory.RoleService.DeleteList(roles);
            return "成功!";
        }
	}
}