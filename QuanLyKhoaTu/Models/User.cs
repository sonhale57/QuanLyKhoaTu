namespace QuanLyKhoaTu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            DiemDanhs = new HashSet<DiemDanh>();
            ThuChis = new HashSet<ThuChi>();
            TuSinhs = new HashSet<TuSinh>();
        }

        public int id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(500)]
        public string OtherName { get; set; }

        [Column(TypeName = "ntext")]
        public string Image { get; set; }

        [StringLength(500)]
        public string Username { get; set; }
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Password { get; set; }

        public bool? Enable { get; set; }

        [StringLength(50)]
        public string Permission { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? DateofBirth { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThuChi> ThuChis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TuSinh> TuSinhs { get; set; }
    }
}
