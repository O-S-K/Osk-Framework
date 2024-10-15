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

        public static string FloatToString(float value, int decim)
        {
            string _string = "F" + decim;
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