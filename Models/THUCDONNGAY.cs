namespace QuanLyTruongMauGiao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THUCDONNGAY")]
    public partial class THUCDONNGAY
    {
        [Key]
        [StringLength(10)]
        public string MaTDN { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Ngay { get; set; }

        [StringLength(500)]
        public string BuaSang { get; set; }

        [StringLength(500)]
        public string BuaTrua { get; set; }

        [StringLength(500)]
        public string BuaXe { get; set; }
    }
}
