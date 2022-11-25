using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyKhoaTu.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<DangKyKhoaTu> DangKyKhoaTus { get; set; }
        public virtual DbSet<DiemDanh> DiemDanhs { get; set; }
        public virtual DbSet<DongPhiDangKy> DongPhiDangKies { get; set; }
        public virtual DbSet<GiaDinh> GiaDinhs { get; set; }
        public virtual DbSet<GiaoTho> GiaoThoes { get; set; }
        public virtual DbSet<GuiDoDung> GuiDoDungs { get; set; }
        public virtual DbSet<KhoaTu> KhoaTus { get; set; }
        public virtual DbSet<LoaiKhoaTu> LoaiKhoaTus { get; set; }
        public virtual DbSet<MuonAoTrang> MuonAoTrangs { get; set; }
        public virtual DbSet<QuyPhungSu> QuyPhungSus { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ThanhVien> ThanhViens { get; set; }
        public virtual DbSet<ThuChi> ThuChis { get; set; }
        public virtual DbSet<TuSinh> TuSinhs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<XepGiaDinh> XepGiaDinhs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiaDinh>()
                .HasMany(e => e.XepGiaDinhs)
                .WithRequired(e => e.GiaDinh)
                .HasForeignKey(e => e.IdGiaDinh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiaoTho>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.DangKyKhoaTus)
                .WithRequired(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.DiemDanhs)
                .WithOptional(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.DongPhiDangKies)
                .WithOptional(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.GiaDinhs)
                .WithOptional(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.GuiDoDungs)
                .WithRequired(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.MuonAoTrangs)
                .WithRequired(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhoaTu>()
                .HasMany(e => e.XepGiaDinhs)
                .WithRequired(e => e.KhoaTu)
                .HasForeignKey(e => e.IdKhoaTu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoaiKhoaTu>()
                .HasMany(e => e.KhoaTus)
                .WithOptional(e => e.LoaiKhoaTu)
                .HasForeignKey(e => e.IdLoaiKhoaTu);

            modelBuilder.Entity<QuyPhungSu>()
                .HasMany(e => e.ThuChis)
                .WithOptional(e => e.QuyPhungSu)
                .HasForeignKey(e => e.IdQuy);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<ThuChi>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<TuSinh>()
                .Property(e => e.CMND)
                .IsUnicode(false);

            modelBuilder.Entity<TuSinh>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<TuSinh>()
                .Property(e => e.SDT_nguoithan)
                .IsUnicode(false);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.DangKyKhoaTus)
                .WithRequired(e => e.TuSinh)
                .HasForeignKey(e => e.IdTuSinh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.DangKyKhoaTus1)
                .WithOptional(e => e.TuSinh1)
                .HasForeignKey(e => e.DiCung);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.DiemDanhs)
                .WithOptional(e => e.TuSinh)
                .HasForeignKey(e => e.IdTuSinh);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.DongPhiDangKies)
                .WithOptional(e => e.TuSinh)
                .HasForeignKey(e => e.IdTuSinh);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.GuiDoDungs)
                .WithRequired(e => e.TuSinh)
                .HasForeignKey(e => e.idTuSinh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.MuonAoTrangs)
                .WithRequired(e => e.TuSinh)
                .HasForeignKey(e => e.IdTuSinh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TuSinh>()
                .HasMany(e => e.XepGiaDinhs)
                .WithRequired(e => e.TuSinh)
                .HasForeignKey(e => e.IdTuSinh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DiemDanhs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ThuChis)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TuSinhs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IdUser);
        }
    }
}
