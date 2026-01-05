using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class DebugUtility
    {
        /// <summary>
        /// 通过四边形对角顶点绘制四边形
        /// </summary>
        /// <param name="_point_0"></param>
        /// <param name="_point_1"></param>
        /// <param name="_color"></param>
        /// <param name="_duration"></param>
        public static void DrawQuadrangleByDiagonalVertex(Vector3 _point_0, Vector3 _point_1, Color? _color = null, float _duration = 0f)
        {
            if (!_color.HasValue)
                _color = Color.red;

            Debug.DrawLine(_point_0, new Vector3(_point_1.x, _point_0.y), _color.Value, _duration);
            Debug.DrawLine(new Vector3(_point_1.x, _point_0.y), _point_1, _color.Value, _duration);
            Debug.DrawLine(_point_1, new Vector3(_point_0.x, _point_1.y), _color.Value, _duration);
            Debug.DrawLine(new Vector3(_point_0.x, _point_1.y), _point_0, _color.Value, _duration);
        }

        /// <summary>
        /// 通过四边形大小绘制四边形
        /// </summary>
        /// <param name="_center"></param>
        /// <param name="_size"></param>
        /// <param name="_color"></param>
        /// <param name="_duration"></param>
        public static void DrawQuadrangleBySize(Vector3 _center, Vector3 _size, float _angle, Color? _color = null, float _duration = 0f)
        {
            if (!_color.HasValue)
                _color = Color.red;

            Vector3 point_0 = _center + _size * 0.5f;
            Vector3 point_1 = _center + new Vector3(_size.x * 0.5f, -_size.y * 0.5f);
            Vector3 point_2 = _center - _size * 0.5f;
            Vector3 point_3 = _center + new Vector3(-_size.x * 0.5f, _size.y * 0.5f);

            point_0 = Vector3Utility.RotateAround(point_0, _center, Vector3.forward, _angle);
            point_1 = Vector3Utility.RotateAround(point_1, _center, Vector3.forward, _angle);
            point_2 = Vector3Utility.RotateAround(point_2, _center, Vector3.forward, _angle);
            point_3 = Vector3Utility.RotateAround(point_3, _center, Vector3.forward, _angle);


            Debug.DrawLine(point_0, point_1, _color.Value, _duration);
            Debug.DrawLine(point_1, point_2, _color.Value, _duration);
            Debug.DrawLine(point_2, point_3, _color.Value, _duration);
            Debug.DrawLine(point_3, point_0, _color.Value, _duration);

            //DrawQuadrangleByDiagonalVertex(_center - _size, _center + _size, _color, _duration);
        }

        public static void DrawBounds(Bounds _bounds, Color? _color = null, float _duration = 0f)
        {
            DrawQuadrangleByDiagonalVertex(_bounds.min, _bounds.max, _color, _duration);
        }

        public static void DrawCircle(Vector3 _center, float _radius, int _vertexCount = 36, Color? _color = null, float _duration = 0f)
        {
            if (!_color.HasValue)
                _color = Color.red;

            _vertexCount = Mathf.Clamp(_vertexCount, 3, 3600);

            float angleStep = 360f / _vertexCount;

            Vector3 lastPoint = _center + Quaternion.AngleAxis(360f - angleStep, Vector3.forward) * Vector3.up * _radius;
            Vector3 currentPoint;

            for (int i = 0; i < _vertexCount; i++)
            {
                currentPoint = _center + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.up * _radius;

                Debug.DrawLine(lastPoint, currentPoint, _color.Value, _duration);

                lastPoint = currentPoint;
            }
        }
    }
}