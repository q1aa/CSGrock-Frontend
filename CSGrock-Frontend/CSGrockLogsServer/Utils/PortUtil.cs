using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer.Utils
{
    internal class PortUtil
    {
        public static int GetValidPort(int rangeFrom, int rangeTo)
        {
            for (int i = rangeFrom; i <= rangeTo; i++)
            {
                if (IsPortAvailable(i)) return i;
            }
            return -1;
        }

        public static bool IsPortAvailable(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            
            if(tcpConnInfoArray.Any(endpoint => endpoint.Port == port)) return false;
            return true;
        }
    }
}
