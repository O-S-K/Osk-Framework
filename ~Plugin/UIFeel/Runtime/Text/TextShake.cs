using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Text))]
    public class TextShake : BaseMeshEffect
    {
        [SerializeField, Range(0, 2f)] private float _power;
        [SerializeField, Range(0, 0.1f)] private float _updateInterval;

        private UIVertex _vertex;
        private Vector3 _vector3;
        private Vector3 _vector3Offset;
        private readonly List<UIVertex> _vertexList = new List<UIVertex>();
        private float _time;

        private void Update()
        {
            if (_updateInterval <= 0f)
            {
                graphic.SetVerticesDirty();
                return;
            }

            if (_time < _updateInterval)
            {
                _time += Time.deltaTime;
                return;
            }

            _time -= _updateInterval;
            graphic.SetVerticesDirty();
        }

        public override void ModifyMesh(VertexHelper vertex)
        {
            _vertexList.Clear();
            vertex.GetUIVertexStream(_vertexList);
            var count = _vertexList.Count;
            for (var i = 0; i < count; i += 6)
            {
                SetOffset();
                SetPosition(i);
                SetPosition(i + 5);

                SetOffset();
                SetPosition(i + 2);
                SetPosition(i + 3);

                SetOffset();
                SetPosition(i + 1);

                SetOffset();
                SetPosition(i + 4);
            }

            vertex.Clear();
            vertex.AddUIVertexTriangleStream(_vertexList);
        }

        private void SetOffset()
        {
            _vector3Offset.x = Random.Range(-_power, _power);
            _vector3Offset.y = Random.Range(-_power, _power);
        }

        private void SetPosition(int index)
        {
            _vertex = _vertexList[index];
            _vector3 = _vertex.position + _vector3Offset;
            _vertex.position = _vector3;
            _vertexList[index] = _vertex;
        }
    }
}