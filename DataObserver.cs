namespace KarpikQuests
{
    public class DataObserver<T>
    {
        unsafe private T* _value;
        public T Value
        {
            get
            {
                T value;
                unsafe
                {
                    value = *_value;
                }
                return value;
            }
        }

        public DataObserver(ref T value)
        {
            unsafe
            {
                fixed (T* ptr = &value)
                {
                    _value = ptr;
                }

            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}