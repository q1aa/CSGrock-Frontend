using CSGrock_Frontend.CSGrockLogic.Requests;
using CSGrock_Frontend.CSGrockLogic.Struct;
using CSGrock_Frontend.CSGrockLogic.Utils;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Socket
{
    internal class SocketConnection
    {
        public static async Task ConnectToSocket()
        {
            try
            {
                byte[] buffer = new byte[1056];

                StorageUtil.webSocket = new ClientWebSocket();
                await StorageUtil.webSocket.ConnectAsync(new Uri(StorageUtil.BackendURL), CancellationToken.None);

                byte[] buf = new byte[1056];

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


        private static async Task<Task> ReceiveMessage(string messageContent)
        {
            if(messageContent.StartsWith("You are connected with UUID ")) {
                Console.WriteLine("You can perform request trough our backend under the following url now");
                Console.WriteLine(messageContent.Replace("You are connected with UUID ", ""));
                return Task.CompletedTask;
            }

            //TODO: implement header support

            var requestJSON = JSONUtil.ConvertJSONToRequest(messageContent);
            requestJSON.requestURL = requestJSON.requestURL.Substring(5);

            string requestURL = "http://localhost:" + StorageUtil.ForwardPort + requestJSON.requestURL.ToString();
            Console.WriteLine("Sending request to " + requestURL);

            IncomingRequestStruct requestStruct = new IncomingRequestStruct(requestJSON.requestBody, requestJSON.requestHeaders, Utils.Enums.RequestMethodeEnum.RequestMethode.GET, requestURL, requestJSON.requestID);

            var result = RequestHandler.HandleRequestAsync(requestStruct);
            var resultJSON = JSONUtil.ConvertResponseToJSON(result.Result);
            await SendMessage("Receaving from " + resultJSON);
            return Task.CompletedTask;
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
