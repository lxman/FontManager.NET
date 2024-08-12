using System.Text;

namespace FontParser.Tables.CFF.CFF
{
    internal class CffDataDicEntry
    {
        public CffOperand[] operands;
        public CFFOperator _operator;

#if DEBUG

        public override string ToString()
        {
            var stbuilder = new StringBuilder();
            int j = operands.Length;
            for (var i = 0; i < j; ++i)
            {
                if (i > 0)
                {
                    stbuilder.Append(" ");
                }
                stbuilder.Append(operands[i].ToString());
            }

            stbuilder.Append(" ");
            stbuilder.Append(_operator);
            return stbuilder.ToString();
        }

#endif
    }
}
