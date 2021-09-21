using System.Collections;
using UnityEngine;

namespace Utility
{
    public class AutoKillObject : MonoBehaviour
    {
        [SerializeField] private float _lifespan = 1;

        private Coroutine _deathCoroutine;

        private void OnEnable()
        {
            _deathCoroutine = StartCoroutine(AutoKill());
        }

        private void OnDisable()
        {
            if (_deathCoroutine == null) return;
            StopCoroutine(_deathCoroutine);
        }

        private IEnumerator AutoKill()
        {
            yield return new WaitForSecondsRealtime(_lifespan);
            Kill();
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}