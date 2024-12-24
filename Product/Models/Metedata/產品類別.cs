using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Models
{
    [MetadataType(typeof(產品類別MetaData))]
    public partial class 產品類別
    {
        // 模型的其他屬性和方法


        public class 產品類別MetaData
        {
            public int 類別編號 { get; set; }

            [Required]
            [Remote("IsCategoryAvailable", "Category", ErrorMessage = "類別名稱已被使用", AdditionalFields = "類別編號")]
            [Display(Name = "類別名稱")]
            public string 類別名稱 { get; set; }

            [Display(Name = "編輯者")]
            public string 編輯者 { get; set; }

            [Display(Name = "建立日")]
            public string 建立日 { get; set; }

            [Display(Name = "修改日")]
            public string 修改日 { get; set; }
        }
    }
}