using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

//Contains IFilter interface translation
//Most translations are from PInvoke.net

namespace EasyETL.IFilter
{
    [Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFilter
    {
        [PreserveSig]
        IFilterReturnCodes Init(IFILTER_INIT grfFlags, int cAttributes, IntPtr aAttributes,
            out IFILTER_FLAGS pdwFlags);

        [PreserveSig]
        IFilterReturnCodes GetChunk(out STAT_CHUNK pStat);

        [PreserveSig]
        IFilterReturnCodes GetText(ref uint pcwcBuffer, [Out, MarshalAs(UnmanagedType.LPArray)]
                char[] awcBuffer);

        [PreserveSig]
        IFilterReturnCodes GetValue(ref IntPtr propVal);

        [PreserveSig]
        IFilterReturnCodes BindRegion(ref FILTERREGION origPos, ref Guid riid, ref object ppunk);
    }

    [Guid("0000010C-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersist
    {
        void GetClassID(out Guid pClassID);
    }

    [Guid("00000109-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistStream : IPersist
    {
        new void GetClassID(out Guid pClassID);

        [PreserveSig]
        int IsDirty();

        void Load([In] IStream pStm);

        void Save([In] IStream pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

        void GetSizeMax(out long pcbSize);
    }

    [Guid("0000000C-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStream
    {
        [PreserveSig]
        HRESULT Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out]
                byte[] pv, int cb, IntPtr pcbRead);

        [PreserveSig]
        HRESULT Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                byte[] pv, int cb, IntPtr pcbWritten);

        [PreserveSig]
        HRESULT Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

        [PreserveSig]
        HRESULT SetSize(long libNewSize);

        HRESULT CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

        [PreserveSig]
        HRESULT Commit(int grfCommitFlags);

        [PreserveSig]
        HRESULT Revert();

        [PreserveSig]
        HRESULT LockRegion(long libOffset, long cb, int dwLockType);

        [PreserveSig]
        HRESULT UnlockRegion(long libOffset, long cb, int dwLockType);

        [PreserveSig]
        HRESULT Stat(out STATSTG pstatstg, int grfStatFlag);

        [PreserveSig]
        HRESULT Clone(out IStream ppstm);
    }

    public class ManagedStream : IStream
    {
        private readonly Stream _stream;

        public ManagedStream(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public HRESULT Clone(out IStream ppstm)
        {
            ppstm = null;
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT Commit(int grfCommitFlags)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT LockRegion(long libOffset, long cb, int dwLockType)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            var bytesRead = _stream.Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
            {
                Marshal.WriteInt32(pcbRead, bytesRead);
            }

            return HRESULT.S_OK;
        }

        public HRESULT Revert()
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            SeekOrigin seekOrigin;

            switch (dwOrigin)
            {
                case (int)STREAM_SEEK.STREAM_SEEK_SET:
                    seekOrigin = SeekOrigin.Begin;
                    break;
                case (int)STREAM_SEEK.STREAM_SEEK_CUR:
                    seekOrigin = SeekOrigin.Current;
                    break;
                case (int)STREAM_SEEK.STREAM_SEEK_END:
                    seekOrigin = SeekOrigin.End;
                    break;
                default:
                    return HRESULT.E_FAIL;
            }

            var position = _stream.Seek(dlibMove, seekOrigin);

            if (plibNewPosition != IntPtr.Zero)
            {
                Marshal.WriteInt64(plibNewPosition, position);
            }

            return HRESULT.S_OK;
        }

        public HRESULT SetSize(long libNewSize)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG
            {
                type = (int)STGTY.STGTY_STREAM,
                cbSize = _stream.Length,
                grfMode = (int)STGM.STGM_READ
            };

            if (_stream.CanRead && _stream.CanWrite)
            {
                pstatstg.grfMode |= (int)STGM.STGM_READWRITE;
            }
            else if (_stream.CanRead)
            {
                pstatstg.grfMode |= (int)STGM.STGM_READ;
            }
            else if (_stream.CanWrite)
            {
                pstatstg.grfMode |= (int)STGM.STGM_WRITE;
            }
            else
            {
                return HRESULT.E_ACCESSDENIED;
            }

            return HRESULT.S_OK;
        }

        public HRESULT UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            return HRESULT.E_NOTIMPL;
        }
    }

    public class NativeMethods
    {
        [DllImport("query.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT LoadIFilter(string pwcsPath, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref IFilter ppIUnk);
    }

    public struct FILETIME
    {
        public uint DateTimeLow;
        public uint DateTimeHigh;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FILTERREGION
    {
        public ulong idChunk;
        public ulong cwcStart;
        public ulong cwcExtent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FULLPROPSPEC
    {
        public Guid guidPropSet;
        public PROPSPEC psProperty;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PROPSPEC
    {
        [FieldOffset(0)]
        public PROPSPECKIND ulKind;

        [FieldOffset(4)]
        public uint propid;

        [FieldOffset(4)]
        public IntPtr lpwstr;
    }

    public struct STAT_CHUNK
    {
        public int idChunk;

        [MarshalAs(UnmanagedType.U4)]
        public CHUNK_BREAKTYPE breakType;

        [MarshalAs(UnmanagedType.U4)]
        public CHUNKSTATE flags;

        public int locale;

        public FULLPROPSPEC attribute;

        public int idChunkSource;

        public int cwcStartSource;

        public int cwcLenSource;
    }

    public struct STATSTG
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pwcsName;
        public int type;
        public long cbSize;
        public FILETIME mtime;
        public FILETIME ctime;
        public FILETIME atime;
        public int grfMode;
        public int grfLocksSupported;
        public Guid clsid;
        public int grfStateBits;
        public int reserved;
    }

    [Flags]
    public enum IFilterReturnCodes : uint
    {
        S_OK = 0,
        E_ACCESSDENIED = 0x80070005,
        E_HANDLE = 0x80070006,
        E_INVALIDARG = 0x80070057,
        E_OUTOFMEMORY = 0x8007000E,
        E_NOTIMPL = 0x80004001,
        E_FAIL = 0x80000008,

        FILTER_E_PASSWORD = 0x8004170B,
        FILTER_E_UNKNOWNFORMAT = 0x8004170C,
        FILTER_E_NO_TEXT = 0x80041705,
        FILTER_E_NO_VALUES = 0x80041706,
        FILTER_E_END_OF_CHUNKS = 0x80041700,
        FILTER_E_NO_MORE_TEXT = 0x80041701,
        FILTER_E_NO_MORE_VALUES = 0x80041702,
        FILTER_E_ACCESS = 0x80041703,
        FILTER_W_MONIKER_CLIPPED = 0x00041704,
        FILTER_E_EMBEDDING_UNAVAILABLE = 0x80041707,
        FILTER_E_LINK_UNAVAILABLE = 0x80041708,
        FILTER_S_LAST_TEXT = 0x00041709,
        FILTER_S_LAST_VALUES = 0x0004170A
    }

    [Flags]
    public enum CHUNK_BREAKTYPE : uint
    {
        CHUNK_NO_BREAK = 0,
        CHUNK_EOW = 1,
        CHUNK_EOS = 2,
        CHUNK_EOP = 3,
        CHUNK_EOC = 4
    }

    [Flags]
    public enum CHUNKSTATE : uint
    {
        CHUNK_TEXT = 0x1,
        CHUNK_VALUE = 0x2,
        CHUNK_FILTER_OWNED_VALUE = 0x4
    }

    [Flags]
    public enum HRESULT : uint
    {
        S_OK = 0x00000000,
        E_NOTIMPL = 0x80004001,
        E_NOINTERFACE = 0x80004002,
        E_POINTER = 0x80004003,
        E_ABORT = 0x80004004,
        E_FAIL = 0x80004005,
        E_UNEXPECTED = 0x8000FFFF,
        E_ACCESSDENIED = 0x80070005,
        E_HANDLE = 0x80070006,
        E_OUTOFMEMORY = 0x8007000E,
        E_INVALIDARG = 0x80070057
    }

    [Flags]
    public enum IFILTER_FLAGS
    {
        IFILTER_FLAGS_OLE_PROPERTIES = 1
    }

    [Flags]
    public enum IFILTER_INIT
    {
        IFILTER_INIT_CANON_PARAGRAPHS = 1,
        IFILTER_INIT_HARD_LINE_BREAKS = 2,
        IFILTER_INIT_CANON_HYPHENS = 4,
        IFILTER_INIT_CANON_SPACES = 8,
        IFILTER_INIT_APPLY_INDEX_ATTRIBUTES = 16,
        IFILTER_INIT_APPLY_CRAWL_ATTRIBUTES = 256,
        IFILTER_INIT_APPLY_OTHER_ATTRIBUTES = 32,
        IFILTER_INIT_INDEXING_ONLY = 64,
        IFILTER_INIT_SEARCH_LINKS = 128,
        IFILTER_INIT_FILTER_OWNED_VALUE_OK = 512,
        IFILTER_INIT_FILTER_AGGRESSIVE_BREAK = 1024,
        IFILTER_INIT_DISABLED_EMBEDDED = 2048,
        IFILTER_INIT_EMIT_FORMATTING = 4096
    }

    [Flags]
    public enum PROPSPECKIND : ulong
    {
        PRSPEC_LPWSTR = 0,
        PRSPEC_PROPID = 1
    }

    [Flags]
    public enum STGM : ulong
    {
        STGM_READ = 0x00000000L,
        STGM_WRITE = 0x00000001L,
        STGM_READWRITE = 0x00000002L,
        STGM_SHARE_DENY_NONE = 0x00000040L,
        STGM_SHARE_DENY_READ = 0x00000030L,
        STGM_SHARE_DENY_WRITE = 0x00000020L,
        STGM_SHARE_EXCLUSIVE = 0x00000010L,
        STGM_PRIORITY = 0x00040000L,
        STGM_CREATE = 0x00001000L,
        STGM_CONVERT = 0x00020000L,
        STGM_FAILIFTHERE = 0x00000000L,
        STGM_DIRECT = 0x00000000L,
        STGM_TRANSACTED = 0x00010000L,
        STGM_NOSCRATCH = 0x00100000L,
        STGM_NOSNAPSHOT = 0x00200000L,
        STGM_SIMPLE = 0x08000000L,
        STGM_DIRECT_SWMR = 0x00400000L,
        STGM_DELETEONRELEASE = 0x04000000L
    }

    [Flags]
    public enum STGTY : int
    {
        STGTY_STORAGE = 1,
        STGTY_STREAM = 2,
        STGTY_LOCKBYTES = 3,
        STGTY_PROPERTY = 4
    }

    [Flags]
    public enum STREAM_SEEK : int
    {
        STREAM_SEEK_SET = 0,
        STREAM_SEEK_CUR = 1,
        STREAM_SEEK_END = 2
    }
}
