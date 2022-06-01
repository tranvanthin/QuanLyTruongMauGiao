using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PagedList;

namespace QuanLyTruongMauGiao.Models.NoDatabase
{
    public class PCGiaoVien
    {
        public string MaLop { get; set; }
        public string NamHoc { get; set; }

        public List<string> MaGV;
    }
}