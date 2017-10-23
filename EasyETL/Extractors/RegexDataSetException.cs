using System;

namespace EasyETL.DataSets
{
    /// <summary>
    ///     Special exception for TextFileDataSet
    /// </summary>
    public class RegexDataSetException : ApplicationException
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="message">Message for this exception</param>
        public RegexDataSetException(string message)
            : base(message)
        {
        }
    }
}