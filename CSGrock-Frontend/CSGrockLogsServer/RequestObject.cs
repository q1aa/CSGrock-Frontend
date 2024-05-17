using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogsServer
{
    internal class LogEntry
    {
        public string RequestMethode { get; set; }
        public string RequestPath { get; set; }
        public string RequestTime { get; set; }
    }
}
