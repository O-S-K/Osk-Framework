using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using OSK;

public class TimeExample : MonoBehaviour
{
    private Timer myTimer;

    [Button("Test Time")]
    private void Test()
    {
        // var t1 = Main.Time.Create(this);
        // t1.OnStart = () => Debug.Log("T1 Timer started.");
        // t1.OnStart?.Invoke();

        // var t2 = Main.Time.Create(this);
        // t2.OnStart = () => Logg.Log("T2 Timer started.");
        // t2.OnStart?.Invoke();
        // t2.UseFixedUpdate(false).SetTimeOut(10);
        // t2.OnTick = () =>
        // {
        //     if (t2.Second == 5)
        //     {
        //         t2.SetSpeedTime(2);
        //     }
        //     if (t2.Second == 9)
        //     {
        //         t2.SetSpeedTime(0.5f);
        //     }
        // };
        // t2.OnTick = () =>
        // {
        //     Logg.Log("t2 Timer:  " + t2.Second);
        //     if (t2.Second >= 10)
        //     {
        //         t2.Dispose();
        //     }
        // };
        // t2.OnCompleted = () => Logg.Log("T2 Timer completed.");

        var t3 = Main.Time.CreateLoops(this);
        t3.OnTick = () => Debug.Log("T3 Timer ticked.");
        t3.OnFixedTick = () => Debug.Log("T3  Fixed Timer ticked.");
        t3.OnTickPerSecond = () => Debug.Log("T3 Timer:  " + t3.Second);

        // var t4 = Main.Time.CreateLoops(this, true).UseFixedUpdate(false).SetSpeedTime(1);
        // t4.OnTickPerSecond = () =>
        // {
        //     Logg.Log("T4 Timer:  " + t4.Second);
        //     if (t4.Second >= 10)
        //     {
        //         t4.Dispose();
        //     }
        // };
        // t4.OnCompleted = () => Logg.Log("T4 Timer completed.");
        //
        // var t5 = Main.Time.CreateLoops(this, true).UseFixedUpdate(false).SetSpeedTime(2);
        // t5.OnTickPerSecond = () =>
        // {
        //     Logg.Log("T5 Timer:  " + t5.Second);
        //     if (t5.Second >= 10)
        //     {
        //         t5.Dispose();
        //     }
        // };
        // t5.OnCompleted = () => Logg.Log("T5 Timer completed.");
    }

    [Button("Clear Timers")]
    private void ClearTimers()
    {
        Main.Time.ClearAllTimers();
        Debug.Log("All timers cleared.");
    }

    [Button("Get Real Time")]
    private void GetRealTime()
    {
        Debug.Log(Main.Time.GetRealTimeWorld());
    }

    [Button("Paused Timers")]
    private void PausedTimers()
    {
        myTimer.Pause();
        Debug.Log("All timers paused.");
    }

    [Button("Resumed Timers")]
    private void ResumedTimers()
    {
        myTimer.Resume();
        Debug.Log("All timers resumed.");
    }

    [Button("Remove Timer")]
    private void RemoveTimer()
    {
        Main.Time.RemoveTimer(myTimer);
        Debug.Log("Timer removed.");
    }
}