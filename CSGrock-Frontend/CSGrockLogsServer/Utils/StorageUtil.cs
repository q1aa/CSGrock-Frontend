using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer.Utils
{
    internal class StorageUtil
    {
        public static int ServerPort = -1;
        public static HttpListener listener;
        public static string ServerURL = null;
        public static bool IsServerRunning = false;
        
        public static string LogDirectory = "Logs";
        public static string LogFilePath = null;

        public static void SetUrl()
        {
            StorageUtil.ServerURL = $"http://localhost:{ServerPort}/";
        }
    }
}
