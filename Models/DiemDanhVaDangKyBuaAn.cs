namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiemDanhVaDangKyBuaAn")]
    public partial class DiemDanhVaDangKyBuaAn
    {
        [StringLength(5)]
        public string ID { get; set; }

        [Required]
        [StringLength(6)]
        public string MaTre { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay { get; set; }

        public bool DiemDanh { get; set; }

        public bool DangKiBuaAn { get; set; }

        public virtual TRE TRE { get; set; }
    }
}
