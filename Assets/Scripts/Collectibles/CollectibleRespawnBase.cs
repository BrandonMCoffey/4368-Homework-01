using System.Collections;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public abstract class CollectibleRespawnBase : CollectibleBase {
        [SerializeField] private float _respawnTime = 5f;

        protected override void DisableObject()
        {
            RespawnObject respawn = new GameObject("Respawning " + gameObject.name, typeof(RespawnObject)).GetComponent<RespawnObject>();
            respawn.Respawn(gameObject, _respawnTime);
        }
    }
}