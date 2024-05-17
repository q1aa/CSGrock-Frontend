using CSGrock_Frontend.CSGrockLogic.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer.Utils
{
    internal class LogUtil
    {
        public static void CreateLogJSONFile()
        {
            Console.WriteLine("Creating log file...");
            string path = $"{Directory.GetCurrentDirectory()}/{StorageUtil.LogDirectory}/logs-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.json";
            if (!Directory.Exists(StorageUtil.LogDirectory))
            {
                Console.WriteLine("Log directory does not exist. Creating directory...");
                Directory.CreateDirectory(StorageUtil.LogDirectory);
            }

            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine("[]");
                writer.Close();
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("Failed to create log file.");
            }
            Console.WriteLine("Log file created.");
            StorageUtil.LogFilePath = path;
        }

        public static void AppendToJSONLogs(RequestMethodeEnum.RequestMethode requestMethode, string requestPath, string requestTime = null)
        {
            if (requestTime == null) requestTime = DateTime.Now.ToString("MM-dd-HH-mm-ss");
            if(StorageUtil.LogFilePath == null)
            {
                Console.WriteLine("Log file path is null. Cannot append to log file.");
                return;
            }

            LogEntry logEntry = new LogEntry
            {
                RequestMethode = requestMethode.ToString(),
                RequestPath = requestPath,
                RequestTime = requestTime
            };

            //TODO: implement a better way to parse the JSON
            string json = JsonConvert.SerializeObject(logEntry);
            string fileContent = File.ReadAllText(StorageUtil.LogFilePath);
            Console.WriteLine(fileContent);
            if (fileContent.StartsWith("[]")) fileContent = $"[{json}]";
            else
            {
                fileContent = fileContent.Remove(fileContent.Length - 1);
                fileContent += $",{json}]";
            }

            File.WriteAllText(StorageUtil.LogFilePath, fileContent);
        }
    }
}
