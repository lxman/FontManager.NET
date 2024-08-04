namespace FontParser
{
    internal static class KnownFontFiles
    {
        public static bool IsTtcf(ushort u1, ushort u2)
        {
            //https://docs.microsoft.com/en-us/typography/opentype/spec/otff#ttc-header
            //check if 1st 4 bytes is ttcf or not
            return (((u1 >> 8) & 0xff) == (byte)'t') &&
                   (((u1) & 0xff) == (byte)'t') &&
                   (((u2 >> 8) & 0xff) == (byte)'c') &&
                   (((u2) & 0xff) == (byte)'f');
        }

        public static bool IsWoff(ushort u1, ushort u2)
        {
            return (((u1 >> 8) & 0xff) == (byte)'w') && //0x77
                   (((u1) & 0xff) == (byte)'O') && //0x4f
                   (((u2 >> 8) & 0xff) == (byte)'F') && // 0x46
                   (((u2) & 0xff) == (byte)'F'); //0x46
        }

        public static bool IsWoff2(ushort u1, ushort u2)
        {
            return (((u1 >> 8) & 0xff) == (byte)'w') &&//0x77
                   (((u1) & 0xff) == (byte)'O') &&  //0x4f
                   (((u2 >> 8) & 0xff) == (byte)'F') && //0x46
                   (((u2) & 0xff) == (byte)'2'); //0x32
        }
    }
}