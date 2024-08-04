namespace FontParser.Tables.CFF.CFF
{
    public readonly struct CffOperand
    {
        public readonly OperandKind _kind;
        public readonly double _realNumValue;

        public CffOperand(double number, OperandKind kind)
        {
            _kind = kind;
            _realNumValue = number;
        }

#if DEBUG

        public override string ToString()
        {
            switch (_kind)
            {
                case OperandKind.IntNumber:
                    return ((int)_realNumValue).ToString();

                default:
                    return _realNumValue.ToString();
            }
        }

#endif
    }
}
