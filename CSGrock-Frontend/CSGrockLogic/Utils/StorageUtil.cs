﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class StorageUtil
    {
        public static ClientWebSocket webSocket = null;

        public static string BackendURL = "csgrock.azurewebsites.net";

        public static int ForwardPort = 0;
    }
}