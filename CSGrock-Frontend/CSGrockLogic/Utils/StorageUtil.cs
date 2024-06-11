using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using static CSGrock_Frontend.CSGrockLogic.Utils.Enums.RequestMethodeEnum;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class StorageUtil
    {
        public static ClientWebSocket webSocket = null;

        public static string BackendURL = "csgrok.julin.dev";

        //public static string BackendURL = "localhost:7006";

        public static int ForwardPort = 0;
        
        public static string UUID = "";
        
        public static List<(string, RequestMethode)> LogsStorage = new List<(string, RequestMethode)>();

        public static List<string> imgeFileTypes = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp", ".svg", ".ico", ".tiff", ".psd", ".raw" };
        public static List<string> videoFileTypes = new List<string> { ".mp4", ".webm", ".ogg", ".flv", ".avi", ".mov", ".wmv", ".mpg", ".mpeg", ".3gp", ".mkv", ".vob", ".rm", ".swf", ".m4v", ".m2v", ".mpe", ".m1v", ".mpv", ".mpa", ".mp2", ".mp3", ".m4a", ".m4p", ".m4b", ".m4r" };
    }
}
