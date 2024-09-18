using System;
using System.Collections.Generic;
using NewFontParser.Models;
using NewFontParser.Reader;
using NewFontParser.Tables.Woff;
using NewFontParser.Tables.Woff.Zlib;

namespace NewFontParser
{
    public class WoffPreprocessor
    {
        public List<TableRecord> TableRecords { get; } = new List<TableRecord>();

        private readonly FileByteReader _reader;

        public WoffPreprocessor(FileByteReader reader, int version)
        {
            _reader = reader;
            uint flavor = reader.ReadUInt32();
            uint length = reader.ReadUInt32();
            ushort numTables = reader.ReadUInt16();
            ushort reserved = reader.ReadUInt16();
            uint totalSfntSize = reader.ReadUInt32();
            uint totalCompressedSize = version == 2 ? reader.ReadUInt32() : 0;
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            uint metaOffset = reader.ReadUInt32();
            uint metaLength = reader.ReadUInt32();
            uint privateOffset = reader.ReadUInt32();
            uint privateLength = reader.ReadUInt32();
            var directoryEntries = new List<IDirectoryEntry>();
            for (var i = 0; i < numTables; i++)
            {
                switch (version)
                {
                    case 1:
                        directoryEntries.Add(new WoffTableDirectoryEntry(reader));
                        break;
                    case 2:
                        directoryEntries.Add(new Woff2TableDirectoryEntry(reader));
                        break;
                }
            }

            directoryEntries.ForEach(d =>
            {
                var tag = string.Empty;
                byte[]? uncompressedData = Array.Empty<byte>();
                switch (d)
                {
                    case WoffTableDirectoryEntry entry:
                        if (entry.OriginalLength != entry.CompressedLength)
                        {
                            _reader.Seek(entry.Offset);
                            byte[] compressedData = _reader.ReadBytes(entry.CompressedLength);
                            uncompressedData = ZlibUtility.Inflate(compressedData);
                        }
                        else
                        {
                            uncompressedData = _reader.ReadBytes(entry.OriginalLength);
                        }
                        tag = entry.Tag;
                        break;
                    case Woff2TableDirectoryEntry entry:
                        tag = entry.Tag;
                        //if (tag != "glyf" && tag != "loca" && tag != "hmtx")
                        //{
                        //    _reader.Seek(entry.);
                        //    byte[] compressedData = _reader.ReadBytes(entry.TransformLength);
                        //    uncompressedData = ZlibUtility.Inflate(compressedData);
                        //}
                        //else
                        //{
                        //    switch (tag)
                        //    {
                        //        case "glyf":
                        //            uncompressedData = entry.GlyfTransform.Transform(_reader, entry.OriginalLength);
                        //            break;
                        //        case "loca":
                        //            uncompressedData = entry.LocaTransform.Transform(_reader, entry.OriginalLength);
                        //            break;
                        //        case "hmtx":
                        //            uncompressedData = entry.HmtxTransform.Transform(_reader, entry.OriginalLength);
                        //            break;
                        //    }
                        //}
                        uncompressedData = Array.Empty<byte>();
                        break;
                }
                TableRecords.Add(new TableRecord { Tag = tag, Data = uncompressedData });
            });
        }
    }
}
