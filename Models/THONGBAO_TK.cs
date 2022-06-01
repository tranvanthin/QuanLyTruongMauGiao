namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class THONGBAO_TK
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string MaTB { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string TenTK { get; set; }

        public bool DaXem { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }

        public virtual THONGBAO THONGBAO { get; set; }
    }
}
