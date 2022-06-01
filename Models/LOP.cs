namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOP")]
    public partial class LOP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOP()
        {
            PHANCONGGIAOVIENs = new HashSet<PHANCONGGIAOVIEN>();
            TREs = new HashSet<TRE>();
        }

        [Key]
        [StringLength(5)]
        public string MaLop { get; set; }

        [StringLength(50)]
        public string TenLop { get; set; }

        public int SiSo { get; set; }

        public int DoTuoi { get; set; }

        [StringLength(20)]
        public string Khoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHANCONGGIAOVIEN> PHANCONGGIAOVIENs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRE> TREs { get; set; }
    }
}
