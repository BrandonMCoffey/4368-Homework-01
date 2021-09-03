using UnityEngine;

namespace Assets.Scripts.Audio {
    public static class AudioHelper {
        public static AudioSource PlayClip(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false, float spacialBlend = 1)
        {
            // pool
            AudioSourceController controller = AudioManager.Instance.GetController();
            // configure
            controller.Reset();
            controller.Claimed = true;
            controller.SetSourceProperties(clip, volume, pitch, loop, spacialBlend);
            // activate
            controller.Play();
            // return (in case other things need it)
            return controller.Source;
        }
    }
}