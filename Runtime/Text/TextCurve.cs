using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    // require unity text normal
    public class TextCurve : BaseMeshEffect
    {
        [SerializeField] private AnimationCurve _animationCurve = new AnimationCurve();
        [SerializeField] private float _power = 10f;
        [SerializeField] private bool _isEachText;

        private List<UIVertex> _vertexList = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper helper)
        {
            _vertexList.Clear();
            helper.GetUIVertexStream(_vertexList);

            var minMaxX = new Vector2(_vertexList[0].position.x, _vertexList[0].position.x);
            foreach (var vertex in _vertexList)
            {
                if (minMaxX[0] > vertex.position.x)
                    minMaxX[0] = vertex.position.x;
                if (minMaxX[1] < vertex.position.x)
                    minMaxX[1] = vertex.position.x;
            }

            for (var i = 0; i < _vertexList.Count; i += 6)
            {
                if (_isEachText)
                {
                    var center = 0f;
                    for (var j = 0; j < 6; j++)
                    {
                        center += _vertexList[i + j].position.x;
                    }

                    center /= 6;
                    var t = Mathf.InverseLerp(minMaxX[0], minMaxX[1], center);
                    var value = _animationCurve.Evaluate(t);
                    for (var j = 0; j < 6; j++)
                    {
                        var vertex = _vertexList[i + j];
                        var pos = vertex.position;
                        pos.y += value * _power;
                        vertex.position = pos;
                        _vertexList[i + j] = vertex;
                    }
                }
                else
                {
                    for (var j = 0; j < 6; j++)
                    {
                        var vertex = _vertexList[i + j];
                        var pos = vertex.position;
                        var t = Mathf.InverseLerp(minMaxX[0], minMaxX[1], pos.x);
                        var value = _animationCurve.Evaluate(t);
                        pos.y += value * _power;
                        vertex.position = pos;
                        _vertexList[i + j] = vertex;
                    }
                }
            }

            helper.Clear();
            helper.AddUIVertexTriangleStream(_vertexList);
        }
    }
}