using System.Linq;
using System.Text;
using System.Web.Mvc;
using Mod.BLL;
using Mod.Models.Member;
using WebGrease.Css.Extensions;

namespace Mod.Web.Controllers
{
    public class BaseController : Controller
    {
        protected static User SysUser = null;
        protected  string ControllerName = "";
        protected  string ActionName = "";

        public  BaseController()
        {
            //if (@User!=null)
            //{
            //    var userId = int.Parse(@User.Identity.GetUserId());
            //    SysUser = ServiceFactory.UserService.Find(m => m.Id == userId);
            //}
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            

            //var space = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
            //var spaceArr = space.Split('.');
           // var area = spaceArr[spaceArr.Length-2];
            var area =  filterContext.Controller.ControllerContext.RouteData.DataTokens["area"].ToString();
            ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            ActionName = filterContext.ActionDescriptor.ActionName;
            var requestMethod = filterContext.HttpContext.Request.RequestType;
            var paramNums = filterContext.ActionParameters.Count();
            
            var privilege = ServiceFactory.PrivilegeService.Find(
                m =>
                   m.AreaName==area && m.ControllerName == ControllerName && m.ActionName == ActionName && m.RequestMethod == requestMethod && m.ParamNums==paramNums);
            if (privilege==null)
            {
                 privilege = new Privilege
                {
                    AreaName = area,
                    ControllerName = ControllerName,
                    ActionName = ActionName,
                    ParamNums = paramNums,
                    RequestMethod = requestMethod
                };
                ServiceFactory.PrivilegeService.Add(privilege);
            }


            if (ActionName == "Register" || ActionName == "Login" || ActionName == "VerificationCode" || ActionName == "ForgetPassword" || ActionName == "ResetPassword")
            {
                return;
            }
            if (privilege.IsCommon && SysUser!=null)
            {
                return;
            }
            if (SysUser != null && SysUser.DisplayName == "超级管理员")
            {
                return;
            }
            bool isHavPrivilege=false;
            if (SysUser != null)
            {
                var privileges = SysUser.Roles.SelectMany(m=>m.Privileges).ToList();
                isHavPrivilege = privileges.Contains(privilege);
            }
            if (!isHavPrivilege)
            {
                filterContext.Result = new ContentResult {Content = "<script>alert('无权访问!("+ActionName+")')</script>",ContentEncoding = Encoding.UTF8};
                //filterContext.Result = new JsonResult
                //{
                //    Data = "无权访问",
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var errorMsg = filterContext.Exception.Message;
            //var values = filterContext.Controller.ControllerContext.RouteData.Values;
            //var controllerName = values["controller"];
            //var actionName = values["action"];
            var relsultMsg = errorMsg + "(" + ControllerName + ":" + ActionName + ")";
            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
                filterContext.Result = new JsonResult()
                {
                    Data = relsultMsg,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.ExceptionHandled = true;
            //}
           
        }


        
	}
}