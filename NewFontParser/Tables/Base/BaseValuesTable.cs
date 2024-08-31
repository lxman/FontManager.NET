using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Base.BaseCoord;

namespace NewFontParser.Tables.Base
{
    public class BaseValuesTable
    {
        public ushort DefaultBaselineIndex { get; }

        public List<IBaseCoordFormat> BaseCoordFormats { get; } = new List<IBaseCoordFormat>();

        public BaseValuesTable(BigEndianReader reader)
        {
            long position = reader.Position;

            DefaultBaselineIndex = reader.ReadUShort();
            ushort baseCoordCount = reader.ReadUShort();
            var baseCoordOffsets = new ushort[baseCoordCount];
            for (var i = 0; i < baseCoordCount; i++)
            {
                baseCoordOffsets[i] = reader.ReadUShort();
            }

            for (var i = 0; i < baseCoordCount; i++)
            {
                reader.Seek(baseCoordOffsets[i] + position);
                ushort format = reader.ReadUShort();
                IBaseCoordFormat table;
                switch (format)
                {
                    case 1:
                        BaseCoordFormats.Add(new Format1(reader));
                        break;
                    case 2:
                        BaseCoordFormats.Add(new Format2(reader));
                        break;
                    case 3:
                        BaseCoordFormats.Add(new Format3(reader));
                        break;
                }
            }
        }
    }
}
