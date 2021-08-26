using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public abstract class CollectibleRespawnBase : CollectibleBase {
        [SerializeField] private float _respawnTime = 5f;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private Collider _collider = null;

        protected override void DisableObject()
        {
            StartCoroutine(RespawnObject());
        }

        private IEnumerator RespawnObject()
        {
            if (_art != null) _art.SetActive(false);
            if (_collider != null) _collider.enabled = false;
            yield return new WaitForSecondsRealtime(_respawnTime);
            if (_art != null) _art.SetActive(true);
            if (_collider != null) _collider.enabled = true;
        }
    }
}