using FontParser.Tables;

namespace FontParser.Typeface
{
    public class RestoreTicket
    {
        internal RestoreTicket()
        {
        }

        internal string TypefaceName { get; set; }

        internal TableHeader[] Headers;
        internal bool HasTtf;
        internal bool HasCff;
        internal bool HasSvg;
        internal bool HasBitmapSource;

        internal bool ControlValues;
        internal bool PrepProgramBuffer;
        internal bool FpgmProgramBuffer;
        internal bool CPALTable;
        internal bool COLRTable;
        internal bool GaspTable;
    }
}