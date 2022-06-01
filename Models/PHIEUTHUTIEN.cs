namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHIEUTHUTIEN")]
    public partial class PHIEUTHUTIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEUTHUTIEN()
        {
            DONGCHIPHIs = new HashSet<DONGCHIPHI>();
        }

        [Key]
        [StringLength(5)]
        public string MaPhieu { get; set; }

        [Required]
        [StringLength(6)]
        public string MaTre { get; set; }

        public DateTime NgayLapPhieu { get; set; }

        public bool TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONGCHIPHI> DONGCHIPHIs { get; set; }

        public virtual TRE TRE { get; set; }
    }
}
