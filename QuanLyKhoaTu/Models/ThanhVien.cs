namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThanhVien")]
    public partial class ThanhVien
    {
        public int id { get; set; }

        [StringLength(100)]
        public string Hoten { get; set; }

        [StringLength(100)]
        public string PhapDanh { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; }

        [StringLength(20)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? Namsinh { get; set; }

        [StringLength(100)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string vitri { get; set; }
    }
}
