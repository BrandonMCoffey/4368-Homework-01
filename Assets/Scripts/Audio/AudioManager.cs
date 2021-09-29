using System.Collections;
using UnityEngine;
using Utility;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool _dontDestroyOnLoad = false;
        [Header("Music Controller")]
        [SerializeField] private AudioSourceController _musicController;
        [Header("Audio Pool")]
        [SerializeField] private string _audioPlayerName = "SFX Player";
        [SerializeField] private Transform _poolParent;
        [SerializeField] private int _initialPoolSize = 5;

        private PoolManager<AudioSourceController> _poolManager = new PoolManager<AudioSourceController>();
        private static string _defaultObjectName = "Audio Manager";
        private static string _defaultPoolName = "SFX Pool";

        #region Singleton

        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null) {
                    _instance = new GameObject(_defaultObjectName, typeof(AudioManager)).GetComponent<AudioManager>();
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
                if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            #endregion

            // TODO: More advanced music control system
            if (_musicController == null) {
                GameObject musicController = new GameObject("Music Controller");
                musicController.transform.SetParent(transform);
                _musicController = musicController.AddComponent<AudioSourceController>();
            }
            // SFX Pool
            if (_poolParent == null) {
                Transform pool = new GameObject(_defaultPoolName).transform;
                pool.SetParent(transform);
                _poolParent = pool;
            }
            _poolManager.BuildInitialPool(_poolParent, _audioPlayerName, _initialPoolSize);
        }

        public void PlayMusic(SfxReference musicTrack, float fadeIn)
        {
            musicTrack.Play(_musicController);
            StartCoroutine(FadeIn(_musicController, fadeIn));
        }

        public void PlayMusic(SfxData musicTrack, float fadeOut, bool crossFade, float fadeIn)
        {
            // TODO: Cross fade?
        }

        public IEnumerator FadeIn(AudioSourceController controller, float fadeIn)
        {
            for (float t = 0; t < fadeIn; t += Time.deltaTime) {
                controller.SetCustomVolume(t / fadeIn);
                yield return null;
            }
            controller.SetCustomVolume(1);
        }

        public AudioSourceController GetController()
        {
            return _poolManager.GetObjectFromPool();
        }

        public void ReturnController(AudioSourceController controller)
        {
            _poolManager.PutObjectIntoPool(controller);
        }
    }
}