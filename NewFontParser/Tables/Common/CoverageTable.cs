﻿using System;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Common
{
    public static class CoverageTable
    {
        public static ICoverageFormat Retrieve(BigEndianReader reader)
        {
            ushort coverageFormat = reader.PeekBytes(2)[1];
            return coverageFormat switch
            {
                1 => new CoverageFormat1(reader),
                2 => new CoverageFormat2(reader),
                _ => throw new ArgumentOutOfRangeException(nameof(coverageFormat), coverageFormat,
                    "Coverage format must be 1 or 2.")
            };
        }
    }
}