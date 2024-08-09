//MIT, 2015-2016, Michael Popoloski, WinterDev

using System.IO;

namespace FontParser.Tables.TrueType
{
    internal class CvtTable : TableEntry
    {
        public const string _N = "cvt ";//need 4 chars//***
        public override string Name => _N;

        //

        /// <summary>
        /// control value in font unit
        /// </summary>
        internal int[] _controlValues;

        protected override void ReadContentFrom(BinaryReader reader)
        {
            var nelems = (int)(TableLength / sizeof(short));
            var results = new int[nelems];
            for (var i = 0; i < nelems; i++)
            {
                results[i] = reader.ReadInt16();
            }
            _controlValues = results;
        }
    }

    internal class PrepTable : TableEntry
    {
        public const string _N = "prep";
        public override string Name => _N;
        //

        internal byte[] _programBuffer;

        //
        protected override void ReadContentFrom(BinaryReader reader)
        {
            _programBuffer = reader.ReadBytes((int)TableLength);
        }
    }

    internal class FpgmTable : TableEntry
    {
        public const string _N = "fpgm";
        public override string Name => _N;
        //

        internal byte[] _programBuffer;

        protected override void ReadContentFrom(BinaryReader reader)
        {
            _programBuffer = reader.ReadBytes((int)TableLength);
        }
    }
}