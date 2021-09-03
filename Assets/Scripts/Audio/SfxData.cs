using UnityEngine;

namespace Assets.Scripts.Audio {
    [CreateAssetMenu]
    public class SfxData : ScriptableObject {
        [SerializeField] private AudioClip _clip = null;
        [SerializeField] private bool _loop = false;

        [SerializeField] [Range(0, 1)] private float _volume = 1f;
        [SerializeField] [Range(.25f, 3)] private float _pitch = 1f;
        [SerializeField] [Range(0f, 1f)] private float _spacialBlend = 0f;

        public void Play()
        {
            if (_clip == null) return;
            PoolController(AudioManager.Instance.GetController());
        }

        public void Play(AudioSourceController controller)
        {
            PoolController(controller);
        }

        public void PlayAtPosition(Vector3 position)
        {
            if (_clip == null) return;
            AudioSourceController controller = PoolController(AudioManager.Instance.GetController());
            controller.SetPosition(position);
        }

        public void PlayWithParent(Transform parent)
        {
            if (_clip == null) return;
            AudioSourceController controller = PoolController(AudioManager.Instance.GetController());
            controller.SetParent(parent);
        }

        private AudioSourceController PoolController(AudioSourceController controller)
        {
            controller.Reset();
            controller.Claimed = true;
            controller.SetSourceProperties(_clip, _volume, _pitch, _loop, _spacialBlend);
            controller.Play();
            return controller;
        }
    }
}