using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace QuanLyKhoaTu.Models
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options) { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ZenCourse> ZenCourses { get; set; }
        public DbSet<JoinCourse> JoinCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập khóa chính
            modelBuilder.Entity<Area>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Bed>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Member>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<ZenCourse>()
                .HasKey(z => z.Id);

            modelBuilder.Entity<JoinCourse>()
                .HasKey(j => new { j.MemberId, j.CourseId }); // Khóa chính kép

            // Thiết lập khóa ngoại
            modelBuilder.Entity<Bed>()
                .HasOne(b => b.Area)
                .WithMany(a => a.Beds)
                .HasForeignKey(b => b.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.User)
                .WithMany(u => u.Members)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZenCourse>()
                .HasOne(z => z.User)
                .WithMany(u => u.ZenCourses)
                .HasForeignKey(z => z.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JoinCourse>()
                .HasOne(j => j.Member)
                .WithMany(m => m.JoinCourses)
                .HasForeignKey(j => j.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JoinCourse>()
                .HasOne(j => j.ZenCourse)
                .WithMany(z => z.JoinCourses)
                .HasForeignKey(j => j.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JoinCourse>()
                .HasOne(j => j.User)
                .WithMany(u => u.JoinCourses)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    [Table("Area")]
    public class Area
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Bed> Beds { get; } = new List<Bed>();
    }

    [Table("Bed")]
    public class Bed
    {
        [Key]
        public int Id { get; set; }
        public int? AreaId { get; set; }
        public int? RowNumber { get; set; }
        public int? BedNumber { get; set; }
        public string? Name { get; set; }
        public bool? IsAvailable { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; }
    }

    [Table("JoinCourse")]
    public class JoinCourse
    {
        [Key, Column(Order = 0)]
        public int MemberId { get; set; }
        [Key, Column(Order = 1)]
        public int CourseId { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public bool? StatusJoin { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? BedId { get; set; }
        public bool? ReceivePhone { get; set; } = true;
        public bool? ReceiveCCCD { get; set; } = true;

        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        [ForeignKey("CourseId")]
        public ZenCourse ZenCourse { get; set; }

        public User User { get; }
    }

    [Table("Member")]
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? OrtherName { get; set; }
        public DateTime? BirthDay { get; set; }= DateTime.Now;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? OrtherPhone { get; set; }
        public string? ImageIdentity { get; set; }
        public string? NumberIdentity { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserId { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? DateUpdate { get; set; }
        public bool? Enable { get; set; }
        public int? PrintCount { get; set; }

        [ForeignKey("UserId")]
        public User User { get; }
        public ICollection<JoinCourse> JoinCourses { get; } = new List<JoinCourse>();
    }

    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? OrtherName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? Enable { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool? AdminRole { get; set; }

        public ICollection<Member> Members { get; } = new List<Member>(); // Thêm danh sách Members
        public ICollection<ZenCourse> ZenCourses { get; } = new List<ZenCourse>(); // Thêm danh sách ZenCourses
        public ICollection<JoinCourse> JoinCourses { get; } = new List<JoinCourse>(); // Thêm danh sách JoinCourses

    }

    [Table("ZenCourse")]
    public class ZenCourse
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateCreate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; }
        public ICollection<JoinCourse> JoinCourses { get; } = new List<JoinCourse>(); // Thêm danh sách JoinCourses
    }
}
