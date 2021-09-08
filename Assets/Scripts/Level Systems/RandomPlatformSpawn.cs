using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level_Systems
{
    public class RandomPlatformSpawn : MonoBehaviour
    {
        [SerializeField] private List<PlatformSpawner> _platforms = new List<PlatformSpawner>();
        [SerializeField] private List<GameObject> _spawnableObjects = new List<GameObject>();

        public void Spawn(int count)
        {
            if (_spawnableObjects.Count == 0) return;
            int iterations = 0;
            for (int i = 0; i < count; ++i) {
                int randPlatform = Random.Range(0, _platforms.Count);
                if (_platforms[randPlatform].IsClear()) {
                    int randObject = Random.Range(0, _spawnableObjects.Count);
                    _platforms[randPlatform].PrepareToSpawn(_spawnableObjects[randObject]);
                } else {
                    i--;
                }
                if (++iterations > 100) {
                    break;
                }
            }
        }
    }
}