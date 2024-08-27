using System;

public static class ActionExtentions
{
    public static void SafeInvoke(this Action invocationTarget)
    {
        if (null != invocationTarget)
        {
            invocationTarget.Invoke();
        }
    }

    public static void SafeInvoke<T>(this Action<T> invocationTarget, T arg)
    {
        if (null != invocationTarget)
        {
            invocationTarget.Invoke(arg);
        }
    }

    public static void SafeInvoke<T1, T2>(this Action<T1, T2> invocationTarget, T1 arg1, T2 arg2)
    {
        if (null != invocationTarget)
        {
            invocationTarget.Invoke(arg1, arg2);
        }
    }

    public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> invocationTarget, T1 arg1, T2 arg2, T3 arg3)
    {
        if (null != invocationTarget)
        {
            invocationTarget.Invoke(arg1, arg2, arg3);
        }
    }

    public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> invocationTarget, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (null != invocationTarget)
        {
            invocationTarget.Invoke(arg1, arg2, arg3, arg4);
        }
    }

    public static void SafeInvoke(this Action invocationTarget, Action<Exception> exceptionAction)
    {
        if (null != invocationTarget)
        {
            try
            {
                invocationTarget.Invoke();
            }
            catch (Exception exception)
            {
                exceptionAction(exception);
            }
        }
    }

    public static void SafeInvoke<T>(this Action<T> invocationTarget, T arg, Action<Exception> exceptionAction)
    {
        if (null != invocationTarget)
        {
            try
            {
                invocationTarget.Invoke(arg);
            }
            catch (Exception exception)
            {
                exceptionAction(exception);
            }
        }
    }

    public static void SafeInvoke<T1, T2>(this Action<T1, T2> invocationTarget, T1 arg1, T2 arg2, Action<Exception> exceptionAction)
    {
        if (null != invocationTarget)
        {
            try
            {
                invocationTarget.Invoke(arg1, arg2);
            }
            catch (Exception exception)
            {
                exceptionAction(exception);
            }
        }
    }

    public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> invocationTarget, T1 arg1, T2 arg2, T3 arg3, Action<Exception> exceptionAction)
    {
        if (null != invocationTarget)
        {
            try
            {
                invocationTarget.Invoke(arg1, arg2, arg3);
            }
            catch (Exception exception)
            {
                exceptionAction(exception);
            }
        }
    }
}