using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class SimpleGlyph : IGlyphSpec
    {
        public ushort[] EndPtsOfContours { get; }

        public ushort InstructionLength { get; }

        public byte[] Instructions { get; }

        public SimpleGlyphFlags[] Flags { get; }

        public short[] XCoordinates { get; }

        public short[] YCoordinates { get; }

        public SimpleGlyph(BigEndianReader reader, GlyphHeader glyphHeader)
        {
            EndPtsOfContours = new ushort[glyphHeader.NumberOfContours];
            for (var i = 0; i < glyphHeader.NumberOfContours; i++)
            {
                EndPtsOfContours[i] = reader.ReadUShort();
            }

            InstructionLength = reader.ReadUShort();
            Instructions = reader.ReadBytes(InstructionLength);

            int numberOfPoints = EndPtsOfContours[glyphHeader.NumberOfContours - 1] + 1;
            Flags = new SimpleGlyphFlags[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                Flags[i] = (SimpleGlyphFlags)reader.ReadByte();
                if (!Flags[i].HasFlag(SimpleGlyphFlags.Repeat)) continue;
                byte repeat = reader.ReadByte();
                for (var j = 0; j < repeat; j++)
                {
                    i++;
                    if (i >= numberOfPoints)
                    {
                        break;
                    }
                    Flags[i] = Flags[i - 1];
                }
            }

            XCoordinates = new short[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                if (Flags[i].HasFlag(SimpleGlyphFlags.XShortVector))
                {
                    if (Flags[i].HasFlag(SimpleGlyphFlags.XIsSameOrPositiveXShortVector))
                    {
                        XCoordinates[i] = reader.ReadByte();
                    }
                    else
                    {
                        XCoordinates[i] = Convert.ToInt16(-reader.ReadByte());
                    }
                }
                else if (Flags[i].HasFlag(SimpleGlyphFlags.XIsSameOrPositiveXShortVector))
                {
                    XCoordinates[i] = XCoordinates[i - 1];
                }
                else
                {
                    XCoordinates[i] = reader.ReadShort();
                }
            }

            YCoordinates = new short[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                if (Flags[i].HasFlag(SimpleGlyphFlags.YShortVector))
                {
                    if (Flags[i].HasFlag(SimpleGlyphFlags.YIsSameOrPositiveYShortVector))
                    {
                        YCoordinates[i] = reader.ReadByte();
                    }
                    else
                    {
                        YCoordinates[i] = Convert.ToInt16(-reader.ReadByte());
                    }
                }
                else if (Flags[i].HasFlag(SimpleGlyphFlags.YIsSameOrPositiveYShortVector))
                {
                    YCoordinates[i] = YCoordinates[i - 1];
                }
                else
                {
                    YCoordinates[i] = reader.ReadShort();
                }
            }
        }
    }
}
