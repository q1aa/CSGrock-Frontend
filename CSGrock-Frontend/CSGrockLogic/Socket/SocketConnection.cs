using CSGrock_Frontend.CSGrockLogic.Requests;
using CSGrock_Frontend.CSGrockLogic.Struct;
using CSGrock_Frontend.CSGrockLogic.Utils;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CSGrock_Frontend.CSGrockLogic.Utils.Enums.RequestMethodeEnum;

namespace CSGrock_Frontend.CSGrockLogic.Socket
{
    internal class SocketConnection
    {
        public static async Task ConnectToSocket()
        {
            try
            {
                byte[] buffer = new byte[16385];

                StorageUtil.webSocket = new ClientWebSocket();
                await StorageUtil.webSocket.ConnectAsync(new Uri("wss://" + StorageUtil.BackendURL + "/ws"), CancellationToken.None);

                byte[] buf = new byte[16385];

                while (StorageUtil.webSocket.State == WebSocketState.Open)
                {
                    var arraySegment = new ArraySegment<byte>(buf);
                    var result = await StorageUtil.webSocket.ReceiveAsync(arraySegment, CancellationToken.None);

                    await HandleMessageReceive(arraySegment, result, buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }

        }

        private static async Task HandleMessageReceive(ArraySegment<byte> arraySegment, WebSocketReceiveResult receiveResult, byte[] buffer)
        {
            if (receiveResult.MessageType == WebSocketMessageType.Close) await HandleClose();

            string messageContent = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, receiveResult.Count);
            await ReceiveMessage(messageContent);
        }

        //TODO: Patch request isnt working!
        private static async Task<Task> ReceiveMessage(string messageContent)
        {
            if (messageContent.StartsWith("You are connected with UUID "))
            {
                string uuid = messageContent.Replace("You are connected with UUID ", "");
                Console.WriteLine("You can perform request trough our backend under the following url now");
                Console.WriteLine($"https://{StorageUtil.BackendURL}/send/{uuid}/");
                Console.WriteLine($"Example: https://{StorageUtil.BackendURL}/send/{uuid}/api/v1/getUsers");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine();
                return Task.CompletedTask;
            }

            try
            {
                var requestJSON = JSONUtil.ConvertJSONToRequest(messageContent);
                if(requestJSON.requestURL == null) return Task.CompletedTask;

                string requestURL = "http://localhost:" + StorageUtil.ForwardPort + requestJSON.requestURL.ToString();

                if (requestURL.Contains("."))
                {
                    string fileType = requestJSON.requestURL.Substring(requestJSON.requestURL.LastIndexOf('.'));
                    if (StorageUtil.imgeFileTypes.Contains(fileType) || StorageUtil.videoFileTypes.Contains(fileType))
                    {
                        var errorResult = new RequestResultStruct("Sry, this file type is not supported yet ://", new Dictionary<string, string>(), System.Net.HttpStatusCode.BadRequest, requestJSON.requestID);
                        var errorResultJSON = JSONUtil.ConvertResponseToJSON(errorResult);
                        await SendMessage("Receaving from " + errorResultJSON);


                        /*IncomingRequestStruct imageRequsetStruct = new IncomingRequestStruct(requestJSON.requestBody, requestJSON.requestHeaders, requestJSON.requestMethode, requestURL, requestJSON.requestID);
                        var imageResult = FileRequestHandler.HandleFileRequestAsync(imageRequsetStruct);
                        Console.WriteLine("Image result: " + imageResult.Result.resultContent.Length);*/

                        return Task.CompletedTask;
                    }
                }

                ConsoleUtil.SendLogMessage(requestURL, requestJSON.requestMethode);
                CSGrockLogsServer.Utils.LogUtil.AppendToJSONLogs(requestJSON.requestMethode, requestJSON.requestURL);
                //requestJSON.requestURL = requestJSON.requestURL.Substring(5);

                IncomingRequestStruct requestStruct = new IncomingRequestStruct(requestJSON.requestBody, requestJSON.requestHeaders, requestJSON.requestMethode, requestURL, requestJSON.requestID);

                var result = RequestHandler.HandleRequestAsync(requestStruct);
                if (result == null)
                {
                    await SendMessage("Invalid request");
                    return Task.CompletedTask;
                }
                else if (result.Result == null)
                {
                    await SendMessage("Invalid request");
                    return Task.CompletedTask;
                }

                var resultJSON = JSONUtil.ConvertResponseToJSON(result.Result);
                await SendMessage("Receaving from " + resultJSON);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                return Task.CompletedTask;
            }
        }

        public static async Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await StorageUtil.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static async Task HandleClose()
        {
            await StorageUtil.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }
    }
}
