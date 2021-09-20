using UnityEngine;

namespace Audio
{
    [CreateAssetMenu]
    public class SfxData : ScriptableObject
    {
        [SerializeField] private AudioClip _clip = null;
        [SerializeField] private bool _loop = false;

        [SerializeField] [Range(0, 1)] private float _volume = 1f;
        [SerializeField] [Range(.25f, 3)] private float _pitch = 1f;
        [SerializeField] [Range(0f, 1f)] private float _spacialBlend = 0f;

        public void Play()
        {
            if (_clip == null) return;
            AudioHelper.PlayClip(_clip, _volume, _pitch, _loop, _spacialBlend);
        }

        public void Play(AudioSourceController controller)
        {
            if (_clip == null) return;
            controller.SetSourceProperties(_clip, _volume, _pitch, _loop, _spacialBlend);
            controller.Play();
        }

        public void PlayAtPosition(Vector3 position)
        {
            if (_clip == null) return;
            AudioHelper.PlayClip(_clip, position, _volume, _pitch, _loop, _spacialBlend);
        }

        public void PlayWithParent(Transform parent)
        {
            if (_clip == null) return;
            AudioHelper.PlayClip(_clip, parent, _volume, _pitch, _loop, _spacialBlend);
        }
    }
}