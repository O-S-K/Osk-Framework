using UnityEngine;
using System;

namespace OSK
{
    public static class MathUtils
    {
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
            var mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
            var mid = Vector2.Lerp(start, end, t);
            return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
        }
        
        public static Vector3[] GetBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segments)
        {
            Vector3[] points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)(segments - 1);
                points[i] = CalculateBezier(p0, p1, p2, p3, t);
            }
            return points;
        }
         
        public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

        public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        } 
        
        public static Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
            
            Vector3 p = uuu * p0; // (1-t)^3 * p0
            p += 3f * uu * t * p1; // 3(1-t)^2 * t * p1
            p += 3f * u * tt * p2; // 3(1-t) * t^2 * p2
            p += ttt * p3; // t^3 * p3
            
            return p;
        }
        
        public static Vector3[] GetCatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segments)
        {
            Vector3[] points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)(segments - 1);
                points[i] = CalculateCatmullRom(p0, p1, p2, p3, t);
            }
            return points;
        }

        public static Vector3 CalculateCatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            Vector3 result = 0.5f * (
                (2f * p1) +
                (-p0 + p2) * t +
                (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                (-p0 + 3f * p1 - 3f * p2 + p3) * t3
            );

            return result;
        }

        public static string FloatToString(float value, int deci)
        {
            string _string = "F" + deci;
            return value.ToString(_string);
        }

        public static float WrapAngle(this float angle)
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        public static Vector3 GetDirection(Vector3 start, Vector3 end)
        {
            Vector3 direction = end - start;
            direction.Normalize();
            return direction;
        }

        public static Vector3 ReverseVector(Vector3 start, Vector3 end)
        {
            Vector3 reverseVector = (start - end) * -1;
            reverseVector.Normalize();
            return reverseVector;
        }

        public static Vector3 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }

        public static float GetAngle(Vector3 start, Vector3 end)
        {
            return Mathf.Atan2(start.z - end.z, start.x - end.x) * Mathf.Rad2Deg;
        }

        public static float GetAngle(Vector2 start, Vector2 end)
        {
            return Mathf.Atan2(start.y - end.y, start.x - end.x) * Mathf.Rad2Deg;
        }

        public static int Sign(double value)
        {
            return (value >= 0) ? 1 : -1;
        }

        public static string IntToHex(uint crc)
        {
            return string.Format("{0:X}", crc);
        }

        public static uint HexToInt(string crc)
        {
            return uint.Parse(crc, System.Globalization.NumberStyles.AllowHexSpecifier);
        }


        public static Vector2 Rotate(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;

            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);

            return v;
        }


        public static Vector2 RotateAround(Vector2 center, Vector2 point, float angleInRadians)
        {
            angleInRadians *= Mathf.Deg2Rad;
            float cosTheta = Mathf.Cos(angleInRadians);
            float sinTheta = Mathf.Sin(angleInRadians);
            return new Vector2
            {
                x = (cosTheta * (point.x - center.x) - sinTheta * (point.y - center.y)),
                y = (sinTheta * (point.x - center.x) + cosTheta * (point.y - center.y))
            };
        }

        public static Quaternion LookAtToTarget2D(this Transform transform, Transform target, float speedSmoothLookAt)
        {
            Vector2 diraction = (target.position - transform.position).normalized;
            Quaternion LookRotation = Quaternion.LookRotation(new Vector2(0, diraction.y));
            return Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * speedSmoothLookAt);
        }

        public static Quaternion LookAtToTarget3D(this Transform transform, Transform target, float speedSmoothLookAt)
        {
            Vector3 diraction = (target.position - transform.position).normalized;
            Quaternion LookRotation = Quaternion.LookRotation(new Vector3(diraction.x, 0, diraction.z));
            return Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * speedSmoothLookAt);
        }
    }
}