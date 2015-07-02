using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Mod.BLL;
using Mod.Models.Member;

namespace Mod.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error(string  mess)
        {
            ViewData["message"] = mess;
            return View();
        }

        /// <summary>
        /// 用户导航栏
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult MenuPrivilege()
        {
            var privileges = ServiceFactory.PrivilegeService.FindList(m => m.RequestMethod == "GET").ToList();
            return View(privileges);
          
        }

        public ActionResult Menu()
        {
            //var menu = new Menu
            //{
            //    Id = 1,
            //    Name = "主页",
            //    Url = "",
            //    Level = 1
            //};
            //ServiceFactory.MenuService.Add(menu);
            var menus = ServiceFactory.MenuService.FindList(m => m.Level == 1).Include(m=>m.Children).ToList();
            return View(menus);
        }

        public ActionResult AddMenu(Menu menu)
        {
            var menus = ServiceFactory.MenuService.FindList(m => true).ToList();
            ViewData["Menus"] = menus;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Developer()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}