using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Product.Models
{
    public class CategoryProductViewModel
    {
        public List<產品類別> Category { get; set; }
        public List<產品資料> Product { get; set; }

        public IPagedList<產品資料> Page_Product { get; set; }
    }
}