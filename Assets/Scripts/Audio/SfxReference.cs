using System;
using UnityEngine;

namespace Assets.Scripts.Audio
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

        private bool NullTest()
        {
            if (UseConstant) {
                return Clip == null;
            }
            return Data == null;
        }
    }
}