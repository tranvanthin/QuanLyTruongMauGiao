namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRE")]
    public partial class TRE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRE()
        {
            DiemDanhVaDangKyBuaAns = new HashSet<DiemDanhVaDangKyBuaAn>();
            PHIEUDANHGIAs = new HashSet<PHIEUDANHGIA>();
            PHIEUTHUTIENs = new HashSet<PHIEUTHUTIEN>();
        }

        [Key]
        [StringLength(6)]
        public string MaTre { get; set; }

        [Required(ErrorMessage = "Không được để trống mã lớp")]
        [StringLength(5)]
        public string MaLop { get; set; }

        [Required(ErrorMessage = "Không được để trống mã phụ huynh")]
        [StringLength(5)]
        public string MaPH { get; set; }

        [StringLength(100)]
        public string TenTre { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgaySinh { get; set; }

        public bool GioiTinh { get; set; }

        [StringLength(50)]
        public string QueQuan { get; set; }

        [StringLength(30)]
        public string DanToc { get; set; }

        public DateTime NgayNhapHoc { get; set; }

        [StringLength(100)]
        public string Anh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanhVaDangKyBuaAn> DiemDanhVaDangKyBuaAns { get; set; }

        public virtual LOP LOP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUDANHGIA> PHIEUDANHGIAs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTHUTIEN> PHIEUTHUTIENs { get; set; }

        public virtual PHUHUYNH PHUHUYNH { get; set; }
    }
}
