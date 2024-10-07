
namespace OSK
{
    public class ObjectPoolContainer<T>
    {
        private T item;

        public bool Used { get; private set; }

        public void Consume()
        {
            Used = true;
        }

        public T Item
        {
            get => item;
            set => item = value;
        }

        public void Release()
        {
            Used = false;
        }
    }
}
