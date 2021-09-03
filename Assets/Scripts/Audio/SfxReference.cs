using System;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Audio {
    [Serializable]
    public class SfxReference {
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
                PoolController(Clip);
            } else {
                Data.Play();
            }
        }

        public void PlayAtPosition(Vector3 position)
        {
            if (NullTest()) return;
            if (UseConstant) {
                var controller = PoolController(Clip);
                controller.transform.position = position;
            } else {
                Data.PlayAtPosition(position);
            }
        }

        public void PlayWithParent(Transform parent)
        {
            if (NullTest()) return;
            if (UseConstant) {
                var controller = PoolController(Clip);
                controller.SetParent(parent);
            } else {
                Data.PlayWithParent(parent);
            }
        }

        private bool NullTest()
        {
            if (UseConstant) {
                return Clip == null;
            } else {
                return Data == null;
            }
        }

        // TODO: Redundant Call mixed with SfxData Functions. Where to put?
        private AudioSourceController PoolController(AudioClip clip)
        {
            var controller = AudioManager.Instance.GetController();
            controller.Reset();
            controller.Claimed = true;
            controller.SetSourceProperties(clip, 1, 1, false, 0);
            controller.Play();
            return controller;
        }
    }
}