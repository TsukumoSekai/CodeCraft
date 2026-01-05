using UnityEngine;

namespace OfflineFantasy.GameCraft.Extension
{
    public static class MonoBehaviourUtility
    {
        public static void StopCoroutineSafely(this MonoBehaviour _self, Coroutine _coroutine)
        {
            if (_coroutine != null)
                _self.StopCoroutine(_coroutine);
        }
    }
}