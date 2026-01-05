using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class RectTransformUtility
    {
        private static readonly Vector3[] s_Corners = new Vector3[4];

        public static Bounds CalculateRelativeRectTransformBoundsWithoutChildren(RectTransform _root, RectTransform _child)
        {
            Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Matrix4x4 worldToLocalMatrix = _root.worldToLocalMatrix;

            _child.GetWorldCorners(s_Corners);

            for (int j = 0; j < 4; j++)
            {
                Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(s_Corners[j]);
                vector = Vector3.Min(lhs, vector);
                vector2 = Vector3.Max(lhs, vector2);
            }

            Bounds result = new Bounds(vector, Vector3.zero);
            result.Encapsulate(vector2);

            return result;
        }
    }
}