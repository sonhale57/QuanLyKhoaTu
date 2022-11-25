namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiKhoaTu")]
    public partial class LoaiKhoaTu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiKhoaTu()
        {
            KhoaTus = new HashSet<KhoaTu>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        public DateTime? Updatetime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhoaTu> KhoaTus { get; set; }
    }
}
