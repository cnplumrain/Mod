using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mod.BLL;
using Mod.Models.Member;
using Mod.Web.Controllers;

namespace Mod.Web.Areas.Member.Controllers
{
    public class PrivilegeController : BaseController
    {
        //
        // GET: /Member/Privilege/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrivilegeDatas()
        {
           
            var privileges = ServiceFactory.PrivilegeService.FindList(m => true).OrderBy(m => m.ControllerName).ThenBy(m => m.ActionName);
            return View(privileges);
        }
        public string SaveDatas(IEnumerable<Privilege> datas)
        {
            var updateList = new List<Privilege>();
            var addList = new List<Privilege>();
            foreach (var item in datas)
            {
                if (item.Id != 0)
                {
                    var privilege = ServiceFactory.PrivilegeService.Find(m => m.Id == item.Id);
                    if (privilege == null)
                    {
                        throw new Exception("异常,请联系管理员!");
                    }

                    privilege.Description = item.Description;
                    privilege.Name = item.Name;
                    updateList.Add(privilege);

                }
                else
                {
                    addList.Add(item);
                }
            }
            ServiceFactory.PrivilegeService.UpdateList(updateList);
            ServiceFactory.PrivilegeService.AddList(addList);
            return "成功!";
        }
	}
}