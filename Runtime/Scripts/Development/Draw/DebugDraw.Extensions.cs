////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of
// the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using UnityEngine;

namespace OSK
{
    /// <summary> Drawing of objects for development. </summary>
    /// <remarks> Only available in the Editor. </remarks>
    public static class DebugDrawExtensions
    {
        /// <summary> Draw a point with a three-axis cross. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Vector3 self, float? size = null, Color? color = null) =>
            DebugDraw.Point(self, size, Quaternion.identity, color);

        /// <summary> Draw an array of points using three-axis crosshairs. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Vector3[] self, float? size, Color? color = null) =>
            DebugDraw.Points(self, size, Quaternion.identity, color);

        /// <summary>  Draw an arrow indicating the forward direction. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Transform self, float length = 1.0f, Color? color = null)
            => DebugDraw.Arrow(self.position, self.rotation, length, SettingDraw.DebugDraw.ArrowTipSize,
                SettingDraw.DebugDraw.ArrowWidth, color);

        /// <summary> Draw bounds. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Bounds self, Color? color = null) => DebugDraw.Bounds(self, color);

        /// <summary> Draw bounds. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this BoundsInt self, Color? color = null) =>
            DebugDraw.Bounds(new Bounds(self.center, self.size), color);

        /// <summary> Draw a ray. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Ray self, Color? color = null) =>
            DebugDraw.Ray(self.origin, Quaternion.LookRotation(self.direction), color);

        /// <summary> Draw a ray with marks where there are impacts. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Ray self, RaycastHit hit, Color? color = null)
        {
            Quaternion rotation = Quaternion.LookRotation(hit.normal);

            DebugDraw.Circle(hit.point, SettingDraw.DebugDraw.HitRadius * 0.5f, rotation,
                color ?? SettingDraw.DebugDraw.HitColor);
            DebugDraw.Circle(hit.point, SettingDraw.DebugDraw.HitRadius, rotation,
                color ?? SettingDraw.DebugDraw.HitColor);
            DebugDraw.Line(hit.point, hit.point + (hit.normal * SettingDraw.DebugDraw.HitLength), Quaternion.identity,
                color ?? SettingDraw.DebugDraw.HitColor);
        }

        /// <summary> Draw a ray with marks where there are impacts. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Ray self, RaycastHit[] hits, int maxHits = 0, Color? color = null)
        {
            if (hits.Length > 0)
            {
                if (maxHits <= 0)
                    maxHits = hits.Length;

                for (int i = 0; i < maxHits; ++i)
                {
                    Quaternion rotation = hits[i].normal.sqrMagnitude > Mathf.Epsilon
                        ? Quaternion.LookRotation(hits[i].normal)
                        : Quaternion.identity;

                    DebugDraw.Circle(hits[i].point, SettingDraw.DebugDraw.HitRadius * 0.5f, rotation,
                        color ?? SettingDraw.DebugDraw.HitColor);
                    DebugDraw.Circle(hits[i].point, SettingDraw.DebugDraw.HitRadius, rotation,
                        color ?? SettingDraw.DebugDraw.HitColor);
                    DebugDraw.Line(hits[i].point, hits[i].point + (hits[i].normal * SettingDraw.DebugDraw.HitLength),
                        Quaternion.identity, color ?? SettingDraw.DebugDraw.HitColor);
                }
            }
        }

        /// <summary> Draw a collision with marks where there are impacts. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Collision self, Color? color = null)
        {
            int contacts = self.contactCount;
            for (int i = 0; i < contacts; ++i)
            {
                Quaternion rotation = self.contacts[i].normal.sqrMagnitude > Mathf.Epsilon
                    ? Quaternion.LookRotation(self.contacts[i].normal)
                    : Quaternion.identity;

                DebugDraw.Circle(self.contacts[i].point, SettingDraw.DebugDraw.HitRadius * 0.5f, rotation,
                    color ?? SettingDraw.DebugDraw.HitColor);
                DebugDraw.Circle(self.contacts[i].point, SettingDraw.DebugDraw.HitRadius, rotation,
                    color ?? SettingDraw.DebugDraw.HitColor);
                DebugDraw.Line(self.gameObject.transform.position, self.contacts[i].point, Quaternion.identity,
                    color ?? SettingDraw.DebugDraw.HitColor);
            }
        }

        /// <summary> Draw the bounds of the collider. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Draw(this Collider self) => DebugDraw.Bounds(self.bounds);

        /// <summary> Draw the name of the GameObject. </summary>
        [Conditional("UNITY_EDITOR")]
        public static void DrawName(this GameObject self, GUIStyle style)
        {
            DebugDraw.Text(self.transform.position, self.name, style);
        } 
    }
}