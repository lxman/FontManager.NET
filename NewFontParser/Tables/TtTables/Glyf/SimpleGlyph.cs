using System;
using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class SimpleGlyph : IGlyphSpec
    {
        public List<SimpleGlyphCoordinate> Coordinates { get; } = new List<SimpleGlyphCoordinate>();

        public ushort[] EndPtsOfContours { get; }

        public ushort InstructionLength { get; }

        public byte[] Instructions { get; }

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
            var flags = new SimpleGlyphFlags[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                flags[i] = (SimpleGlyphFlags)reader.ReadByte();
                if (!flags[i].HasFlag(SimpleGlyphFlags.Repeat)) continue;
                byte repeat = reader.ReadByte();
                for (var j = 0; j < repeat; j++)
                {
                    i++;
                    if (i >= numberOfPoints)
                    {
                        break;
                    }
                    flags[i] = flags[i - 1];
                }
            }

            var xCoordinates = new short[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                if (flags[i].HasFlag(SimpleGlyphFlags.XShortVector))
                {
                    if (flags[i].HasFlag(SimpleGlyphFlags.XIsSameOrPositiveXShortVector))
                    {
                        xCoordinates[i] = Convert.ToInt16(reader.ReadByte() + (i > 0 ? xCoordinates[i - 1] : 0));
                    }
                    else
                    {
                        xCoordinates[i] = Convert.ToInt16(-reader.ReadByte() + (i > 0 ? xCoordinates[i - 1] : 0));
                    }
                }
                else if (flags[i].HasFlag(SimpleGlyphFlags.XIsSameOrPositiveXShortVector))
                {
                    xCoordinates[i] = Convert.ToInt16(i > 0 ? xCoordinates[i - 1] : 0);
                }
                else
                {
                    xCoordinates[i] = Convert.ToInt16(reader.ReadShort() + (i > 0 ? xCoordinates[i - 1] : 0));
                }
            }

            var yCoordinates = new short[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                if (flags[i].HasFlag(SimpleGlyphFlags.YShortVector))
                {
                    if (flags[i].HasFlag(SimpleGlyphFlags.YIsSameOrPositiveYShortVector))
                    {
                        yCoordinates[i] = Convert.ToInt16(reader.ReadByte() + (i > 0 ? yCoordinates[i - 1] : 0));
                    }
                    else
                    {
                        yCoordinates[i] = Convert.ToInt16(-reader.ReadByte() + (i > 0 ? yCoordinates[i - 1] : 0));
                    }
                }
                else if (flags[i].HasFlag(SimpleGlyphFlags.YIsSameOrPositiveYShortVector))
                {
                    yCoordinates[i] = Convert.ToInt16(i > 0 ? xCoordinates[i - 1] : 0);
                }
                else
                {
                    yCoordinates[i] = Convert.ToInt16(reader.ReadShort() + (i > 0 ? yCoordinates[i - 1] : 0));
                }
            }

            for (var i = 0; i < numberOfPoints; i++)
            {
                Coordinates.Add(new SimpleGlyphCoordinate(new Point(xCoordinates[i], yCoordinates[i]), flags[i].HasFlag(SimpleGlyphFlags.OnCurve)));
            }
        }
    }
}
