using UnityEngine;
using UnityEngine.Profiling;

namespace OSK
{
    [System.Serializable]
    public class PerforInfo
    {
        public string label;
        public long startTime;
        
        public PerforInfo(string label, long startTime)
        { 
            this.label = label;
            this.startTime = startTime;
        }
        
        
        public void StartTest(string label)
        {
            Profiler.BeginSample(label);
            startTime = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public void StopTest()
        {
            long endTime = System.Diagnostics.Stopwatch.GetTimestamp();
            Profiler.EndSample();
            double elapsedTime = (endTime - startTime) * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            //Debug.Log($"{startTime} took {elapsedTime:F2} ms");
        }
    }
    
    public class Performance : GameFrameworkComponent
    {
        private string _label;
        private long _startTime;
        private PerforInfo info;
        
        public void StartTest(string label)
        {
            info = new PerforInfo(label, _startTime);
            info.StartTest(label);
        }
        
        public void StopTest()
        {
            info.StopTest();
        }

    }
}