using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class CameraUtility
    {
        /// <summary>
        /// 获取相机投射视锥半尺寸
        /// </summary>
        /// <param name="_camera"></param>
        /// <param name="_zOffset"></param>
        /// <returns></returns>
        public static Vector2 GetCameraProjectionConeHalfSize(Camera _camera, float _zOffset)
        {
            float verticalAngle = _camera.fieldOfView * Mathf.PI / 360f;
            float horizontalAngle = Camera.VerticalToHorizontalFieldOfView(_camera.fieldOfView, _camera.aspect) * Mathf.PI / 360f;

            float verticalOffset = _zOffset * Mathf.Tan(verticalAngle);
            float horizontalOffset = _zOffset * Mathf.Tan(horizontalAngle);

            return new Vector2(horizontalOffset, verticalOffset);
        }

        /// <summary>
        /// 获取相机投射视锥尺寸
        /// </summary>
        /// <param name="_camera"></param>
        /// <param name="_zOffset"></param>
        /// <returns></returns>
        public static Vector2 GetCameraProjectionConeSize(Camera _camera, float _zOffset)
        {
            return GetCameraProjectionConeHalfSize(_camera, _zOffset) * 2f;
        }

        public enum CameraProjectionConeVerticeType
        {
            RightUp,
            RightDown,
            LeftUp,
            LeftDown,
        }

        /// <summary>
        /// 获取相机投射视锥顶点坐标
        /// </summary>
        /// <param name="_camera"></param>
        /// <param name="_zOffset"></param>
        /// <param name="_verticeType"></param>
        /// <returns></returns>
        public static Vector3 GetCameraProjectionConeVertice(Camera _camera, float _zOffset, CameraProjectionConeVerticeType _verticeType)
        {
            Vector2 coneHalfSize = GetCameraProjectionConeHalfSize(_camera, _zOffset);

            Vector2 direction;

            switch (_verticeType)
            {
                case CameraProjectionConeVerticeType.RightUp:
                    direction = new Vector2(1f, 1f);
                    break;
                case CameraProjectionConeVerticeType.RightDown:
                    direction = new Vector2(1f, -1f);
                    break;
                case CameraProjectionConeVerticeType.LeftUp:
                    direction = new Vector2(-1f, 1f);
                    break;
                case CameraProjectionConeVerticeType.LeftDown:
                    direction = new Vector2(-1f, -1f);
                    break;
                default:
                    direction = Vector2.zero;
                    break;
            }

            return _camera.transform.position + new Vector3(coneHalfSize.x * direction.x, coneHalfSize.y * direction.y, _zOffset);
        }

        #region 拓展方法

        public static void AddCullingMask(this Camera _camera, params int[] _layerArray)
        {
            if (_layerArray != null)
                for (int i = 0; i < _layerArray.Length; i++)
                {
                    _camera.cullingMask |= 1 << _layerArray[i];
                }
        }

        public static void RemoveCullingMask(this Camera _camera, params int[] _layerArray)
        {
            if (_layerArray != null)
                for (int i = 0; i < _layerArray.Length; i++)
                {
                    _camera.cullingMask &= ~(1 << _layerArray[i]);
                }
        }

        #endregion
    }
}