using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class UGUIUtility
    {
        #region Image

        public static void SetSprite(this Image _image, Sprite _sprite, bool _autoEnable = true)
        {
            _image.sprite = _sprite;

            if (_autoEnable)
            {
                if (_image.sprite != null)
                {
                    if (!_image.enabled)
                        _image.enabled = true;
                }
                else
                {
                    if (_image.enabled)
                        _image.enabled = false;
                }
            }
        }

        #endregion

        #region TextMeshProUGUI

        public static void SetTextAndRebuildLayout(this TextMeshProUGUI _tmp, string _text)
        {
            _tmp.SetText(_text);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_tmp.rectTransform);
        }

        #endregion

        #region CanvasGroup

        public static void Switch(this CanvasGroup _canvasGroup, bool _enable)
        {
            if (_enable)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
            }
            else
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }
        }

        public static async UniTask FadeIn(this CanvasGroup _canvasGroup, float _duration, CancellationToken _cancellationToken = default)
        {
            if (_duration > 0f)
            {
                float remaining = _duration;

                while (_duration > 0f && _cancellationToken.IsCancellationRequested)
                {
                    _canvasGroup.alpha = Mathf.Lerp(0f, 1f, remaining / _duration);
                    remaining -= Time.deltaTime;

                    await UniTask.NextFrame(_cancellationToken);
                }
            }

            _canvasGroup.alpha = 1f;
            //_canvasGroup.blocksRaycasts = true;
            //_canvasGroup.interactable = true;
        }

        public static async UniTask FadeOut(this CanvasGroup _canvasGroup, float _duration, CancellationToken _cancellationToken = default)
        {
            if (_duration > 0f)
            {
                float remaining = _duration;

                while (_duration > 0f && _cancellationToken.IsCancellationRequested)
                {
                    _canvasGroup.alpha = Mathf.Lerp(1f, 0f, remaining / _duration);
                    remaining -= Time.deltaTime;

                    await UniTask.NextFrame(_cancellationToken);
                }
            }

            _canvasGroup.alpha = 0f;
        }

        #endregion
    }
}