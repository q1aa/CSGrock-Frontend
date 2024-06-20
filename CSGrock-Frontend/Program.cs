using System;
using System.Threading;
using System.Net.WebSockets;
using CSGrock_Frontend.CSGrockLogic.Socket;
using System.Threading.Tasks;
using CSGrock_Frontend.CSGrockLogic.Utils;

namespace CSGrock_Frontend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0) _ = StartUpWithoutArgsAsync();
            else {
                string serverType = args[0].Replace("--", "");
                string port = args[1].Replace("--", "");

                try
                {
                    int.Parse(port);
                }
                catch (Exception)
                {
                    printUsage();
                    return;
                }

                StartConnections(int.Parse(port), serverType);
            }
            Console.ReadLine();
        }

        static async Task StartUpWithoutArgsAsync()
        {
            Console.WriteLine("Please enter the server type and port you want to start up split by a space. (Example: http 8080)");
            string[] input = Console.ReadLine().Split(' ');
            if(input.Length != 2)
            {
                printUsage();
                await StartUpWithoutArgsAsync();
            }
            else
            {
                string serverType = input[0];
                string port = input[1];

                StartConnections(int.Parse(port), serverType);
                Console.ReadLine();
            }
        }

        static async void StartConnections(int port, string serverType = "http")
        {
            Console.Clear();
            Console.WriteLine($"Starting up server for port {port} with server type {serverType}");
            StorageUtil.ForwardPort = port;
            new Thread(() =>
            {
                CSGrockLogsServer.LogsServer.StartUp();
            }).Start();
            CSGrockLogsServer.Utils.LogUtil.CreateLogJSONFile();
            await SocketConnection.ConnectToSocket();
        }

        static void printUsage()
        {
            Console.WriteLine("Invalid input. Please try again.");
            Console.WriteLine("example: CSGrok http 8080");
        }
    }
}
