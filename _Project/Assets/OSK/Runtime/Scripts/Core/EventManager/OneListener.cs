using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseOneListener<T> : IEnumerable<T> where T : class
{
    protected readonly List<T> _list = new List<T>();
    protected int _count;

    public void Add(T action)
    {
        var index = _list.IndexOf(action);
        if (index == -1)
        {
            _list.Add(action);
            _count++;
        }
        else
        {
            if (_count == 1) return;
            _list[index] = null;
            _list.Add(action);
        }
    }

    public void Remove(T action)
    {
        var index = _list.IndexOf(action);
        if (index != -1)
        {
            _list[index] = null;
            _count--;
        }
    }

    public void RemoveAll()
    {
        _list.Clear();
        _count = 0;
    }

    public bool Contains(T action)
    {
        return _list.Contains(action);
    }

    public int Count { get { return _count; } }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}

public sealed class OneListener : BaseOneListener<Action>
{
    public void Invoke()
    {
        if (_count == 0)
            return;

        int length = _list.Count;
        for (int i = 0; i < Math.Min(length, _list.Count); i++)
        {
            var current = _list[i];
            if (current != null)
            {
                current.Invoke();
            }
        }

        if (_count == _list.Count)
            return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            if (null == _list[i])
            {
                _list.RemoveAt(i);
            }
        }
    }
}

public sealed class OneListener<T> : BaseOneListener<Action<T>>
{
    public void Invoke(T value)
    {
        if (_count == 0)
            return;

        int length = _list.Count;
        for (int i = 0; i < Math.Min(length, _list.Count); i++)
        {
            var current = _list[i];
            if (current != null)
            {
                current.Invoke(value);
            }
        }

        if (_count == _list.Count)
            return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            if (null == _list[i])
            {
                _list.RemoveAt(i);
            }
        }
    }
}

public sealed class OneListener<T1, T2> : BaseOneListener<Action<T1, T2>>
{
    public void Invoke(T1 value1, T2 value2)
    {
        if (_count == 0)
            return;

        int length = _list.Count;
        for (int i = 0; i < Math.Min(length, _list.Count); i++)
        {
            var current = _list[i];
            if (current != null)
            {
                current.Invoke(value1, value2);
            }
        }

        if (_count == _list.Count)
            return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            if (null == _list[i])
            {
                _list.RemoveAt(i);
            }
        }
    }
}

public sealed class OneListener<T1, T2, T3> : BaseOneListener<Action<T1, T2, T3>>
{
    public void Invoke(T1 value1, T2 value2, T3 value3)
    {
        if (_count == 0)
            return;

        int length = _list.Count;
        for (int i = 0; i < Math.Min(length, _list.Count); i++)
        {
            var current = _list[i];
            if (current != null)
            {
                current.Invoke(value1, value2, value3);
            }
        }

        if (_count == _list.Count)
            return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            if (null == _list[i])
            {
                _list.RemoveAt(i);
            }
        }
    }
}

public sealed class OneListener<T1, T2, T3, T4> : BaseOneListener<Action<T1, T2, T3, T4>>
{
    public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        if (_count == 0)
            return;

        int length = _list.Count;
        for (int i = 0; i < Math.Min(length, _list.Count); i++)
        {
            var current = _list[i];
            if (current != null)
            {
                current.Invoke(value1, value2, value3, value4);
            }
        }

        if (_count == _list.Count)
            return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            if (null == _list[i])
            {
                _list.RemoveAt(i);
            }
        }
    }
}