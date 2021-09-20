using System.Collections.Generic;
using UnityEngine;

namespace Level_Systems
{
    public class RandomPlatformSpawn : MonoBehaviour
    {
        [SerializeField] private List<PlatformSpawner> _platforms = new List<PlatformSpawner>();
        [SerializeField] private SpawnableObjectData _spawnableObjects = null;

        public void Spawn(int count)
        {
            if (_spawnableObjects == null) return;
            int iterations = 0;
            for (int i = 0; i < count; ++i) {
                int randPlatform = Random.Range(0, _platforms.Count);
                if (_platforms[randPlatform].IsClear()) {
                    GameObject obj = _spawnableObjects.GetRandom();
                    if (obj != null) {
                        _platforms[randPlatform].PrepareToSpawn(obj);
                        continue;
                    }
                }
                --i;
                if (++iterations > 100) {
                    break;
                }
            }
        }
    }
}