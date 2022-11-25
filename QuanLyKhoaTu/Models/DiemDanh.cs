namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiemDanh")]
    public partial class DiemDanh
    {
        public int id { get; set; }

        public int? IdKhoaTu { get; set; }

        public int? IdTuSinh { get; set; }

        public bool? DiemDanhCong { get; set; }

        public bool? DiemDanhXe { get; set; }

        public DateTime? Thoigian_CheckinCong { get; set; }

        public DateTime? Thoigian_CheckinXe { get; set; }

        public bool? DiemDanhTuTuc { get; set; }

        public DateTime? Thoigian_CheckinTuTuc { get; set; }

        public int? IdUser { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        public virtual TuSinh TuSinh { get; set; }

        public virtual User User { get; set; }
    }
}
