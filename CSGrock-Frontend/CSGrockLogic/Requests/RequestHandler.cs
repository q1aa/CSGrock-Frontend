﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CSGrock_Frontend.CSGrockLogic.Struct;
using CSGrock_Frontend.CSGrockLogic.Utils;

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

            Console.WriteLine("header count: " + requestStruct.requestHeaders.Count);

            //TODO: build in header support
            foreach (var header in requestStruct.requestHeaders)
            {
                if (header.Key == null || header.Value == null) continue;

                try {
                    client.DefaultRequestHeaders.Add(header.Key.ToString(), header.Value.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while adding header: " + header.Key + " " + header.Value);
                    Console.WriteLine(e.Message);
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
            Console.WriteLine("Performing get request");
            try
            {
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.Key != null && header.Value != null)
                    {
                        responseHeaders.Add(header.Key, header.Value.FirstOrDefault());
                    }
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPostRequets(HttpClient client, string content, string requestID)
        {
            Console.WriteLine("Performing post request");
            try 
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(client.BaseAddress.ToString(), httpContent);

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPutRequset(HttpClient client, string content, string requestID)
        {
            Console.WriteLine("Performing put request");
            try
            {
                HttpContent httpContent = new StringContent(content);
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress.ToString(), httpContent);

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformDeleteRequest(HttpClient client, string requestID)
        {
            Console.WriteLine("Performing delete request");
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress);

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformHeadRequest(HttpClient client, string requestID)
        {
            Console.WriteLine("Performing head request");
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, client.BaseAddress));

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformOptionsRequest(HttpClient client, string requestID)
        {
            Console.WriteLine("Performing options request");
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, client.BaseAddress));

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<RequestResultStruct> PerformPatchRequest(HttpClient client, string requestID)
        {
            Console.WriteLine("Performing patch request");
            try
            {
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), client.BaseAddress));

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    if (header.ToString().Contains(":")) continue;
                    if (header.ToString().Split(':').Length < 2) continue;

                    string headerKey = header.ToString().Split(':')[0];
                    string headerValue = header.ToString().Split(':')[1];
                    responseHeaders.Add(headerKey, headerValue);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                client.Dispose();
                return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
