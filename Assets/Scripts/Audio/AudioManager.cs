using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio {
    public class AudioManager : MonoBehaviour {
        [Header("Music Controller")]
        [SerializeField] private SoundInfo _backgroundMusic = null;
        [SerializeField] private AudioSourceController _musicController = null;
        [Header("Audio Pool")]
        [SerializeField] private string _audioPlayerName = "Audio Player";
        [SerializeField] private Transform _poolParent;
        [SerializeField] private int _initialPoolSize = 5;

        private List<AudioSourceController> _pool = new List<AudioSourceController>();

        #region Singleton

        private static AudioManager _instance;

        public static AudioManager Instance {
            get {
                if (_instance == null) {
                    _instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                    // Start is called twice, but must be called immediately (here) before it is returned. Then is called by unity next frame...
                    _instance.Start();
                }
                return _instance;
            }
            private set => _instance = value;
        }

        private void Awake()
        {
            if (_instance == this) return;
            if (_instance == null) {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        #endregion

        private void Start()
        {
            // TODO: More advanced music control system
            if (_musicController == null) {
                GameObject musicController = new GameObject("Music Controller", typeof(AudioSourceController));
                musicController.transform.SetParent(transform);
                _musicController = musicController.GetComponent<AudioSourceController>();
            }
            if (_backgroundMusic != null) {
                _backgroundMusic.Play(_musicController);
            }
            // SFX Pool
            if (_poolParent == null) {
                Transform sfxPool = new GameObject("SFX Pool").transform;
                sfxPool.SetParent(transform);
                _poolParent = sfxPool;
            }
            BuildInitialPool(_initialPoolSize);
        }

        public AudioSourceController GetController()
        {
            Debug.Log(_pool.Count);
            if (_pool.Count > 0) {
                AudioSourceController output = _pool[0];
                _pool.Remove(output);
                output.gameObject.SetActive(true);
                return output;
            }
            GameObject obj = new GameObject(_audioPlayerName);
            return obj.AddComponent<AudioSourceController>();
        }

        public void PutController(AudioSourceController controller)
        {
            if (_pool.Contains(controller)) return;
            controller.Claimed = false;
            AddToPool(controller);
        }

        private void BuildInitialPool(int size)
        {
            int currentSize = _pool.Count;
            for (int i = currentSize; i < size; ++i) {
                GameObject obj = new GameObject(_audioPlayerName);
                obj.transform.SetParent(_poolParent);
                AddToPool(obj.AddComponent<AudioSourceController>());
            }
        }

        private void AddToPool(AudioSourceController controller)
        {
            _pool.Add(controller);
            controller.gameObject.SetActive(false);
        }
    }
}