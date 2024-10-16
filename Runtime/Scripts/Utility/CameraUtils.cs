using UnityEngine;

namespace OSK
{
    public class CameraUtils : MonoBehaviour
    {
        public static float WorldWidth
        {
            get { return 2f * Camera.main.orthographicSize * Camera.main.aspect; }
        }

        public static float WorldHeight
        {
            get { return 2f * Camera.main.orthographicSize; }
        }

        public static float XScale
        {
            get { return (float)UnityEngine.Screen.width / 1080f; }
        }

        public static float YScale
        {
            get { return (float)UnityEngine.Screen.height / 1920f; }
        }

        private static Vector2 sizeCamera;

        public enum LimitType
        {
            MaxLimitX,
            MinLimitX,
            MaxLimitY,
            MinLimitY
        }

        public static void SetSize(Vector2 size)
        {
            sizeCamera = new Vector2();
            sizeCamera = size;
        }

        public static Vector2 GetSize()
        {
            return sizeCamera;
        }

        public static float GetLimitCamera(Vector2 posCamera, LimitType limitType)
        {
            return limitType switch
            {
                LimitType.MaxLimitX => posCamera.x + sizeCamera.x / 2f,
                LimitType.MinLimitX => posCamera.x - sizeCamera.x / 2f,
                LimitType.MaxLimitY => posCamera.y + sizeCamera.y / 2f,
                LimitType.MinLimitY => posCamera.y - sizeCamera.y / 2f,
                _ => 0,
            };
        }


        public static Vector3 GUIPositionToWorldPosition(Camera self, Vector2 guiPosition) =>
            self.ScreenPointToRay(guiPosition).GetPoint(0.0f);

        public static Vector3 GUIDeltaToWorldDelta(Camera self, Vector2 guiDelta)
        {
            Vector3 screenDelta = GUIUtility.GUIToScreenPoint(guiDelta);
            Ray worldRay = self.ScreenPointToRay(screenDelta);

            Vector3 worldDelta = worldRay.GetPoint(0.0f);
            worldDelta -= self.ScreenPointToRay(Vector3.zero).GetPoint(0.0f);

            return worldDelta;
        }

        public static bool IsObjectVisible(Camera self, Renderer renderer) =>
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(self), renderer.bounds);

        public static bool IsPointVisible(Camera self, Vector3 point)
        {
            Vector3 p = self.WorldToViewportPoint(point);

            return p.x >= 0.0f && p.x <= 1.0f && p.y >= 0.0f && p.y <= 1.0f;
        }

        public static Rect OrthographicVisibleRect(Camera self)
        {
            return new Rect(
                self.transform.position - new Vector3(self.aspect * self.orthographicSize, self.orthographicSize),
                new Vector2
                (
                    self.aspect * self.orthographicSize * 2.0f,
                    self.orthographicSize * 2.0f
                ));
        }

        public static void FocusOn2D(Camera camera, GameObject target)
        {
            camera.transform.position = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y,
                Camera.main.transform.position.z);
        }

        public static Bounds GetBounds(Vector2 posCamera)
        {
            return new Bounds(posCamera, sizeCamera);
        }

        public static bool IsObjectVisibleOnCamera(Vector2 posCamera, Vector3 pos)
        {
            return GetBounds(posCamera).Contains(pos);
        }

        public static bool IsObjectVisibleOnCamera(Vector2 posCamera, Bounds other)
        {
            other.center = (Vector2)other.center;
            return GetBounds(posCamera).Intersects(other);
        }

        public static bool IsTargetVisible(Transform go, Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var point = go.position;
            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0)
                    return false;
            }

            return true;
        }
    }
}