using Kaitai;

namespace KaitaiTtf.Os2
{
    public class CodePageRange : KaitaiStruct
    {
        public static CodePageRange FromFile(string fileName)
        {
            return new CodePageRange(new KaitaiStream(fileName));
        }

        public CodePageRange(KaitaiStream p__io, Os2 p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _symbolCharacterSet = m_io.ReadBitsIntBe(1) != 0;
            _oemCharacterSet = m_io.ReadBitsIntBe(1) != 0;
            _macintoshCharacterSet = m_io.ReadBitsIntBe(1) != 0;
            _reservedForAlternateAnsiOem = m_io.ReadBitsIntBe(7);
            _cp1361KoreanJohab = m_io.ReadBitsIntBe(1) != 0;
            _cp950ChineseTraditionalCharsTaiwanAndHongKong = m_io.ReadBitsIntBe(1) != 0;
            _cp949KoreanWansung = m_io.ReadBitsIntBe(1) != 0;
            _cp936ChineseSimplifiedCharsPrcAndSingapore = m_io.ReadBitsIntBe(1) != 0;
            _cp932JisJapan = m_io.ReadBitsIntBe(1) != 0;
            _cp874Thai = m_io.ReadBitsIntBe(1) != 0;
            _reservedForAlternateAnsi = m_io.ReadBitsIntBe(8);
            _cp1257WindowsBaltic = m_io.ReadBitsIntBe(1) != 0;
            _cp1256Arabic = m_io.ReadBitsIntBe(1) != 0;
            _cp1255Hebrew = m_io.ReadBitsIntBe(1) != 0;
            _cp1254Turkish = m_io.ReadBitsIntBe(1) != 0;
            _cp1253Greek = m_io.ReadBitsIntBe(1) != 0;
            _cp1251Cyrillic = m_io.ReadBitsIntBe(1) != 0;
            _cp1250Latin2EasternEurope = m_io.ReadBitsIntBe(1) != 0;
            _cp1252Latin1 = m_io.ReadBitsIntBe(1) != 0;
            _cp437Us = m_io.ReadBitsIntBe(1) != 0;
            _cp850WeLatin1 = m_io.ReadBitsIntBe(1) != 0;
            _cp708ArabicAsmo708 = m_io.ReadBitsIntBe(1) != 0;
            _cp737GreekFormer437G = m_io.ReadBitsIntBe(1) != 0;
            _cp775MsDosBaltic = m_io.ReadBitsIntBe(1) != 0;
            _cp852Latin2 = m_io.ReadBitsIntBe(1) != 0;
            _cp855IbmCyrillicPrimarilyRussian = m_io.ReadBitsIntBe(1) != 0;
            _cp857IbmTurkish = m_io.ReadBitsIntBe(1) != 0;
            _cp860MsDosPortuguese = m_io.ReadBitsIntBe(1) != 0;
            _cp861MsDosIcelandic = m_io.ReadBitsIntBe(1) != 0;
            _cp862Hebrew = m_io.ReadBitsIntBe(1) != 0;
            _cp863MsDosCanadianFrench = m_io.ReadBitsIntBe(1) != 0;
            _cp864Arabic = m_io.ReadBitsIntBe(1) != 0;
            _cp865MsDosNordic = m_io.ReadBitsIntBe(1) != 0;
            _cp866MsDosRussian = m_io.ReadBitsIntBe(1) != 0;
            _cp869IbmGreek = m_io.ReadBitsIntBe(1) != 0;
            _reservedForOem = m_io.ReadBitsIntBe(16);
        }
        private bool _symbolCharacterSet;
        private bool _oemCharacterSet;
        private bool _macintoshCharacterSet;
        private ulong _reservedForAlternateAnsiOem;
        private bool _cp1361KoreanJohab;
        private bool _cp950ChineseTraditionalCharsTaiwanAndHongKong;
        private bool _cp949KoreanWansung;
        private bool _cp936ChineseSimplifiedCharsPrcAndSingapore;
        private bool _cp932JisJapan;
        private bool _cp874Thai;
        private ulong _reservedForAlternateAnsi;
        private bool _cp1257WindowsBaltic;
        private bool _cp1256Arabic;
        private bool _cp1255Hebrew;
        private bool _cp1254Turkish;
        private bool _cp1253Greek;
        private bool _cp1251Cyrillic;
        private bool _cp1250Latin2EasternEurope;
        private bool _cp1252Latin1;
        private bool _cp437Us;
        private bool _cp850WeLatin1;
        private bool _cp708ArabicAsmo708;
        private bool _cp737GreekFormer437G;
        private bool _cp775MsDosBaltic;
        private bool _cp852Latin2;
        private bool _cp855IbmCyrillicPrimarilyRussian;
        private bool _cp857IbmTurkish;
        private bool _cp860MsDosPortuguese;
        private bool _cp861MsDosIcelandic;
        private bool _cp862Hebrew;
        private bool _cp863MsDosCanadianFrench;
        private bool _cp864Arabic;
        private bool _cp865MsDosNordic;
        private bool _cp866MsDosRussian;
        private bool _cp869IbmGreek;
        private ulong _reservedForOem;
        private Ttf m_root;
        private Os2 m_parent;
        public bool SymbolCharacterSet => _symbolCharacterSet;
        public bool OemCharacterSet => _oemCharacterSet;
        public bool MacintoshCharacterSet => _macintoshCharacterSet;
        public ulong ReservedForAlternateAnsiOem => _reservedForAlternateAnsiOem;
        public bool Cp1361KoreanJohab => _cp1361KoreanJohab;
        public bool Cp950ChineseTraditionalCharsTaiwanAndHongKong => _cp950ChineseTraditionalCharsTaiwanAndHongKong;
        public bool Cp949KoreanWansung => _cp949KoreanWansung;
        public bool Cp936ChineseSimplifiedCharsPrcAndSingapore => _cp936ChineseSimplifiedCharsPrcAndSingapore;
        public bool Cp932JisJapan => _cp932JisJapan;
        public bool Cp874Thai => _cp874Thai;
        public ulong ReservedForAlternateAnsi => _reservedForAlternateAnsi;
        public bool Cp1257WindowsBaltic => _cp1257WindowsBaltic;
        public bool Cp1256Arabic => _cp1256Arabic;
        public bool Cp1255Hebrew => _cp1255Hebrew;
        public bool Cp1254Turkish => _cp1254Turkish;
        public bool Cp1253Greek => _cp1253Greek;
        public bool Cp1251Cyrillic => _cp1251Cyrillic;
        public bool Cp1250Latin2EasternEurope => _cp1250Latin2EasternEurope;
        public bool Cp1252Latin1 => _cp1252Latin1;
        public bool Cp437Us => _cp437Us;
        public bool Cp850WeLatin1 => _cp850WeLatin1;
        public bool Cp708ArabicAsmo708 => _cp708ArabicAsmo708;
        public bool Cp737GreekFormer437G => _cp737GreekFormer437G;
        public bool Cp775MsDosBaltic => _cp775MsDosBaltic;
        public bool Cp852Latin2 => _cp852Latin2;
        public bool Cp855IbmCyrillicPrimarilyRussian => _cp855IbmCyrillicPrimarilyRussian;
        public bool Cp857IbmTurkish => _cp857IbmTurkish;
        public bool Cp860MsDosPortuguese => _cp860MsDosPortuguese;
        public bool Cp861MsDosIcelandic => _cp861MsDosIcelandic;
        public bool Cp862Hebrew => _cp862Hebrew;
        public bool Cp863MsDosCanadianFrench => _cp863MsDosCanadianFrench;
        public bool Cp864Arabic => _cp864Arabic;
        public bool Cp865MsDosNordic => _cp865MsDosNordic;
        public bool Cp866MsDosRussian => _cp866MsDosRussian;
        public bool Cp869IbmGreek => _cp869IbmGreek;
        public ulong ReservedForOem => _reservedForOem;
        public Ttf M_Root => m_root;
        public Os2 M_Parent => m_parent;
    }
}
