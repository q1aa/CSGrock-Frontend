using CSGrock_Frontend.CSGrockLogic.Utils.Enums;
using CSGrock_Frontend.CSGrockLogsServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CSGrock_Frontend.CSGrockLogic.Utils.Enums.RequestMethodeEnum;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class ConsoleUtil
    {
        public static void SendLogMessage(string url, RequestMethode requestMethode)
        {
            switch (requestMethode)
            {
                case RequestMethodeEnum.RequestMethode.GET:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("GET");
                    break;
                case RequestMethode.POST:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("POST");
                    break;
                case RequestMethode.PUT:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("PUT");
                    break;
                case RequestMethode.PATCH:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("PATCH");
                    break;
                case RequestMethode.DELETE:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("DELETE");
                    break;
                case RequestMethode.HEAD:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("HEAD");
                    break;
                case RequestMethode.OPTIONS:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("OPTIONS");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {url}");
            Console.WriteLine();
        }

        public static void AddLog(string url, RequestMethode requestMethode)
        {
            //add to the beginning
            StorageUtil.LogsStorage.Insert(0, (url, requestMethode));
            WriteConsoleInfoMessages();
        }

        public static void WriteConsoleInfoMessages()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------------");
            PrintWithColor($"Logserver started up on port {CSGrockLogsServer.Utils.StorageUtil.ServerPort} successfully!", ConsoleColor.Green);
            Console.WriteLine($"http://localhost:{CSGrockLogsServer.Utils.StorageUtil.ServerPort}/ to view the logs in your browser");
            Console.WriteLine("---------------------------------------------------------------");
            PrintWithColor("You can perform request trough our backend under the following url now", ConsoleColor.Green);
            Console.WriteLine($"https://{StorageUtil.BackendURL}/send/{StorageUtil.UUID}/");
            Console.WriteLine($"Example: https://{StorageUtil.BackendURL}/send/{StorageUtil.UUID}/api/v1/getUsers");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine();

            for (int i = 0; i < 20; i++)
            {
                if(StorageUtil.LogsStorage.Count <= i) break;

                SendLogMessage(StorageUtil.LogsStorage[i].Item1, StorageUtil.LogsStorage[i].Item2);
            }
        }

        public static void PrintWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
