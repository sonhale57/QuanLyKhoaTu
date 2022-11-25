namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiaDinh")]
    public partial class GiaDinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiaDinh()
        {
            XepGiaDinhs = new HashSet<XepGiaDinh>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        public int? IdKhoaTu { get; set; }

        public virtual KhoaTu KhoaTu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<XepGiaDinh> XepGiaDinhs { get; set; }
    }
}
