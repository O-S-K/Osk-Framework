using System.Collections.Generic;

namespace OSK
{
    public class UpdateContainer<T>
    {
        private readonly List<T> _updates = new();
        public IReadOnlyList<T> GetUpdates() => _updates;

        public void Add(T item)
        {
            if (!_updates.Contains(item))
                _updates.Add(item);
        }

        public void Remove(T item)
        {
            _updates.Remove(item);
        }

        public void Execute(System.Action<T> action)
        {
            foreach (var item in _updates)
            {
                action?.Invoke(item);
            }
        }
    }
}
