using CSGrock_Frontend.CSGrockLogsServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer
{
    internal class LogsServer
    {
        /*https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7*/
        public static void StartUp()
        {
            Console.WriteLine("Starting up server...");
            StorageUtil.ServerPort = PortUtil.GetValidPort(5000, 6000);
            if(StorageUtil.ServerPort == -1)
            {
                Console.WriteLine("No available ports in range 5000-6000");
                return;
            }

            StorageUtil.SetUrl();

            StorageUtil.listener = new HttpListener();
            StorageUtil.listener.Prefixes.Add(StorageUtil.ServerURL);
            StorageUtil.listener.Start();
            Console.WriteLine("Listening for connections on {0}", StorageUtil.ServerPort);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            StorageUtil.listener.Close();
        }

        /*https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7*/
        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await StorageUtil.listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Console.WriteLine(req.Url.AbsolutePath.ToString());
                switch (req.Url.AbsolutePath)
                {
                    case "/":
                        sendresponse(resp, HTMLUtil.GetResponseHTML());
                        break;
                    case "/style.css":
                        sendresponse(resp, HTMLUtil.GetCSS(), "text/css");
                        break;
                    default:
                        sendresponse(resp, "<h2>404 not found!</h2>");
                        break;
                }
            }
        }

        public static async void sendresponse(HttpListenerResponse resp, string data, string contentType = "text/html")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            resp.ContentType = contentType;
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = dataBytes.LongLength;

            await resp.OutputStream.WriteAsync(dataBytes, 0, dataBytes.Length);
            resp.Close();
        }
    }
}
