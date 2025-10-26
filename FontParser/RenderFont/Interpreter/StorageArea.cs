using System;

namespace FontParser.RenderFont.Interpreter
{
    public class StorageArea
    {
        private readonly SAItem[] _data;

        public StorageArea(int length)
        {
            _data = new SAItem[length];
            for (var i = 0; i < length; i++)
            {
                _data[i] = new SAItem();
            }
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= _data.Length || !_data[index].Written)
                {
                    throw new IndexOutOfRangeException();
                }
                return _data[index].Value;
            }
            set
            {
                if (index < 0 || index >= _data.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                _data[index].Written = true;
                _data[index].Value = value;
            }
        }
    }
}