using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility {
    public class RespawnObject : MonoBehaviour {
        public void Respawn(GameObject obj, float timer)
        {
            StartCoroutine(RespawnTimer(obj, timer));
        }

        private IEnumerator RespawnTimer(GameObject obj, float timer)
        {
            obj.SetActive(false);
            yield return new WaitForSecondsRealtime(timer);
            obj.SetActive(true);
            Destroy(gameObject);
        }
    }
}