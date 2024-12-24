using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Product.Models.產品資料;

namespace Product.Models
{
    [MetadataType(typeof(會員MetaData))]
    public partial class 會員
    {
        public class 會員MetaData
        {
            [Required]
            [StringLength(20, MinimumLength = 3)]
            public string 帳號 { get; set; }
            [Required]
            [StringLength(20, MinimumLength = 3)]
            public string 密碼 { get; set; }
            
            public string 角色 { get; set; }
            
            public string 權限 { get; set; }
        }
    }
}