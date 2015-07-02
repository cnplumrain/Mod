using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod.Models.Member
{
 
    public class Menu
    {
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("地址")]
        public string Url { get; set; }
        [DisplayName("级别")]
        public int Level { get; set; }

        //[DisplayName("上级内码")]
        //public int? ParentId { get; set; }

        //[ForeignKey("ParentId")]
        public virtual Menu Parent { get; set; }

        
        public ICollection<Menu> Children { get; set; } 
    }
}
