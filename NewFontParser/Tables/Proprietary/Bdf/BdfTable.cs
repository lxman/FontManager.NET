﻿namespace NewFontParser.Tables.Proprietary.Bdf
{
    // This is a proprietary table.
    // I am not able to find any information on this table.
    public class BdfTable : IInfoTable
    {
        public static string Tag => "BDF ";

        public BdfTable(byte[] data)
        {
        }
    }
}