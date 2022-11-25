namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuyPhungSu")]
    public partial class QuyPhungSu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuyPhungSu()
        {
            ThuChis = new HashSet<ThuChi>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string LoaiQuy { get; set; }

        public double? SoDu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThuChi> ThuChis { get; set; }
    }
}
