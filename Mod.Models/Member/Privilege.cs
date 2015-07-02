using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mod.Models.Member
{
    public class Privilege
    {
        public int Id { get; set; }
        [Display(Name="权限名称")]
        public string Name { get; set; }

        [Display(Name = "方法名称")]
        public string ActionName { get; set; }

        [Display(Name = "控制器名称")]
        public string ControllerName { get; set; }
        [Display(Name = "路由区域名称")]
        public string AreaName { get; set; }

        [Display(Name = "参数个数")]
        public int ParamNums { get; set; }

        /// <summary>
        /// Get or Post
        /// </summary>
        [Display(Name = "请求类型")]
        public string RequestMethod { get; set; }

        [Display(Name = "是否是公共权限")]
        public bool IsCommon { get; set; }

        [Display(Name = "权限描述")]
        public string Description { get; set; }

        public virtual IList<Role> Roles { get; set; }
    }
}
