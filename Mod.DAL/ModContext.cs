using System.Data.Entity;
using Mod.Models.Member;

namespace Mod.DAL
{

    public partial class ModContext : DbContext
    {

        public ModContext()
            : base("Name=ModContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ModContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<daima_zpbm>().HasKey(t => new { t.zpbh, t.bmbm });
        //    modelBuilder.Entity<daima_zpgw>().HasKey(t => new { t.zpbh, t.zpbm, t.gwbm });
        //    modelBuilder.Entity<DaiMa>().HasKey(t => new { t.lb, t.bm });
           
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Menu> Menus { get; set; }        
    }
}


