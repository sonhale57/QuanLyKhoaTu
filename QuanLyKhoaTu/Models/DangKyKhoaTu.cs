namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyKhoaTu")]
    public partial class DangKyKhoaTu
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKhoaTu { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTuSinh { get; set; }

        public DateTime? NgayGhiDanh { get; set; }

        [StringLength(100)]
        public string DiChuyen { get; set; }

        public int? Trangthai { get; set; }

        public int? DiCung { get; set; }

        public bool? Checkin { get; set; }

        public int? TypeCheckin { get; set; }
        public DateTime? TimeCheckin { get; set; }
        public bool MuonAoTrang { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        public virtual TuSinh TuSinh { get; set; }

        public virtual TuSinh TuSinh1 { get; set; }
    }
}
