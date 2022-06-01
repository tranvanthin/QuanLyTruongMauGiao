namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NOIDUNGDANHGIA")]
    public partial class NOIDUNGDANHGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NOIDUNGDANHGIA()
        {
            KETQUADANHGIAs = new HashSet<KETQUADANHGIA>();
        }

        [Key]
        [StringLength(5)]
        public string MaNDDG { get; set; }

        [Column("NoiDungDanhGia")]
        [Required]
        [StringLength(300)]
        public string NoiDungDanhGia1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KETQUADANHGIA> KETQUADANHGIAs { get; set; }
    }
}
