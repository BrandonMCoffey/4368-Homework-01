using UnityEngine;

namespace Assets.Scripts.Audio
{
    public static class AudioHelper
    {
        public static void PlayClip(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false, float spacialBlend = 0)
        {
            var controller = PoolController();
            controller.SetSourceProperties(clip, volume, pitch, loop, spacialBlend);
            controller.Play();
        }

        public static void PlayClip(AudioClip clip, Vector3 position, float volume = 1, float pitch = 1, bool loop = false, float spacialBlend = 0)
        {
            var controller = PoolController();
            controller.SetSourceProperties(clip, volume, pitch, loop, spacialBlend);
            controller.SetPosition(position);
            controller.Play();
        }

        public static void PlayClip(AudioClip clip, Transform followParent, float volume = 1, float pitch = 1, bool loop = false, float spacialBlend = 0)
        {
            var controller = PoolController();
            controller.SetSourceProperties(clip, volume, pitch, loop, spacialBlend);
            controller.SetParent(followParent);
            controller.Play();
        }

        private static AudioSourceController PoolController()
        {
            var controller = AudioManager.Instance.GetController();
            controller.Reset();
            controller.Claimed = true;
            return controller;
        }
    }
}