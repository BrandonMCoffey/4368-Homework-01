using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class SfxReference
    {
        public bool UseConstant;
        public AudioClip Clip;
        public SfxData Data;

        #region Constructors

        public SfxReference()
        {
            UseConstant = true;
            Clip = null;
        }

        public SfxReference(AudioClip clip)
        {
            UseConstant = true;
            Clip = clip;
        }

        public SfxReference(SfxData data)
        {
            UseConstant = false;
            Data = data;
        }

        #endregion

        public void Play()
        {
            if (NullTest()) return;
            if (UseConstant) {
                AudioHelper.PlayClip(Clip);
            } else {
                Data.Play();
            }
        }

        public void Play(AudioSourceController controller)
        {
            if (NullTest()) return;
            if (UseConstant) {
                controller.SetSourceProperties(Clip, 1, 1, true, 0);
                controller.Play();
            } else {
                Data.Play(controller);
            }
        }

        public void PlayAtPosition(Vector3 position)
        {
            if (NullTest()) return;
            if (UseConstant) {
                AudioHelper.PlayClip(Clip, position);
            } else {
                Data.PlayAtPosition(position);
            }
        }

        public void PlayWithParent(Transform parent)
        {
            if (NullTest()) return;
            if (UseConstant) {
                AudioHelper.PlayClip(Clip, parent);
            } else {
                Data.PlayWithParent(parent);
            }
        }

        public bool NullTest()
        {
            if (UseConstant) {
                return Clip == null;
            }
            return Data == null;
        }
    }
}