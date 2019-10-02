using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Endpoint
{
    public static class EndpointExtensions
    {
        public static bool HasFiles(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("HASFILES", "False"));
        }

        public static bool CanStream(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("CANSTREAM", "False"));
        }

        public static bool CanRead(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("CanRead", "False"));
        }

        public static bool CanWrite(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("CanWrite", "False"));
        }

        public static bool CanList(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("CanList", "False"));
        }

        public static bool CanListen(this IEasyEndpoint endpoint)
        {
            return Convert.ToBoolean(endpoint.GetEasyProperty("CanListen", "False"));
        }
        
    }
}
