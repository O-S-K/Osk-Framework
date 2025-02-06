using UnityEngine;
using UnityEngine.Profiling;

namespace OSK
{
    public class FPSCounter : MonoBehaviour
    {
        private struct AverageFloat
        {
            public float Average => _count > 0 ? _value / _count : 0;

            private float _value;
            private int _count;

            public void Add(float number)
            {
                _value += number;
                _count++;
            }

            public void Reset()
            {
                _value = 0f;
                _count = 0;
            }
        }

        [SerializeField] private float _averageValueCollectionTime = 2f;
        [SerializeField] private float _refreshFrequency = 0.4f;
        [SerializeField] private Color _textColor = Color.green;
        [SerializeField] private Color _textColorAvg = Color.yellow;
        [SerializeField] private Color _textColorStatus = Color.blue;

        [SerializeField] private int _fontSize = 20;
        [SerializeField] private int _fontSizeStatus = 17;

        [SerializeField] private Vector2 _position = new Vector2(10, 10);

        private float _timeSinceFpsReset = 0f;
        private float _timeSinceUpdate = 0f;
        private AverageFloat _fpsValue;
        private int _fps;

        private GUIStyle _styleFps;
        private GUIStyle _styleAvg;
        private GUIStyle _styleStatus;
        
        public bool isShowMemoryStatus = true;
        

        private void Start()
        {
            _styleFps = new GUIStyle
            {
                fontSize = _fontSize,
                normal = new GUIStyleState { textColor = _textColor }
            };
            _styleAvg = new GUIStyle
            {
                fontSize = _fontSize - 4,
                normal = new GUIStyleState { textColor = _textColorAvg }
            };
            
            _styleStatus = new GUIStyle
            {
                fontSize = _fontSizeStatus,
                normal = new GUIStyleState { textColor = _textColorStatus }
            };
        }

        private void Update()
        {
            if (_timeSinceFpsReset > _averageValueCollectionTime)
            {
                _timeSinceFpsReset = 0;
                _fpsValue.Reset();
            }

            _fpsValue.Add(1f / Time.unscaledDeltaTime);
            _timeSinceFpsReset += Time.deltaTime;

            if (_timeSinceUpdate < _refreshFrequency)
            {
                _timeSinceUpdate += Time.deltaTime;
                return;
            }

            _fps = Mathf.RoundToInt(_fpsValue.Average);
            _timeSinceUpdate = 0f;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(_position.x, _position.y, 100, 30), $"FPS: {_fps}", _styleFps);
            GUI.Label(new Rect(_position.x, _position.y + 25, 100, 30), $"FPS AVG: {_fpsValue.Average:0.0}", _styleAvg);
            
            if(isShowMemoryStatus)
                DrawMemoryStatus();
        }
        
        private void DrawMemoryStatus()
        {
            var totalMemory = SystemInfo.systemMemorySize; // Tổng RAM (MB)
            long allocatedMemory = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024); // RAM đã dùng (MB)
            long reservedMemory = Profiler.GetTotalReservedMemoryLong() / (1024 * 1024); // RAM dự trữ (MB)
            long monoUsedMemory = Profiler.GetMonoUsedSizeLong() / (1024 * 1024); // RAM dùng cho Mono (MB)
            long freeMemory = totalMemory - allocatedMemory; // RAM còn trống (MB)
            
            GUI.Label(new Rect(_position.x, _position.y + 50, 100, 30), $"Memory: {allocatedMemory} MB / {totalMemory} MB", _styleStatus);
            GUI.Label(new Rect(_position.x, _position.y + 125, 100, 30), $"Free: {freeMemory} MB", _styleStatus);
            GUI.Label(new Rect(_position.x, _position.y + 75, 100, 30), $"Mono: {monoUsedMemory} MB", _styleStatus);
            GUI.Label(new Rect(_position.x, _position.y + 100, 100, 30), $"Reserved: {reservedMemory} MB", _styleStatus);   
        }
    }
}