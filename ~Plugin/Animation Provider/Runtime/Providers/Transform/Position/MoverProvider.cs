using UnityEngine;

namespace OSK
{
    [DisallowMultipleComponent]
    public class MoverProvider : MonoBehaviour
    {
        public float speed = 1f;
        public float offset = 0f;
        public bool noNegatives = false;
        public Vector3 direction = Vector3.up;

        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        private void Update()
        {
            float sinVal = Mathf.Sin(Time.time * speed + offset * Mathf.PI);
            sinVal = noNegatives ? Mathf.Abs(sinVal) : sinVal;
            transform.localPosition = originalPosition + direction * sinVal;
        }
    }
}