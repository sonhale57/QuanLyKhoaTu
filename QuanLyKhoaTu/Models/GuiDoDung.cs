namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GuiDoDung")]
    public partial class GuiDoDung
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKhoaTu { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idTuSinh { get; set; }

        [Column(TypeName = "ntext")]
        public string Vatdung { get; set; }

        [StringLength(500)]
        public string Hinhanh { get; set; }

        public int? GhiChu { get; set; }

        public bool? Gui { get; set; }

        public DateTime? Thoigian_Gui { get; set; }

        public bool? Tra { get; set; }

        public DateTime? Thoigian_Tra { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        public virtual TuSinh TuSinh { get; set; }
    }
}
