using System.Text;
using NewFontParser.Reader;
using NewFontParser.Tables.Base.BaseCoord;

namespace NewFontParser.Tables.Base
{
    public class FeatMinMaxRecord
    {
        public string FeatureTag { get; }

        public IBaseCoordFormat? MinCoord { get; }

        public IBaseCoordFormat? MaxCoord { get; }

        public FeatMinMaxRecord(BigEndianReader reader, long origin)
        {
            FeatureTag = Encoding.UTF8.GetString(reader.ReadBytes(4));
            ushort minCoordOffset = reader.ReadUShort();
            ushort maxCoordOffset = reader.ReadUShort();
            reader.Seek(minCoordOffset + origin);
            ushort minFormat = reader.ReadUShort();
            MinCoord = minFormat switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                3 => new Format3(reader),
                _ => MinCoord
            };
            reader.Seek(maxCoordOffset + origin);
            ushort maxFormat = reader.ReadUShort();
            MaxCoord = maxFormat switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                3 => new Format3(reader),
                _ => MaxCoord
            };
        }
    }
}