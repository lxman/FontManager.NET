using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;
using NewFontParser.Tables.Bitmap.Common.GlyphBitmapData;
using NewFontParser.Tables.Bitmap.Common.IndexSubtables;
using NewFontParser.Tables.Bitmap.Eblc;

namespace NewFontParser.Tables.Bitmap.Ebdt
{
    public class EbdtTable : IInfoTable
    {
        public static string Tag => "EBDT";

        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public List<GlyphObject> BitmapData { get; } = new List<GlyphObject>();

        private readonly BigEndianReader _reader;

        public EbdtTable(byte[] bytes)
        {
            var reader = new BigEndianReader(bytes);
            _reader = reader;
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
        }

        public void Process(EblcTable eblcTable)
        {
            eblcTable.BitmapSizes.ForEach(bs =>
            {
                bs.IndexSubtableList.IndexSubtables.ForEach(table =>
                {
                    return;
                    // TODO: Implement this
                    // Not working correctly
                    ushort firstGlyph = table.FirstGlyphIndex;
                    ushort lastGlyph = table.LastGlyphIndex;
                    switch (table.Subtable)
                    {
                        case Common.IndexSubtables.Format1 subtable:
                            break;
                        case Common.IndexSubtables.Format2 subtable:
                            break;
                        case Common.IndexSubtables.Format3 subtable:
                            break;
                        case Common.IndexSubtables.Format4 subtable:
                            {
                                ushort imageFormat = subtable.ImageFormat;
                                uint imageDataOffset = subtable.ImageDataOffset;
                                _reader.Seek(imageDataOffset);
                                for (var i = 0; i < subtable.GlyphIdOffsetPairs.Count - 1; i++)
                                {
                                    GlyphIdOffsetPair? op = subtable.GlyphIdOffsetPairs[i];
                                    var dataSize = Convert.ToUInt32(subtable.GlyphIdOffsetPairs[i + 1].Offset - op.Offset);
                                    if (dataSize == 0)
                                    {
                                        continue;
                                    }
                                    switch (imageFormat)
                                    {
                                        case 1:
                                            _reader.Seek(imageDataOffset + op.Offset);
                                            BitmapData.Add(new GlyphObject(op.GlyphId, new Common.GlyphBitmapData.Format1(_reader, dataSize)));
                                            break;
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                            _reader.Seek(imageDataOffset + op.Offset);
                                            BitmapData.Add(new GlyphObject(Convert.ToUInt16(op.GlyphId + firstGlyph), new Format7(_reader, dataSize)));
                                            break;
                                        case 8:
                                        case 9:
                                            break;
                                    }
                                }

                                break;
                            }
                        case Common.IndexSubtables.Format5 subtable:
                            break;
                    }
                });
            });
        }
    }
}