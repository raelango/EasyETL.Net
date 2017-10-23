using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Loaders
{
    [Serializable]
    public class EasyLoaderException : ApplicationException
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="message">Message for this exception</param>
        public EasyLoaderException(string message)
            : base(message)
        {
        }
    }
}


