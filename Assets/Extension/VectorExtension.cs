using UnityEngine;
namespace Extension
{
    public static class VectorExtension
    {
        public static Vector3 SetYinZ(this Vector3 vector, float Y = 0)
            => new Vector3(vector.x, Y, vector.y);
        public static Vector3 SetY(this Vector3 vector, float Y = 0)
            => new Vector3(vector.x, Y, vector.z);
        public static Vector3 Increas(this Vector3 vector, float value)
            => new Vector3(vector.x * value, vector.y * value, vector.z * value);
    }
}