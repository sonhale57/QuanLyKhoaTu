namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThuChi")]
    public partial class ThuChi
    {
        public int id { get; set; }

        public bool? Loai { get; set; }

        [StringLength(250)]
        public string LyDo { get; set; }

        public double? Sotien { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public int? IdUser { get; set; }

        [StringLength(100)]
        public string NguoiNhan { get; set; }

        [StringLength(20)]
        public string SDT { get; set; }

        public bool? TrangThai { get; set; }

        public int? IdQuy { get; set; }

        public virtual QuyPhungSu QuyPhungSu { get; set; }

        public virtual User User { get; set; }
    }
}
