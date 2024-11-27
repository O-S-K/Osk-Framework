using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class Path
    {
        public int index;
        public Vector3 position;
        public Quaternion rotation;

        public Path(int index, Vector3 position, Quaternion rotation)
        {
            this.index = index;
            this.position = position;
            this.rotation = rotation;
        }
    }
}