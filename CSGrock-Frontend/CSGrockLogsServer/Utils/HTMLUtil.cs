using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer.Utils
{
    internal class HTMLUtil
    {
        public static string GetResponseHTML()
        {
            return $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Document</title>\r\n    <link rel=\"stylesheet\" href=\"style.css\">\r\n</head>\r\n<body>\r\n    <div class=\"main\"><div class=\"request-holder\">{GetRequestHTML()}</div>\r\n    </div>\r\n<script>\r\n    setInterval(() => location.reload(), 1000);\r\n</script>\r\n</body>\r\n</html>";
        }

        public static string GetCSS()
        {
            return @"* {margin: 0;padding: 0; box-sizing: border-box; font-family: 'Roboto', sans-serif; } .info {display: flex;background-color: #f1f1f1; flex-direction: column; align-items: center; width: 90%;margin-top: 20px; }.info-holder {display: flex;justify-content: space-between; align-items: center; padding: 0 20px; background-color: #f1f1f1; }.request-holder { display: flex;background-color: #f1f1f1; flex-direction: column; align-items: center; }.request-holder .request { width: 400px;background-color: #fff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); border: 1px solid #e1e1e1; width: 90%;display: flex;justify-content: space-between; align-items: center; height: 100px;margin-top: 20px; }.request .request-type-holder { height: 100%; width: 150px;display: flex;justify-content: center; align-items: center; border-radius: 10px 0 0 10px; }.request .request-url-holder { width: calc(100% - 100px); height: 100%;display: flex;align-items: center; padding: 0 10px; }.request .request-time-holder { background-color: #dddcd2; height: 100%;width: 150px;display: flex;justify-content: center; align-items: center; border-radius: 0 10px 10px 0; }.GET-request {background-color: green; }.POST-request { background-color: yellow; }.PUT-request {background-color: blue; }.DELETE-request { background-color: red; }.OPTIONS-request { background-color: darkmagenta; }.PATCH-request { background-color: magenta; }.HEAD-request {background-color: darkgreen; }@media (max-width: 768px) { .info {width: 100%;}.request-holder .request { width: 100%;}}@media (max-width: 576px) { .request-holder .request { flex-direction: column; height: auto;}.request .request-type-holder { border-radius: 10px 10px 0 0; }.request .request-url-holder { width: 100%;padding: 10px 10px; }.request .request-time-holder { border-radius: 0 0 10px 10px; }.request .request-type-holder { width: 100%;border-radius: 0; }.request .request-time-holder { width: 100%;border-radius: 0; }}";
        }

        private static string GetRequestHTML()
        {
            string fileContent = File.ReadAllText(StorageUtil.LogFilePath);
            LogEntry[] logEntries = JsonConvert.DeserializeObject<LogEntry[]>(fileContent);
            string requestHTML = "";
            foreach (LogEntry logEntry in logEntries)
            {
                requestHTML += GenerateHTMLRequest(logEntry.RequestMethode, logEntry.RequestPath, logEntry.RequestTime);
            }
            return requestHTML;
        }

        private static string GenerateHTMLRequest(string requestMethode, string requestPath, string requestTime)
        {
            return $"<div class=\"request next-to-each-other\"><div class=\"request-type-holder {requestMethode}-request\"><h2>{requestMethode}</h2></div><div class=\"request-url-holder\"><h3>{requestPath}</h3></div><div class=\"request-time-holder\"><h3>at {requestTime}</h3></div></div>";
        }
    }
}
