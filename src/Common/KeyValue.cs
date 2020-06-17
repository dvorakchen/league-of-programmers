using System.Text;

namespace System.Collections.Generic
{
    public static class KeyValue
    {
        public static KeyValue<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValue<TKey, TValue>(key, value);
        }
    }


    public struct KeyValue<TKey, TValue>
    {
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder('[', 10);
            if (Key != null)
                s.Append(Key.ToString());
            if (Value != null)
                s.Append(Value.ToString());
            s.Append(']');
            return s.ToString();
        }
    }
}
