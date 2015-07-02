using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mod.Models.Member
{
     public class Role
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{1}到{0}个字")]
        [Display(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        public string  Description { get; set; }
       
        public virtual IList<User> Users { get; set; }

        public virtual IList<Privilege> Privileges { get; set; }

    }
}
