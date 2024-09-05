using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class LayerList
    {
        public List<IPaintTable> Layers { get; } = new List<IPaintTable>();

        public LayerList(BigEndianReader reader)
        {
            uint layerCount = reader.ReadUInt32();
            var layerOffsets = new uint[layerCount];
            for (var i = 0; i < layerCount; i++)
            {
                layerOffsets[i] = reader.ReadUInt32();
            }
            for (var i = 0; i < layerCount; i++)
            {
                Layers.Add(PaintTableFactory.CreatePaintTable(reader, layerOffsets[i]));
            }
        }
    }
}