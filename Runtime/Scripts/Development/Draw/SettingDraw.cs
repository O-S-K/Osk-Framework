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
using UnityEngine;

namespace OSK
{
    /// <summary> Configurable Foundation parameters. </summary>
    public static class SettingDraw
    {
        public static Color SetA(this Color self, float a) => new(self.r, self.g, self.b, a);

        /// <summary> Debug Draw. </summary>
        public static class DebugDraw
        {
            public static readonly bool DrawInSceneView = true;
            public static readonly bool DrawInGameView = true;

            public static readonly int Capacity = 5000;
            public static readonly string Profiler = "FronkonGames.GameWork.Foundation.DebugDraw";
            public static readonly int TextCapacity = 100;
            public static readonly float Transparency = 0.7f;
            public static readonly float OccludedColor = 0.1f;
            public static readonly float PointSize = 0.5f;
            public static readonly int Divisions = 64;
            public static readonly Color AxisXColor = Color.red;
            public static readonly Color AxisYColor = Color.green;
            public static readonly Color AxisZColor = Color.blue;
            public static readonly Color LineColor = Tailwind.Neutral_500;

            public static readonly float LineGapSize = 0.05f;
            public static readonly Color ArrowColor = Tailwind.Orange_500;
            public static readonly float ArrowTipSize = 0.25f;
            public static readonly float ArrowWidth = 0.5f;
            public static readonly Color RayColor = Tailwind.Amber_500;
            public static readonly float RayLength = 1000.0f;
            public static readonly Color HitColor = Tailwind.Yellow_200;
            public static readonly float HitRadius = 0.1f;
            public static readonly float HitLength = 0.25f;
            public static readonly Color CircleColor = Tailwind.Slate_500;
            public static readonly Color CubeColor = Tailwind.Slate_500;
            public static readonly Color SphereColor = Tailwind.Slate_500;
            public static readonly Color ArcColor = Tailwind.Slate_500;
            public static readonly Color DiamondColor = Tailwind.Slate_500;
            public static readonly float DiamondSize = 0.5f;
            public static readonly Color ConeColor = Tailwind.Slate_500;
            public static readonly Color BoundsColor = Tailwind.Slate_500;
        }
    }
}