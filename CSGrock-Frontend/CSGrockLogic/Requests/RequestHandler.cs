﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CSGrock_Frontend.CSGrockLogic.Struct;
using CSGrock_Frontend.CSGrockLogic.Utils;
using System.Web.UI;

namespace CSGrock_Frontend.CSGrockLogic.Requests
{
    internal class RequestHandler
    {
        public static async Task<RequestResultStruct> HandleRequestAsync(IncomingRequestStruct requestStruct)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(requestStruct.requestURL);

            string requestID = requestStruct.requestID;

            foreach (var header in requestStruct.requestHeaders)
            {
                if (header.Key == null || header.Value == null) continue;

                try {
                    client.DefaultRequestHeaders.Add(header.Key.ToString(), header.Value.ToString());
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error while adding header: " + header.Key + " " + header.Value);
                    //Console.WriteLine(e.Message);
                }   
            }

            switch (requestStruct.requestMethode)
            {
                case Utils.Enums.RequestMethodeEnum.RequestMethode.GET:
                    return await PerformGetRequest(client, requestID);

                case Utils.Enums.RequestMethodeEnum.RequestMethode.POST:
                    return await PerformPostRequets(client, requestStruct.requestBody, requestID);

                case Utils.Enums.RequestMethodeEnum.RequestMethode.PUT:
                    return await PerformPutRequset(client, requestStruct.requestBody, requestID);

                case Utils.Enums.RequestMethodeEnum.RequestMethode.DELETE:
                    return await PerformDeleteRequest(client, requestID);

                case Utils.Enums.RequestMethodeEnum.RequestMethode.HEAD:
                    return await PerformHeadRequest(client, requestID);

                case Utils.Enums.RequestMethodeEnum.RequestMethode.OPTIONS:
                    return await PerformOptionsRequest(client, requestID);
            }

            client.Dispose();
            return null;
        }

        public static async Task<RequestResultStruct> PerformGetRequest(HttpClient client, string requestID)
        {
            try
            {
                client.DefaultRequestHeaders.Remove("Host");
                client.DefaultRequestHeaders.Add("Host", client.BaseAddress.Host);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the get request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPostRequets(HttpClient client, string content, string requestID)
        {
            try 
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress.ToString(), httpContent);

                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the post request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPutRequset(HttpClient client, string content, string requestID)
        {
            try
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress.ToString(), httpContent);
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the put request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformDeleteRequest(HttpClient client, string requestID)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress);
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the delete request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformHeadRequest(HttpClient client, string requestID)
        {
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, client.BaseAddress));
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the head request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformOptionsRequest(HttpClient client, string requestID)
        {
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, client.BaseAddress));
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the options request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPatchRequest(HttpClient client, string requestID)
        {
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), client.BaseAddress));
                RequestResultStruct result = await ResponseHandler.HandleResponseAsync(response, requestID);
                client.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the patch request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }
    }

    internal class FileRequestHandler
    {
        public static async Task<RequestResultStruct> HandleFileRequestAsync(IncomingRequestStruct requestStruct)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(requestStruct.requestURL);

            string requestID = requestStruct.requestID;

            foreach (var header in requestStruct.requestHeaders)
            {
                if (header.Key == null || header.Value == null) continue;

                try
                {
                    client.DefaultRequestHeaders.Add(header.Key.ToString(), header.Value.ToString());
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error while adding header: " + header.Key + " " + header.Value);
                    //Console.WriteLine(e.Message);
                }
            }

            var result = await PerformFileGetRequest(client, requestID);
            return result;
        }

        private static async Task<RequestResultStruct> PerformFileGetRequest(HttpClient client, string requestID)
        {
            try
            {
                string base64 = string.Empty;

                var repsponse = await client.GetAsync(client.BaseAddress);
                if (repsponse.IsSuccessStatusCode)
                {
                    var bytes = await repsponse.Content.ReadAsByteArrayAsync();
                    base64 = Convert.ToBase64String(bytes);
                }

                return new RequestResultStruct(base64, new Dictionary<string, string>(), repsponse.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while performing the file get request.");
                //Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
