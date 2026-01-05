using System.Collections.Generic;
using OfflineFantasy.GameCraft.Models;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class Vector2Utility
    {
        [System.Flags]
        public enum AxisType
        {
            X,
            Y,
        }

        /// <summary>
        /// 二维方向类型
        /// </summary>
        public enum TwoDDirectionType
        {
            Right,
            Left,
            Up,
            Down,
        }

        /// <summary>
        /// 重映射
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_sourceMin"></param>
        /// <param name="_sourceMax"></param>
        /// <param name="_targetMin"></param>
        /// <param name="_targetMax"></param>
        /// <returns></returns>
        public static Vector2 Remap(Vector2 _value, Vector2 _sourceMin, Vector2 _sourceMax, Vector2 _targetMin, Vector2 _targetMax)
        {
            return new Vector2(FloatUtility.Remap(_value.x, _sourceMin.x, _sourceMax.x, _targetMin.x, _targetMax.x),
                               FloatUtility.Remap(_value.y, _sourceMin.y, _sourceMax.y, _targetMin.y, _targetMax.y));
        }

        #region 求随机值

        /// <summary>
        /// 获取两点确定的矩形中随机点
        /// </summary>
        /// <param name="_anchor_0"></param>
        /// <param name="_anchor_1"></param>
        /// <returns></returns>
        public static Vector2 GetRandomPointInArea(Vector2 _anchor_0, Vector2 _anchor_1)
        {
            float minX = Mathf.Min(_anchor_0.x, _anchor_1.x);
            float maxX = Mathf.Max(_anchor_0.x, _anchor_1.x);
            float minY = Mathf.Min(_anchor_0.y, _anchor_1.y);
            float maxY = Mathf.Max(_anchor_0.y, _anchor_1.y);

            return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }

        //public static Vector2 GetRandomPointInEllipse(float _width, float _height)
        //{
        //    float angle = Mathf.Atan2(Random.value, Random.value);
        //    float radius = Mathf.Sqrt(Random.value) * Mathf.Min(_width, _height) * 0.5f;

        //    return new Vector2(radius * Mathf.Cos(angle) * _width * 0.5f + _width * 0.5f,
        //                       radius * Mathf.Sin(angle) * _height * 0.5f + _height * 0.5f);
        //}

        #endregion

        /// <summary>
        /// 根据角度和力计算抛物线
        /// </summary>
        /// <param name="_angle">与地面的夹角</param>
        /// <param name="_force">力</param>
        /// <returns></returns>
        public static Vector2 GetParabolaVelocity(float _angle, float _force)
        {
            return new Vector2(_force * Mathf.Cos(_angle * Mathf.Deg2Rad),
                               _force * Mathf.Sin(_angle * Mathf.Deg2Rad));
        }

        public static Vector2 Clamp(Vector2 _value, Vector2 _min, Vector2 _max)
        {
            _value.x = Mathf.Clamp(_value.x, _min.x, _max.x);
            _value.y = Mathf.Clamp(_value.y, _min.y, _max.y);

            return _value;
        }

        public static Vector2 Abs(Vector2 _value)
        {
            return new Vector2(Mathf.Abs(_value.x), Mathf.Abs(_value.y));
        }

        /// <summary>
        /// 检查点是否在矩形内
        /// </summary>
        /// <param name="_point"></param>
        /// <param name="_minPoint"></param>
        /// <param name="_maxPoint"></param>
        /// <returns></returns>
        public static bool CheckPointInRectangle(Vector2 _point, Vector2 _minPoint, Vector2 _maxPoint, bool _includeBoundary = false)
        {
            return _includeBoundary ?
                   _point.x >= _minPoint.x && _point.y >= _minPoint.y && _point.x <= _maxPoint.x && _point.y <= _maxPoint.y :
                   _point.x > _minPoint.x && _point.y > _minPoint.y && _point.x < _maxPoint.x && _point.y < _maxPoint.y;
        }

        /// <summary>
        /// 检查点是否在多边形内
        /// </summary>
        /// <param name="_point"></param>
        /// <param name="_polygonPointArray"></param>
        /// <param name="_includeBoundary"></param>
        /// <returns></returns>
        public static bool CheckPointInPolygon(Vector2 _point, IList<Vector2> _polygonPointArray, bool _includeBoundary = false)
        {
            int intersections = 0;
            Vector2 p1, p2;

            p1 = _polygonPointArray[_polygonPointArray.Count - 1];

            for (int i = 0; i < _polygonPointArray.Count; i++)
            {
                p2 = _polygonPointArray[i];

                if (_point.y > Mathf.Min(p1.y, p2.y) && _point.y <= Mathf.Max(p1.y, p2.y) && _point.x <= Mathf.Max(p1.x, p2.x) && p1.y != p2.y)
                {
                    float xIntersection = (_point.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
                    if (p1.x == p2.x || _point.x <= xIntersection)
                    {
                        intersections++;
                    }
                }

                p1 = p2;
            }
            return (intersections % 2) != 0;
        }

        /// <summary>
        /// 获取多个坐标的长度总和
        /// </summary>
        /// <param name="_valueArray"></param>
        /// <returns></returns>
        public static float GetLength(IList<Vector2> _valueArray)
        {
            float length = 0;

            if (_valueArray.Count > 1)
            {
                for (int i = 1; i < _valueArray.Count; i++)
                {
                    length += Vector2.Distance(_valueArray[i - 1], _valueArray[i]);
                }
            }

            return length;
        }

        /// <summary>
        /// 规范向量
        /// </summary>
        /// <param name="_vector2"></param>
        /// <param name="_axisType"></param>
        /// <param name="_snapLimit"></param>
        /// <returns></returns>
        public static Vector2 SnapVector(Vector2 _vector2, AxisType _axisType, Vector2 _snapLimit)
        {
            float angle = Mathf.Abs(Vector2.SignedAngle(Vector2.up, _vector2));

            if (_axisType.HasFlag(AxisType.X))
                _vector2.x = angle >= 90f - _snapLimit.x && angle <= 90f + _snapLimit.x ?
                             _vector2.x > 0f ? 1f : -1f :
                             0f;

            if (_axisType.HasFlag(AxisType.X))
                _vector2.y = angle <= _snapLimit.y || angle >= 180f - _snapLimit.y ?
                             _vector2.y > 0f ? 1f : -1f :
                             0f;

            //_vector2.Normalize();
            //_vector2 = _vector2.normalized;
            //_vector2.x = System.MathF.Round(_vector2.x, 1);
            //_vector2.y = System.MathF.Round(_vector2.y, 1);

            //return _vector2;
            return _vector2.normalized;
        }

        /// <summary>
        /// 规范向量_四向
        /// </summary>
        /// <param name="_vector2"></param>
        /// <param name="_axisType"></param>
        /// <returns></returns>
        public static Vector2 SnapVector_4D(Vector2 _vector2, AxisType _axisType)
        {
            return SnapVector(_vector2, _axisType, new Vector2(45f, 45f));
        }

        /// <summary>
        /// 规范向量_八向
        /// </summary>
        /// <param name="_vector2"></param>
        /// <param name="_axisType"></param>
        /// <returns></returns>
        public static Vector2 SnapVector_8D(Vector2 _vector2, AxisType _axisType)
        {
            return SnapVector(_vector2, _axisType, new Vector2(67.5f, 67.5f));
        }

        /// <summary>
        /// 获取多边形包围盒
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Bounds GetPolygonBounds(IList<Vector2> points)
        {
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;

            foreach (Vector2 point in points)
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

            Bounds bounds = new Bounds();
            bounds.SetMinMax(new Vector3(xMin, yMin), new Vector3(xMax, yMax));

            return bounds;
        }

        /// <summary>
        /// 整体移动多个点
        /// </summary>
        /// <param name="_offset"></param>
        /// <param name="points"></param>
        public static IList<Vector2> MovePoints(Vector2 _offset, IList<Vector2> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] += _offset;
            }

            return points;
        }

        #region 算法

        /// <summary>
        /// 边界追踪算法
        /// </summary>
        /// <param name="_edges"></param>
        /// <returns></returns>
        public static List<Vector2> TraceBoundary(IList<(Vector2, Vector2)> _edges)
        {
            List<Vector2> result = new();

            if (_edges.Count == 0)
                return result;

            var startEdge = _edges[0];

            Vector2 currentVertex = startEdge.Item1;
            Vector2 nextVertex = startEdge.Item2;

            //存疑：初始点可能在内部
            result.Add(currentVertex);
            result.Add(nextVertex);

            _edges.RemoveAt(0);

            bool foundNext;

            while (_edges.Count > 0)
            {
                foundNext = false;

                foreach (var edge in _edges)
                {
                    if (Vector2.Distance(edge.Item1, nextVertex) <= 1f)
                    {
                        currentVertex = nextVertex;
                        nextVertex = edge.Item2;

                        result.Add(nextVertex);

                        _edges.Remove(edge);

                        foundNext = true;

                        break;
                    }
                    else if (Vector2.Distance(edge.Item2, nextVertex) <= 1f)
                    {
                        currentVertex = nextVertex;
                        nextVertex = edge.Item1;

                        result.Add(nextVertex);

                        _edges.Remove(edge);

                        foundNext = true;

                        break;
                    }
                }

                if (!foundNext)
                {
                    Debug.LogError("算法错误");
                    break;
                }
            }

            return result;
        }


        #endregion

        #region 转换

        public static Vector2 ToVector2(this IVector2 _value)
        {
            return new Vector2(_value.X, _value.Y);
        }

        #endregion

        #region 拓展方法

        /// <summary>
        /// 获取和值
        /// </summary>
        /// <param name="_vector2"></param>
        /// <returns></returns>
        public static float GetSumValue(this Vector2 _vector2)
        {
            return _vector2.x + _vector2.y;
        }

        #endregion
    }
}