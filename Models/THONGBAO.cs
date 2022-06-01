namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THONGBAO")]
    public partial class THONGBAO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THONGBAO()
        {
            THONGBAO_TK = new HashSet<THONGBAO_TK>();
        }

        [Key]
        [StringLength(5)]
        public string MaTB { get; set; }

        [StringLength(200)]
        public string TenTB { get; set; }

        [StringLength(500)]
        public string NoiDung { get; set; }

        public DateTime NgayTB { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONGBAO_TK> THONGBAO_TK { get; set; }
    }
}
