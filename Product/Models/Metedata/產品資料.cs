using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Models
{

    [MetadataType(typeof(產品資料MetaData))]
    public partial class 產品資料
    {
        //模型的其他屬性和方法

        public class 產品資料MetaData
        {
            [Required]
            [StringLength(20, MinimumLength = 2)]
            
            public string 產品編號 { get; set; }
            [Required]
            [Remote("IsNameAvailable", "Product", ErrorMessage = "品名名稱已被使用", AdditionalFields = "產品編號")]
            public string 品名 { get; set; }
            [Required]
            [Range(0,10000)]
            public Nullable<int> 單價 { get; set; }
            public string 圖示 { get; set; }
            [Required]
            public Nullable<int> 類別編號 { get; set; }
            public string 編輯者 { get; set; }
            public string 建立日 { get; set; }
            public string 修改日 { get; set; }
        }
    }
}

