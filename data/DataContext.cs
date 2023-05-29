using Microsoft.EntityFrameworkCore;
namespace Project.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Project.Models.User> Users { get; set; }
        public DbSet<Project.Models.UserDetail> User_Detail { get; set; }
        public DbSet<Project.Models.UserList> User_List { get; set; }
        public DbSet<Project.Models.ListPosition> ListPosition { get; set; }
        public DbSet<Project.Models.ListTitle> ListTitle { get; set; }
        public DbSet<Project.Models.ListDepartment> ListDepartment { get; set; }
        public DbSet<Project.Models.Articles> Articles { get; set; }
        public DbSet<Project.Models.Blogs> Blogs { get; set; }
        public DbSet<Project.Models.Menu> Menu { get; set; }
        public DbSet<Project.Models.Hastag> Hastag { get; set; }
        public DbSet<Project.Models.Role> Role { get; set; }
        public DbSet<Project.Models.Rule> Rule { get; set; }
        public DbSet<Project.Models.Article_Menu> Article_Menu { get; set; }
        public DbSet<Project.Models.Role_Menu> Role_Menu { get; set; }
        public DbSet<Project.Models.Upload_Files_Mart> Upload_Files_Mart { get; set; }
        public DbSet<Project.Models.Upload_Files_Warehouse> Upload_Files_Warehouse { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project.Models.User>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.UserDetail>();
            modelBuilder.Entity<Project.Models.UserList>().HasKey(m => new { m.USERID, m.TABLELIST, m.LISTCODE }); ;
            modelBuilder.Entity<Project.Models.ListPosition>();
            modelBuilder.Entity<Project.Models.ListTitle>();
            modelBuilder.Entity<Project.Models.ListDepartment>();
            modelBuilder.Entity<Project.Models.Articles>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Blogs>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Menu>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Hastag>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Role>();
            modelBuilder.Entity<Project.Models.Rule>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Article_Menu>().HasKey(m => new { m.ARTICLEID, m.MENUID });
            modelBuilder.Entity<Project.Models.Role_Menu>();
            modelBuilder.Entity<Project.Models.Upload_Files_Mart>().Property(f => f.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project.Models.Upload_Files_Warehouse>().Property(f => f.ID).ValueGeneratedOnAdd();
        }


    }
}