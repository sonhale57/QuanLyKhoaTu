namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiaoTho")]
    public partial class GiaoTho
    {
        public int id { get; set; }

        [StringLength(100)]
        public string Hoten { get; set; }

        [StringLength(100)]
        public string PhapDanh { get; set; }

        [StringLength(20)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string TuVien { get; set; }

        [StringLength(200)]
        public string LinkFb { get; set; }

        public DateTime? Namsinh { get; set; }

        [StringLength(500)]
        public string Hinhanh { get; set; }
    }
}
