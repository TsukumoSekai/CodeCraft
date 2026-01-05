//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace OfflineFantasy.GameCraft.Utility
//{
//    public static class DOTweenUtility
//    {
//        /// <summary>
//        /// 模拟抛物线
//        /// </summary>
//        /// <param name="_transform"></param>
//        /// <param name="_vertexHeight"></param>
//        /// <param name="_endPoint"></param>
//        /// <param name="_duration"></param>
//        /// <returns></returns>
//        public static Sequence AnalogParabola(this Transform _transform, float _vertexHeight, Vector3 _endPoint, float _duration)
//        {
//            float halfDuration = _duration * 0.5f;

//            Sequence sequence = DOTween.Sequence(_transform);
//            sequence.Append(_transform.DOMoveX(_endPoint.x, _duration).SetEase(Ease.Linear));
//            sequence.Join(_transform.DOMoveY(_vertexHeight, halfDuration).SetEase(Ease.OutCirc));
//            sequence.Insert(halfDuration, _transform.DOMoveY(_endPoint.y, halfDuration).SetEase(Ease.InCirc));

//            return sequence;
//        }
//    }
//}