using EasyETL.Extractors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.IFilter
{
    [DisplayName("Generic Reader")]
    public class GenericContentExtractor : AbstractContentExtractor
    {

        public override Stream GetStream(Stream inStream)
        {
            if (String.IsNullOrWhiteSpace(FileName)) return inStream;
            inStream.Position = 0;
            IFilter filter = Load(inStream, Path.GetExtension(FileName));
            string contents = String.Empty;
            if (filter !=null) contents = GetText(filter);
            return new MemoryStream(Encoding.ASCII.GetBytes(contents));
        }

        //private string ParseIFilter(Stream s)
        //{
        //    // Get an IFilter for a file or file extension
        //    IFilter filter = null;
        //    FilterReturnCodes result = NativeMethods.LoadIFilter(Path.GetExtension(FileName), null, ref filter);
        //    if (result != FilterReturnCodes.S_OK)
        //    {
        //        Marshal.ThrowExceptionForHR((int)result);
        //    }

        //    // Copy the content to global memory
        //    byte[] buffer = new byte[s.Length];
        //    s.Read(buffer, 0, buffer.Length);
        //    IntPtr nativePtr = Marshal.AllocHGlobal(buffer.Length);
        //    Marshal.Copy(buffer, 0, nativePtr, buffer.Length);

        //    // Create a COM stream
        //    IStream comStream;
        //    NativeMethods.CreateStreamOnHGlobal(nativePtr, true, out comStream);

        //    // Load the contents to the iFilter using IPersistStream interface
        //    var persistStream = (IPersistStream)filter;
        //    persistStream.Load(comStream);

        //    // Initialize iFilter
        //    FilterFlags filterFlags;
        //    result = filter.Init(
        //       FilterInit.IFILTER_INIT_INDEXING_ONLY, 0, IntPtr.Zero, out filterFlags);

        //    return ExtractTextFromIFilter(filter);
        //}

        //private string ExtractTextFromIFilter(IFilter filter)
        //{
        //    var sb = new StringBuilder();


        //    while (true)
        //    {
        //        StatChunk chunk;
        //        FilterReturnCodes result = filter.GetChunk(out chunk);

        //        if (result == FilterReturnCodes.S_OK)
        //        {
        //            if (chunk.flags == ChunkState.CHUNK_TEXT)
        //            {
        //                sb.Append(ExtractTextFromChunk(filter, chunk));
        //            }

        //            continue;
        //        }

        //        if (result == FilterReturnCodes.FILTER_E_END_OF_CHUNKS)
        //        {
        //            return sb.ToString();
        //        }

        //        Marshal.ThrowExceptionForHR((int)result);
        //    }
        //}

        //private string ExtractTextFromChunk(IFilter filter, StatChunk chunk)
        //{
        //    var sb = new StringBuilder();

        //    var result = FilterReturnCodes.S_OK;
        //    while (result == FilterReturnCodes.S_OK)
        //    {
        //        int sizeBuffer = 16384;
        //        var buffer = new StringBuilder(sizeBuffer);
        //        result = filter.GetText(ref sizeBuffer, buffer);

        //        if ((result == FilterReturnCodes.S_OK) || (result == FilterReturnCodes.FILTER_S_LAST_TEXT))
        //        {
        //            if ((sizeBuffer > 0) && (buffer.Length > 0))
        //            {
        //                sb.Append(buffer.ToString(0, sizeBuffer));
        //            }
        //        }

        //        if (result == FilterReturnCodes.FILTER_E_NO_TEXT)
        //        {
        //            return string.Empty;
        //        }

        //        if ((result == FilterReturnCodes.FILTER_S_LAST_TEXT) || (result == FilterReturnCodes.FILTER_E_NO_MORE_TEXT))
        //        {
        //            return sb.ToString();
        //        }
        //    }

        //    return sb.ToString();
        //}

        private IFilter Load(Stream stream, string extension)
        {
            IFilter filter = null;

            if (NativeMethods.LoadIFilter(extension, null, ref filter) == HRESULT.S_OK)
            {
                if (filter is IPersistStream persistStream)
                {
                    persistStream.Load(new ManagedStream(stream));
                    if (filter.Init(IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES, 0, IntPtr.Zero, out _) == IFilterReturnCodes.S_OK)
                    {
                        return filter;
                    }
                }
            }

            return null;
        }

        private string GetText(IFilter filter)
        {
            var text = new StringBuilder();

            while (filter.GetChunk(out var chunk) == IFilterReturnCodes.S_OK)
            {
                ReadChunk(filter, chunk, text);
            }

            return text.ToString();
        }

        private  void ReadChunk(IFilter filter, STAT_CHUNK chunk, StringBuilder text)
        {
            var textResult = IFilterReturnCodes.S_OK;
            while (textResult == IFilterReturnCodes.S_OK)
            {
                var bufferSize = 4096U;
                var buffer = new char[bufferSize];
                textResult = filter.GetText(ref bufferSize, buffer);

                if ((textResult == IFilterReturnCodes.S_OK || textResult == IFilterReturnCodes.FILTER_S_LAST_TEXT) && bufferSize > 0)
                {
                    if (chunk.breakType == CHUNK_BREAKTYPE.CHUNK_EOP)
                    {
                        text.Append('\n');
                    }

                    text.Append(buffer, 0, (int)bufferSize);
                }
            }
        }
    }
}
