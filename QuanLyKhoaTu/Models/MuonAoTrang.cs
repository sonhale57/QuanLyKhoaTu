namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MuonAoTrang")]
    public partial class MuonAoTrang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKhoaTu { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTuSinh { get; set; }

        public bool? Muon { get; set; }

        public DateTime? Thoigian_Muon { get; set; }

        public bool? Tra { get; set; }

        public DateTime? Thoigian_tra { get; set; }

        [StringLength(50)]
        public string SizeAo { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        public virtual TuSinh TuSinh { get; set; }
    }
}
