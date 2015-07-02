using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Mod.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Mod.BLL;
using Mod.Common;
using Mod.Models.Member;
using Mod.Web.Areas.Member.Models;
using Mod.Web.Controllers;
using Mod.Web.Models;
using Newtonsoft.Json;
using LoginViewModel = Mod.Web.Models.LoginViewModel;
using RegisterViewModel = Mod.Web.Areas.Member.Models.RegisterViewModel;

namespace Mod.Web.Areas.Member.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
       
        public UserController() {  }
        //
        // GET: /Member/User/
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(6);
            Bitmap img = Security.CreateVerificationImage(verificationCode, 160, 30);
            img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register)
        {
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != register.VerificationCode.ToUpper())
            {
                ModelState.AddModelError("VerificationCode", "验证码不正确");
                return View(register);
            }
            if (ModelState.IsValid)
            {


                if (ServiceFactory.UserService.Exist(register.UserName)) ModelState.AddModelError("UserName", "用户名已存在");
                else
                {
                  

                    var user = new User
                    {
                        UserName = register.UserName,
                        //默认用户组代码写这里
                        DisplayName = register.DisplayName,
                        Password = Security.Sha256(register.Password),
                        //邮箱验证与邮箱唯一性问题
                        Email = register.Email,
                        //用户状态问题
                        Status = 0,
                        RegistrationTime = System.DateTime.Now,
                        LoginTime = new DateTime(1900, 1, 1)
                    };
                 
                    user = ServiceFactory.UserService.Add(user);
                    if (user.Id > 0)
                    {
                        var identity = ServiceFactory.UserService.CreateIdentity(user,
                            DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(identity);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "注册失败！");
                }
            }
            else
            {
                return Content("模型状态不可用!请联系管理员");
            }
            return View(register);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="returnUrl">返回Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
           
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {  
            if (loginViewModel == null)
            {
                return Content("未获得输入信息!");
            }
            if (!ModelState.IsValid)
            {
                return Content("模型状态不可用!");
            }

            try
            {
                SqlConnection conn = SqlHelper.GetConnection();
                conn.Open();
                conn.Close();
            }
            catch (Exception e)
            {
                return Content("数据库连接不可用用" + e.Message);
            }
           
            try
            {
                var user = ServiceFactory.UserService.Find(loginViewModel.UserName);
               
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "用户名不存在");
                    return View(loginViewModel);
                }
                if (user.Password != Security.Sha256(loginViewModel.Password))
                {
                    ModelState.AddModelError("Password", "密码错误");
                    return View(loginViewModel);
                }
                
                user.LoginTime = System.DateTime.Now;
                user.LoginIp = Request.UserHostAddress;
                ServiceFactory.UserService.Update(user);
                SysUser = user;
             
                var identity = ServiceFactory.UserService.CreateIdentity(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
               
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(
                    new AuthenticationProperties {IsPersistent = loginViewModel.RememberMe}, identity);
              
                return RedirectToAction("Menu", "Home",new{area=""});
            }
            catch (Exception e)
            {
                return Content(e.Message + (e.InnerException == null ? "" : e.InnerException.Message));
            }

            //try
            //{
            //    var users =  ServiceFactory.UserService.Entities();
            //    if (users == null || !users.Any())
            //    {
            //        return Content("未能连接上数据库！");
            //    }
            //    var user =  ServiceFactory.UserService.Find(loginViewModel.UserName);
            //    if (user == null)
            //    {
            //        return Content("登录失败！用户名错误");
            //    }
            //    if (user.Password == Security.Sha256(loginViewModel.Password))
            //    {
            //        return Content("登录成功!");
            //    }
            //    return Content("密码错误!");
            //}
            //catch (Exception e)
            //{
            //    return Content("登录失败!" + e.Message);
            //}
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public string ForgetPassword(string userName)
        {
            var user = ServiceFactory.UserService.Find(m => m.UserName == userName);
            var email = user.Email;
            const string pattern = "/(.{2}).+(.{2}@.+)/g";
            var fuzzyEmail = Regex.Replace(email,pattern,"***");
            EmailHelper.SendEmailDefault(email, user.Id.ToString(CultureInfo.InvariantCulture));
            return "已向您的邮箱"+fuzzyEmail+"发送确认信息！请登录邮箱查看";
        }

        public ActionResult ResetPassword(string userId)
        {
            ViewBag.userId = userId;

            return View();
        }
        [HttpPost]
        public string ResetPassword(string userId,string password)
        {
            var id = int.Parse(userId.Replace("01F",""));
            var user = ServiceFactory.UserService.Find(m => m.Id == id);
            user.Password = Security.Sha256(password);
            ServiceFactory.UserService.Update(user);
            return "重置成功";
        }
        public string GetNickName()
        {
            var userId = int.Parse(@User.Identity.GetUserId());
            var user = ServiceFactory.UserService.Find(m => m.Id == userId);
            string name = user.DisplayName;
            // var json = new {displayName = name};
            return name;
        }
        /// <summary>
        /// 显示资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            return View( ServiceFactory.UserService.Find(User.Identity.Name));
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Modify()
        {

            var user =  ServiceFactory.UserService.Find(User.Identity.Name);
            if (user == null) ModelState.AddModelError("", "用户不存在");
            else
            {
                if (TryUpdateModel(user, new[] { "DisplayName", "Email" }))
                {
                    if (ModelState.IsValid)
                    {
                        if ( ServiceFactory.UserService.Update(user)) ModelState.AddModelError("", "修改成功！");
                        else ModelState.AddModelError("", "无需要修改的资料");
                    }
                }
                else ModelState.AddModelError("", "更新模型数据失败");
            }
            return View("Details", user);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel passwordViewModel)
        {
           // AuthenticationManager.User.Claims.First(u => u.ValueType == "");
            if (ModelState.IsValid)
            {
                var user =  ServiceFactory.UserService.Find(User.Identity.Name);
                if (user.Password == Security.Sha256(passwordViewModel.OriginalPassword))
                {
                    user.Password = Security.Sha256(passwordViewModel.Password);
                    if ( ServiceFactory.UserService.Update(user)) ModelState.AddModelError("", "修改密码成功");
                    else ModelState.AddModelError("", "修改密码失败");
                }
                else ModelState.AddModelError("", "原密码错误");
            }
            return View(passwordViewModel);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect(Url.Action("Login","User",new{area="Member"}));
        }

        /// <summary>
        /// 用户导航栏
        /// </summary>
        /// <returns>分部视图</returns>
        public ActionResult Menu()
        {
            return PartialView();
        }

        #region 属性
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion


        public string GetAllUsers()
        {
            var users = ServiceFactory.UserService.FindList(m => true).Select(n=>new
            {
                Id = n.Id,
                Name = n.DisplayName
            }).ToList();
            var jsonUsers = JsonConvert.SerializeObject(users); ; 
            return jsonUsers;
        }

        public ActionResult UserDatas()
        {
            var users =new  List<User>();
            IQueryable<Role> roles;
            try
            {
                var tmp = ServiceFactory.UserService.FindList(m => true);
                if (tmp.Any())
                {
                    users.AddRange(tmp);
                }
                roles = ServiceFactory.RoleService.FindList(m => true);
            }
            catch (Exception e)
            {
                return Content("<script>alert('出错:"+e.Message+"')</script>");
            }
            ViewData["Roles"] = roles.ToList();
            return View(users);
        }

      
	}
}