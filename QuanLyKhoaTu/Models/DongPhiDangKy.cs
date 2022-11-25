namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DongPhiDangKy")]
    public partial class DongPhiDangKy
    {
        public int id { get; set; }

        public int? IdTuSinh { get; set; }

        public int? IdKhoaTu { get; set; }

        public double? Sotien { get; set; }

        public DateTime? ThoiGianCapNhat { get; set; }

        public int? IdUser { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        public virtual TuSinh TuSinh { get; set; }
    }
}
