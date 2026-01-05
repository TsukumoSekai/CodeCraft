using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class Vector3Utility
    {
        private const float m_DefaultCanvasHalfWidth = 960f;
        private const float m_DefaultCanvasHalfHeight = 540f;

        /// <summary>
        /// 三维向量轴类型
        /// </summary>
        [System.Flags]
        public enum Vector3Type
        {
            X = 1,
            Y = 2,
            Z = 4,
        }

        /// <summary>
        /// 三维方向类型
        /// </summary>
        public enum ThreeDDirectionType
        {
            Right,
            Left,
            Up,
            Down,
            Forward,
            Back
        }

        /// <summary>
        /// 获取两直线交点
        /// </summary>
        /// <param name="_line1_StartPoint">直线1起点</param>
        /// <param name="_line1_Direction">直线1方向</param>
        /// <param name="_line2_StartPoint">直线2起点</param>
        /// <param name="_line2_Direction">直线2方向</param>
        /// <param name="_intersection">交点坐标</param>
        /// <returns></returns>
        public static bool GetIntersectionOfLineWithLine(Vector3 _line1_StartPoint, Vector3 _line1_Direction, Vector3 _line2_StartPoint, Vector3 _line2_Direction, out Vector3 _intersection)
        {
            Vector3 lineVec3 = _line2_StartPoint - _line1_StartPoint;
            Vector3 crossVec1and2 = Vector3.Cross(_line1_Direction, _line2_Direction);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, _line2_Direction);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f &&
                crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                _intersection = _line1_StartPoint + (_line1_Direction * s);
                return true;
            }
            else
            {
                _intersection = Vector3.zero;
                return false;
            }
        }

        /// <summary>
        /// 选择性赋值
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_new"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static Vector3 SelectiveAssign(Vector3 _source, Vector3 _new, Vector3Type _type = Vector3Type.X | Vector3Type.Y)
        {
            Vector3 result = _source;

            if (_type.HasFlag(Vector3Type.X))
                result.x = _new.x;
            if (_type.HasFlag(Vector3Type.Y))
                result.y = _new.y;
            if (_type.HasFlag(Vector3Type.Z))
                result.z = _new.z;

            return result;
        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static Vector3 Extract(this Vector3 _source, Vector3Type _type = Vector3Type.X | Vector3Type.Y)
        {
            Vector3 result = Vector3.zero;

            if (_type.HasFlag(Vector3Type.X))
                result.x = _source.x;
            if (_type.HasFlag(Vector3Type.Y))
                result.y = _source.y;
            if (_type.HasFlag(Vector3Type.Z))
                result.z = _source.z;

            return result;
        }

        /// <summary>
        /// 限制
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_min"></param>
        /// <param name="_max"></param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 _value, Vector3 _min, Vector3 _max)
        {
            _value.x = Mathf.Clamp(_value.x, _min.x, _max.x);
            _value.y = Mathf.Clamp(_value.y, _min.y, _max.y);
            _value.z = Mathf.Clamp(_value.z, _min.z, _max.z);

            return _value;
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static Vector3 Abs(Vector3 _value)
        {
            _value.x = Mathf.Abs(_value.x);
            _value.y = Mathf.Abs(_value.y);
            _value.z = Mathf.Abs(_value.z);

            return _value;
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
        public static Vector3 Remap(Vector3 _value, Vector3 _sourceMin, Vector3 _sourceMax, Vector3 _targetMin, Vector3 _targetMax)
        {
            return new Vector3(FloatUtility.Remap(_value.x, _sourceMin.x, _sourceMax.x, _targetMin.x, _targetMax.x),
                               FloatUtility.Remap(_value.y, _sourceMin.y, _sourceMax.y, _targetMin.y, _targetMax.y),
                               FloatUtility.Remap(_value.z, _sourceMin.z, _sourceMax.z, _targetMin.z, _targetMax.z));
        }

        /// <summary>
        /// 获取总长
        /// </summary>
        /// <param name="_valueArray"></param>
        /// <returns></returns>
        public static float GetLength(Vector3[] _valueArray)
        {
            float length = 0;

            if (_valueArray.Length > 1)
            {
                for (int i = 1; i < _valueArray.Length; i++)
                {
                    length += Vector3.Distance(_valueArray[i - 1], _valueArray[i]);
                }
            }

            return length;
        }

        #region 算法

        #region 贝塞尔曲线

        #region 二次贝塞尔曲线

        /// <summary>
        /// 二次贝塞尔曲线
        /// </summary>
        /// <param name="_t">插值，范围0-1</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3 QuadraticBezierCurve(float _t, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint)
        {
            Vector3 a = _startPoint + (_controlPoint - _startPoint) * _t;
            Vector3 b = _controlPoint + (_endPoint - _controlPoint) * _t;

            return a + (b - a) * _t;
        }

        /// <summary>
        /// 获取二次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3[] GetQuadraticBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint)
        {
            Vector3[] pathArray = new Vector3[_count];
            float step = 1f / (_count - 1);

            for (int i = 0; i < _count; i++)
            {
                pathArray[i] = QuadraticBezierCurve(i * step, _startPoint, _controlPoint, _endPoint);
            }

            return pathArray;
        }

        /// <summary>
        /// 获取二次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <param name="_length">曲线长度</param>
        /// <returns></returns>
        public static Vector3[] GetQuadraticBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint, out float _length)
        {
            Vector3[] pathArray = GetQuadraticBezierCurvePathArray(_count, _startPoint, _controlPoint, _endPoint);
            _length = 0f;

            for (int i = 1; i < pathArray.Length; i++)
            {
                _length += Vector3.Distance(pathArray[i - 1], pathArray[i]);
            }

            return pathArray;
        }

        #endregion

        #region 三次贝塞尔曲线

        /// <summary>
        /// 三次贝塞尔曲线
        /// </summary>
        /// <param name="_t">插值，范围0-1</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint_0">控制点_0</param>
        /// <param name="_controlPoint_0">控制点_1</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3 CubicBezierCurve(float _t, Vector3 _startPoint, Vector3 _controlPoint_0, Vector3 _controlPoint_1, Vector3 _endPoint)
        {
            Vector3 aa = _startPoint + (_controlPoint_0 - _startPoint) * _t;
            Vector3 bb = _controlPoint_0 + (_controlPoint_1 - _controlPoint_0) * _t;
            Vector3 cc = _controlPoint_1 + (_endPoint - _controlPoint_1) * _t;

            Vector3 aaa = aa + (bb - aa) * _t;
            Vector3 bbb = bb + (cc - bb) * _t;

            return aaa + (bbb - aaa) * _t;
        }

        /// <summary>
        /// 获取三次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint_0">控制点_0</param>
        /// <param name="_controlPoint_0">控制点_1</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3[] GetCubicBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint_0, Vector3 _controlPoint_1, Vector3 _endPoint)
        {
            Vector3[] pathArray = new Vector3[_count];
            float step = 1f / (_count - 1);

            for (int i = 0; i < _count; i++)
            {
                pathArray[i] = CubicBezierCurve(i * step, _startPoint, _controlPoint_0, _controlPoint_1, _endPoint);
            }

            return pathArray;
        }

        /// <summary>
        /// 获取三次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint_0">控制点_0</param>
        /// <param name="_controlPoint_1">控制点_1</param>
        /// <param name="_endPoint">终点</param>
        /// <param name="_length">曲线长度</param>
        /// <returns></returns>
        public static Vector3[] GetCubicBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint_0, Vector3 _controlPoint_1, Vector3 _endPoint, out float _length)
        {
            Vector3[] pathArray = GetCubicBezierCurvePathArray(_count, _startPoint, _controlPoint_0, _controlPoint_1, _endPoint);
            _length = 0f;

            for (int i = 1; i < pathArray.Length; i++)
            {
                _length += Vector3.Distance(pathArray[i - 1], pathArray[i]);
            }

            return pathArray;
        }

        #endregion

        #region N次贝塞尔曲线

        /// <summary>
        /// N次贝塞尔曲线
        /// 开销略高
        /// 二次和三次贝塞尔曲线用上述方法
        /// </summary>
        /// <param name="_t">插值，范围0-1</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_endPoint">终点</param>
        /// <param name="_controlPointArray">控制点</param>
        /// <returns></returns>
        public static Vector3 CustomBezierCurve(float _t, Vector3 _startPoint, Vector3 _endPoint, params Vector3[] _controlPointArray)
        {
            if (_controlPointArray == null)
            {
                return _startPoint + (_endPoint - _startPoint) * _t;
            }
            else
            {
                List<Vector3[]> vector3List = new List<Vector3[]>();
                vector3List.Add(new Vector3[_controlPointArray.Length + 2]);

                vector3List[0][0] = _startPoint;
                vector3List[0][_controlPointArray.Length + 1] = _endPoint;
                for (int i = 0; i < _controlPointArray.Length; i++)
                {
                    vector3List[0][i + 1] = _controlPointArray[i];
                }

                for (int i = 0; i < _controlPointArray.Length + 1; i++)
                {
                    vector3List.Add(new Vector3[_controlPointArray.Length + 1 - i]);

                    for (int j = 0; j < vector3List[i].Length - 1; j++)
                    {
                        vector3List[i + 1][j] = vector3List[i][j] + (vector3List[i][j + 1] - vector3List[i][j]) * _t;
                    }
                }

                return vector3List[_controlPointArray.Length + 1][0];
            }
        }

        /// <summary>
        /// 获取N次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_endPoint">终点</param>
        /// <param name="_controlPointArray">控制点</param>
        /// <returns></returns>
        public static Vector3[] GetCustomBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _endPoint, params Vector3[] _controlPointArray)
        {
            Vector3[] pathArray = new Vector3[_count];
            float step = 1f / (_count - 1);

            for (int i = 0; i < _count; i++)
            {
                pathArray[i] = CustomBezierCurve(i * step, _startPoint, _endPoint, _controlPointArray);
            }

            return pathArray;
        }

        #endregion

        #region 经过控制点的二次贝塞尔曲线

        /// <summary>
        /// 经过控制点的二次贝塞尔曲线
        /// </summary>
        /// <param name="_t">插值，范围0-1</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3 OverControlPointQuadraticBezierCurve(float _t, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint)
        {
            _controlPoint -= Mathf.Sqrt((_startPoint - _controlPoint).magnitude * (_endPoint - _controlPoint).magnitude) *
                             ((_startPoint - _controlPoint) / (_startPoint - _controlPoint).magnitude + (_endPoint - _controlPoint) / (_endPoint - _controlPoint).magnitude) * 0.5f;

            Vector3 a = _startPoint + (_controlPoint - _startPoint) * _t;
            Vector3 b = _controlPoint + (_endPoint - _controlPoint) * _t;

            return a + (b - a) * _t;
        }

        /// <summary>
        /// 获取经过控制点的二次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <returns></returns>
        public static Vector3[] GetOverControlPointQuadraticBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint)
        {
            Vector3[] pathArray = new Vector3[_count];
            float step = 1f / (_count - 1);

            for (int i = 0; i < _count; i++)
            {
                pathArray[i] = OverControlPointQuadraticBezierCurve(i * step, _startPoint, _controlPoint, _endPoint);
            }

            return pathArray;
        }

        /// <summary>
        /// 获取经过控制点的二次贝塞尔曲线路径
        /// </summary>
        /// <param name="_count">插值次数</param>
        /// <param name="_startPoint">起点</param>
        /// <param name="_controlPoint">控制点</param>
        /// <param name="_endPoint">终点</param>
        /// <param name="_length">曲线长度</param>
        /// <returns></returns>
        public static Vector3[] GetOverControlPointQuadraticBezierCurvePathArray(int _count, Vector3 _startPoint, Vector3 _controlPoint, Vector3 _endPoint, out float _length)
        {
            Vector3[] pathArray = GetOverControlPointQuadraticBezierCurvePathArray(_count, _startPoint, _controlPoint, _endPoint);
            _length = 0f;

            for (int i = 1; i < pathArray.Length; i++)
            {
                _length += Vector3.Distance(pathArray[i - 1], pathArray[i]);
            }

            return pathArray;
        }

        #endregion

        #endregion

        #region 旋转

        /// <summary>
        /// 旋转方向向量
        /// </summary>
        /// <param name="_direction"></param>
        /// <param name="_axis"></param>
        /// <param name="_angle"></param>
        /// <returns></returns>
        public static Vector3 Rotate(Vector3 _direction, Vector3 _axis, float _angle)
        {
            return RotateAround(_direction, Vector3.zero, _axis, _angle);
        }

        /// <summary>
        /// 获取向量绕点旋转后的新向量
        /// </summary>
        /// <param name="_point"></param>
        /// <param name="_center"></param>
        /// <param name="_axis"></param>
        /// <param name="_angle"></param>
        /// <returns></returns>
        public static Vector3 RotateAround(Vector3 _point, Vector3 _center, Vector3 _axis, float _angle)
        {
            return Quaternion.AngleAxis(_angle, _axis) * (_point - _center) + _center;
        }

        #endregion

        /// <summary>
        /// 获取椭圆形的点
        /// </summary>
        /// <param name="_center"></param>
        /// <param name="_ellipseHorizontalRadius"></param>
        /// <param name="_ellipseVerticalRadius"></param>
        /// <param name="_vertexCount"></param>
        /// <returns></returns>
        public static Vector3[] GetEllipsePoints(Vector3 _center, float _ellipseHorizontalRadius, float _ellipseVerticalRadius, int _vertexCount = 64)
        {
            Vector3[] points = new Vector3[_vertexCount];

            float deltaAngle = Mathf.PI * 2f / _vertexCount;
            float angle = 0f;

            float x, y;

            for (int i = 0; i < _vertexCount; i++)
            {
                x = _center.x + Mathf.Sin(angle) * _ellipseHorizontalRadius;
                y = _center.y + Mathf.Cos(angle) * _ellipseVerticalRadius;

                points[i] = new Vector3(_center.x + x, _center.y + y);

                angle += deltaAngle;
            }

            return points;
        }

        #endregion

        #region 转换

        #region 世界<->屏幕

        /// <summary>
        /// 世界转屏幕坐标
        /// </summary>
        /// <param name="_worldGlobalPoint"></param>
        /// <param name="_worldCamera"></param>
        /// <returns></returns>
        public static Vector2 WorldToScreenPoint(Camera _worldCamera, Vector3 _worldGlobalPoint)
        {
            return _worldCamera.WorldToScreenPoint(_worldGlobalPoint);
        }

        //public static Vector2 WorldToScreenPoint(Vector3 _worldPoint, Canvas _canvas, float _canvasHalfWidth = m_DefaultCanvasHalfWidth, float _canvasHalfHeight = m_DefaultCanvasHalfHeight)
        //{
        //    Vector2 point = new Vector3(FloatUtility.Remap(_worldPoint.x, -_canvasHalfWidth, _canvasHalfWidth, 0f, Screen.width),
        //                                FloatUtility.Remap(_worldPoint.y, -_canvasHalfHeight, _canvasHalfHeight, 0f, Screen.height));

        //    return point;
        //}

        /// <summary>
        /// 屏幕转世界坐标，性能较好
        /// </summary>
        /// <param name="_screenPoint"></param>
        /// <param name="_canvas"></param>
        /// <param name="_canvasHalfWidth"></param>
        /// <param name="_canvasHalfHeight"></param>
        /// <returns></returns>
        public static Vector3 ScreenToWorldPoint(Vector2 _screenPoint, Canvas _canvas, float _canvasHalfWidth = m_DefaultCanvasHalfWidth, float _canvasHalfHeight = m_DefaultCanvasHalfHeight)
        {
            Vector3 point = new Vector3(FloatUtility.Remap(_screenPoint.x, 0f, Screen.width, -_canvasHalfWidth, _canvasHalfWidth),
                                        FloatUtility.Remap(_screenPoint.y, 0f, Screen.height, -_canvasHalfHeight, _canvasHalfHeight));

            return _canvas.transform.TransformPoint(point);
        }

        /// <summary>
        /// 屏幕转世界坐标，性能较差
        /// </summary>
        /// <param name="_screenPoint"></param>
        /// <param name="_worldCamera"></param>
        /// <returns></returns>
        public static Vector3 ScreenToWorldPoint(Camera _worldCamera, Vector2 _screenPoint)
        {
            return _worldCamera.ScreenToWorldPoint(_screenPoint);
        }

        #endregion

        #region 屏幕<->UI

        /// <summary>
        /// UI转屏幕坐标
        /// </summary>
        /// <param name="_uiGlobalPoint"></param>
        /// <param name="_uiCamera"></param>
        /// <returns></returns>
        public static Vector2 UIToScreenPoint(Camera _uiCamera, Vector3 _uiGlobalPoint)
        {
            return UnityEngine.RectTransformUtility.WorldToScreenPoint(_uiCamera, _uiGlobalPoint);
        }

        /// <summary>
        /// 屏幕转UI坐标，用重映射实现，性能较好
        /// </summary>
        /// <param name="_canvas"></param>
        /// <param name="_screenPoint"></param>
        /// <param name="_canvasHalfWidth"></param>
        /// <param name="_canvasHalfHeight"></param>
        /// <returns></returns>
        public static Vector3 ScreenToUIPoint(Vector2 _screenPoint, Canvas _canvas, RectTransform _parent, float _canvasHalfWidth = m_DefaultCanvasHalfWidth, float _canvasHalfHeight = m_DefaultCanvasHalfHeight)
        {
            return _parent.InverseTransformPoint(ScreenToWorldPoint(_screenPoint, _canvas, _canvasHalfWidth, _canvasHalfHeight));
        }

        /// <summary>
        /// 屏幕转UI坐标，用Unity自身API实现，性能较差
        /// </summary>
        /// <param name="_uiCamera"></param>
        /// <param name="_rt"></param>
        /// <param name="_screenPoint"></param>
        /// <returns>世界坐标</returns>
        public static Vector3 ScreenToUIPoint(Camera _uiCamera, RectTransform _rt, Vector2 _screenPoint)
        {
            UnityEngine.RectTransformUtility.ScreenPointToWorldPointInRectangle(_rt, _screenPoint, _uiCamera, out var uiGlobalPoint);

            return uiGlobalPoint;
        }

        #endregion

        #region 世界<->UI

        /// <summary>
        /// 世界转UI坐标
        /// </summary>
        /// <param name="_worldCamera"></param>
        /// <param name="_uiCamera"></param>
        /// <param name="_rt"></param>
        /// <param name="_worldGlobalPoint"></param>
        /// <returns></returns>
        public static Vector3 WorldToUIPoint(Camera _worldCamera, Camera _uiCamera, RectTransform _rt, Vector3 _worldGlobalPoint)
        {
            var screenPoint = WorldToScreenPoint(_worldCamera, _worldGlobalPoint);
            return ScreenToUIPoint(_uiCamera, _rt, screenPoint);
        }

        /// <summary>
        /// UI转世界坐标
        /// </summary>
        /// <param name="_worldCamera"></param>
        /// <param name="_uiCamera"></param>
        /// <param name="_uiGlobalPoint"></param>
        /// <returns></returns>
        public static Vector3 UIToWorldPoint(Camera _worldCamera, Camera _uiCamera, Vector3 _uiGlobalPoint)
        {
            var screenPoint = UIToScreenPoint(_worldCamera, _uiGlobalPoint);
            return ScreenToWorldPoint(_worldCamera, screenPoint);
        }

        #endregion

        #endregion
    }
}