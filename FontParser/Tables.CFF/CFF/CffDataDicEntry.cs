﻿using System.Text;

namespace FontParser.Tables.CFF.CFF
{
    internal class CffDataDicEntry
    {
        public CffOperand[] operands;
        public CFFOperator _operator;

#if DEBUG

        public override string ToString()
        {
            StringBuilder stbuilder = new StringBuilder();
            int j = operands.Length;
            for (int i = 0; i < j; ++i)
            {
                if (i > 0)
                {
                    stbuilder.Append(" ");
                }
                stbuilder.Append(operands[i].ToString());
            }

            stbuilder.Append(" ");
            stbuilder.Append(_operator.ToString());
            return stbuilder.ToString();
        }

#endif
    }
}
