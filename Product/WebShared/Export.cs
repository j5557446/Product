using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.WebShared
{
    
    public class Export : Controller
    {
        private readonly string password = "123456";
        public ActionResult ExportToExcel(IEnumerable<dynamic> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

                if (data != null && data.Any())
                {
                    var firstItem = data.First();
                    var properties = firstItem.GetType().GetProperties();

                    // 動態設置標題
                    for (int i = 0; i < properties.Length; i++)
                    {
                        workSheet.Cells[1, i + 1].Value = properties[i].Name;
                    }

                    // 將資料寫入工作表
                    int row = 2;
                    foreach (var item in data)
                    {
                        for (int i = 0; i < properties.Length; i++)
                        {
                            workSheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
                        }
                        row++;
                    }
                }

                // 設置密碼保護
                
                excel.Encryption.IsEncrypted = true;
                excel.Encryption.Password = password;

                byte[] excelBytes = excel.GetAsByteArray();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.UtcNow.Year}\\{DateTime.UtcNow.Month}\\{DateTime.UtcNow.Day}\\{DateTime.UtcNow.Hour}\\{DateTime.UtcNow.Minute}\\{DateTime.UtcNow.Second}.xlsx");
            }
        }
    }

}
