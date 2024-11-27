using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Image))]
    public sealed class UIFilled : BaseMeshEffect
    {
        public enum FillMethod
        {
            HORIZONTAL,
            VERTICAL,
        }

        [SerializeField] [Range(0, 1)] private float m_fillAmount = 1;
        [SerializeField] private FillMethod m_fillMethod;
        [SerializeField] private bool m_isReverse;

        private readonly List<UIVertex> m_vertexList = new();

        public float FillAmount
        {
            get => m_fillAmount;
            set
            {
                m_fillAmount = Mathf.Clamp01(value);
                graphic.SetVerticesDirty();
            }
        }

        public override void ModifyMesh(VertexHelper helper)
        {
            m_vertexList.Clear();
            helper.GetUIVertexStream(m_vertexList);

            var axisIndex = (int)m_fillMethod;

            var minMax = new Vector2
            (
                m_vertexList.Select(v => v.position[axisIndex]).Min(),
                m_vertexList.Select(v => v.position[axisIndex]).Max()
            );

            var minIndex = m_isReverse ? 1 : 0;
            var maxIndex = m_isReverse ? 0 : 1;

            var targetPos = Mathf.Lerp(minMax[minIndex], minMax[maxIndex], m_fillAmount);

            for (var index = 0; index < m_vertexList.Count; index += 6)
            {
                var minMaxUV = new Vector2(m_vertexList[index].uv0[axisIndex], m_vertexList[index].uv0[axisIndex]);
                var minMaxPos = new Vector2(m_vertexList[index].position[axisIndex],
                    m_vertexList[index].position[axisIndex]);

                for (var i = 1; i < 6; i++)
                {
                    if (minMaxUV[0] > m_vertexList[index + i].uv0[axisIndex])
                    {
                        minMaxUV[0] = m_vertexList[index + i].uv0[axisIndex];
                    }

                    if (minMaxUV[1] < m_vertexList[index + i].uv0[axisIndex])
                    {
                        minMaxUV[1] = m_vertexList[index + i].uv0[axisIndex];
                    }

                    if (minMaxPos[0] > m_vertexList[index + i].position[axisIndex])
                    {
                        minMaxPos[0] = m_vertexList[index + i].position[axisIndex];
                    }

                    if (minMaxPos[1] < m_vertexList[index + i].position[axisIndex])
                    {
                        minMaxPos[1] = m_vertexList[index + i].position[axisIndex];
                    }
                }

                for (var i = 0; i < 6; i++)
                {
                    var vertex = m_vertexList[index + i];

                    if (!(m_fillAmount <
                          Mathf.InverseLerp(minMax[minIndex], minMax[maxIndex], vertex.position[axisIndex]))) continue;

                    var pos = vertex.position;

                    pos[axisIndex] = targetPos;
                    vertex.position = pos;

                    var uv = vertex.uv0;

                    uv[axisIndex] = Mathf.Lerp
                    (
                        minMaxUV[minIndex],
                        minMaxUV[maxIndex],
                        Mathf.InverseLerp(minMaxPos[minIndex], minMaxPos[maxIndex], pos[axisIndex])
                    );

                    vertex.uv0 = uv;
                    m_vertexList[index + i] = vertex;
                }
            }

            helper.Clear();
            helper.AddUIVertexTriangleStream(m_vertexList);
        }
    }
}