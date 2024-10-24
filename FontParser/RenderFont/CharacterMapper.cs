using System;
using FontParser.Tables.Cmap;
using FontParser.Tables.Cmap.SubTables;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace FontParser.RenderFont
{
    public class CharacterMapper
    {
        private readonly ICmapSubtable _subTable;

        public CharacterMapper(CmapTable cmapTable)
        {
            PlatformId platformId;
            OperatingSystem info = Environment.OSVersion;
            switch (info.Platform)
            {
                case PlatformID.MacOSX:
                    platformId = PlatformId.Macintosh;
                    break;
                case PlatformID.Unix:
                    platformId = PlatformId.Unicode;
                    break;
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Xbox:
                    platformId = PlatformId.Windows;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (platformId)
            {
                case PlatformId.Macintosh:
                    {
                        CmapEncoding? encoding = cmapTable.Encodings.Find(x => x.Encoding is { PlatformId: PlatformId.Macintosh, MacintoshEncoding: MacintoshEncodingId.Roman });
                        if (!(encoding is null))
                        {
                            _subTable = encoding.SubTable;
                        }

                        break;
                    }
                case PlatformId.Windows:
                    {
                        CmapEncoding? encoding = cmapTable.Encodings.Find(x => x.Encoding is { PlatformId: PlatformId.Windows, WindowsEncoding: WindowsEncodingId.UnicodeBmp });
                        if (!(encoding is null))
                        {
                            _subTable = encoding.SubTable;
                            break;
                        }
                        encoding = cmapTable.Encodings.Find(x => x.Encoding is { PlatformId: PlatformId.Windows, WindowsEncoding: WindowsEncodingId.UnicodeUCS4 });
                        if (!(encoding is null))
                        {
                            _subTable = encoding.SubTable;
                            break;
                        }
                        encoding = cmapTable.Encodings.Find(x => x.Encoding is { PlatformId: PlatformId.Windows, WindowsEncoding: WindowsEncodingId.UnicodeCsm });
                        if (!(encoding is null))
                        {
                            _subTable = encoding.SubTable;
                            break;
                        }
                        encoding = cmapTable.Encodings.Find(x => x.Encoding is { PlatformId: PlatformId.Unicode, UnicodeEncoding: UnicodeEncodingId.Unicode20 });
                        if (!(encoding is null))
                        {
                            _subTable = encoding.SubTable;
                        }

                        break;
                    }
                case PlatformId.Unicode:
                    // TBD
                    break;
                case PlatformId.Iso:
                    // TBD
                    break;
                case PlatformId.Custom:
                    // TBD
                    break;
                default:
                    break;
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return _subTable.GetGlyphId(codePoint);
        }
    }
}
