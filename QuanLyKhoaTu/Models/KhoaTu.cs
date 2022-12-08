namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhoaTu")]
    public partial class KhoaTu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoaTu()
        {
            DangKyKhoaTus = new HashSet<DangKyKhoaTu>();
            DiemDanhs = new HashSet<DiemDanh>();
            DongPhiDangKies = new HashSet<DongPhiDangKy>();
            GiaDinhs = new HashSet<GiaDinh>();
            GuiDoDungs = new HashSet<GuiDoDung>();
            MuonAoTrangs = new HashSet<MuonAoTrang>();
            XepGiaDinhs = new HashSet<XepGiaDinh>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        [Column(TypeName = "ntext")]
        public string Poster { get; set; }

        public DateTime? Ngaybatdau { get; set; }

        public DateTime? Ngayketthuc { get; set; }

        public int? IdLoaiKhoaTu { get; set; }

        public double? Chiphi { get; set; }

        public bool Active { get; set; }

        [StringLength(100)]
        public string DiaDiem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyKhoaTu> DangKyKhoaTus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DongPhiDangKy> DongPhiDangKies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiaDinh> GiaDinhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuiDoDung> GuiDoDungs { get; set; }

        public virtual LoaiKhoaTu LoaiKhoaTu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MuonAoTrang> MuonAoTrangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<XepGiaDinh> XepGiaDinhs { get; set; }
    }
}
