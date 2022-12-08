namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TuSinh")]
    public partial class TuSinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TuSinh()
        {
            DangKyKhoaTus = new HashSet<DangKyKhoaTu>();
            DangKyKhoaTus1 = new HashSet<DangKyKhoaTu>();
            DiemDanhs = new HashSet<DiemDanh>();
            DongPhiDangKies = new HashSet<DongPhiDangKy>();
            GuiDoDungs = new HashSet<GuiDoDung>();
            MuonAoTrangs = new HashSet<MuonAoTrang>();
            XepGiaDinhs = new HashSet<XepGiaDinh>();
        }

        public int id { get; set; }

        [StringLength(500)]
        public string Hinhanh { get; set; }

        [StringLength(100)]
        public string Hoten { get; set; }

        public DateTime? Namsinh { get; set; }

        [StringLength(100)]
        public string Phapdanh { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string CMND { get; set; }

        [StringLength(20)]
        public string Gioitinh { get; set; }

        [StringLength(20)]
        public string SDT { get; set; }

        [StringLength(20)]
        public string SDT_nguoithan { get; set; }

        [StringLength(2000)]
        public string LinkFB { get; set; }

        public int? IdUser { get; set; }

        public DateTime? Updatetime { get; set; }

        [StringLength(500)]
        public string QRCode { get; set; }

        [StringLength(100)]
        public string DiaChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyKhoaTu> DangKyKhoaTus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyKhoaTu> DangKyKhoaTus1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DongPhiDangKy> DongPhiDangKies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuiDoDung> GuiDoDungs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MuonAoTrang> MuonAoTrangs { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<XepGiaDinh> XepGiaDinhs { get; set; }
    }
}
