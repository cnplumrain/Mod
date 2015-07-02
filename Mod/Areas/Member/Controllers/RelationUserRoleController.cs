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
    public class RelationUserRoleController : BaseController
    {
        //
        // GET: /Member/RelationUserRole/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RelationDatas()
        {
            var users = new List<User>();
          
            var tmp = ServiceFactory.UserService.FindList(m => true);
            if (tmp.Any())
            {
                users.AddRange(tmp);
            }
            return View(users);
        }

        public ActionResult RelateUserRole()
        {
            var users = new List<User>();
            IQueryable<Role> roles;
            
            var tmp = ServiceFactory.UserService.FindList(m => true);
            if (tmp.Any())
            {
                users.AddRange(tmp);
            }
            roles = ServiceFactory.RoleService.FindList(m => true);
           
            ViewData["Roles"] = roles.ToList();
            return View(users);
        }
        [HttpPost]
        public string RelateUserRole(UserRoleViewModel data)
        {
            var user =
                ServiceFactory.UserService.Find(m => m.Id == data.UserId);
            var role = ServiceFactory.RoleService.Find(m => m.Id == data.RoleId);
            user.Roles.Add(role);
            ServiceFactory.UserService.Update(user);
            return "成功!";
        }
        public string DeleteRelation(int userId,IEnumerable<string> roles)
        {
            var user = ServiceFactory.UserService.Find(m => m.Id == userId);
            foreach (var item in roles)
            {
                var id = int.Parse(item);
                
                var role = ServiceFactory.RoleService.Find(m => m.Id == id);
                user.Roles.Remove(role);
            }
            ServiceFactory.UserService.Update(user); 
            return "成功!";
        }
	}
}