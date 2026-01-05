using System.Collections.Generic;
using OfflineFantasy.GameCraft.Models;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class Vector2IntUtility
    {
        public static Vector2Int RotateAround(Vector2Int _point, Vector2Int _center, float _angle)
        {
            Vector3 vector3 = Quaternion.AngleAxis(_angle, Vector3.forward) * (Vector2)(_point - _center);
            return new Vector2Int(vector3.x.RoundToInt(), vector3.y.RoundToInt()) + _center;
        }

        /// <summary>
        /// 获取多边形包围盒
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static BoundsInt GetPolygonBounds(IList<Vector2Int> points)
        {
            int xMin = int.MaxValue;
            int yMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMax = int.MinValue;

            foreach (Vector2Int point in points)
            {
                if (point.x > xMax)
                    xMax = point.x;

                if (point.y > yMax)
                    yMax = point.y;

                if (point.x < xMin)
                    xMin = point.x;

                if (point.y < yMin)
                    yMin = point.y;
            }

            BoundsInt bounds = new BoundsInt();
            bounds.SetMinMax(new Vector3Int(xMin, yMin), new Vector3Int(xMax, yMax));

            return bounds;
        }

        public static Vector2Int ToVector2Int(this IVector2Int _value)
        {
            return new Vector2Int(_value.X, _value.Y);
        }
    }
}