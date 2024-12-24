using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Product.WebShared
{
    public class LogExecutionTimeAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string logText = $"\n[{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName} : {filterContext.ActionDescriptor.ActionName}] -> OnActionExecuting \t- {DateTime.Now} \n";
            LogExecutionTimeIntoFile(logText);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string logText = $"\n[{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName} : {filterContext.ActionDescriptor.ActionName}] -> OnActionExecuted \t- {DateTime.Now} \n";
            LogExecutionTimeIntoFile(logText);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string logText = $"\n[{filterContext.RouteData.Values["controller"]} : {filterContext.RouteData.Values["action"]}] -> OnResultExecuting \t- {DateTime.Now} \n";
            LogExecutionTimeIntoFile(logText);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string logText = $"\n[{filterContext.RouteData.Values["controller"]} : {filterContext.RouteData.Values["action"]}] -> OnResultExecuted \t- {DateTime.Now} \n";
            LogExecutionTimeIntoFile(logText);
            LogExecutionTimeIntoFile("---------------------------------------------------------\n");
        }

        public void OnException(ExceptionContext filterContext)
        {
            string logText = $"\n[{filterContext.RouteData.Values["controller"]} : {filterContext.RouteData.Values["action"]}] -> \n OnException Message: {filterContext.Exception.Message} OnResultExecuted \t- {DateTime.Now} \n";
            LogExecutionTimeIntoFile(logText);
            LogExecutionTimeIntoFile("---------------------------------------------------------\n");
        }

        private void LogExecutionTimeIntoFile(string logText)
        {
            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/LogExecutionTime/LogExecutionTime.txt"), logText);
            string filePath = HttpContext.Current.Server.MapPath($"~/Log/{DateTime.UtcNow.Year}\\{DateTime.UtcNow.Month}\\{DateTime.UtcNow.Day}.txt");

            // 檢查檔案是否存在，如果不存在則創建檔案
            if (!File.Exists(filePath))
            {
                // 確保目錄存在
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // 創建檔案並寫入初始內容
                File.WriteAllText(filePath, logText);
            }
            else
            {
                // 檔案存在，追加內容
                File.AppendAllText(filePath, logText);
            }
        }
    }
}