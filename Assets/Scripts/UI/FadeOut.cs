using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeOut : MonoBehaviour
    {
        [SerializeField] private float _inOffset = 0;
        [SerializeField] private float _outOffset = 0;
        [SerializeField] private float _fadeInTime = 1;
        [SerializeField] private float _fadeOutTime = 1;
        [SerializeField] private Button _buttonToDisable = null;
        [SerializeField] private Image _imageToFade = null;
        [SerializeField] private TextMeshProUGUI _textToFade = null;

        private float _startImageAlpha = 1;
        private float _startTextAlpha = 1;

        private void SetValues()
        {
            if (_imageToFade != null) {
                _imageToFade.gameObject.SetActive(true);
                _startImageAlpha = _imageToFade.color.a;
            }
            if (_textToFade != null) {
                _textToFade.gameObject.SetActive(true);
                _startTextAlpha = _textToFade.color.a;
            }
        }

        public void StartFadeIn()
        {
            SetValues();
            if (_buttonToDisable != null) {
                _buttonToDisable.enabled = true;
            }
            if (_imageToFade != null) {
                Color col = _imageToFade.color;
                col.a = 0;
                _imageToFade.color = col;
            }
            if (_textToFade != null) {
                Color col = _textToFade.color;
                col.a = 0;
                _textToFade.color = col;
            }
            StartCoroutine(Fade(_inOffset, _fadeInTime));
        }

        public void StartFadeOut()
        {
            SetValues();
            if (_buttonToDisable != null) {
                _buttonToDisable.enabled = false;
            }
            StartCoroutine(Fade(_outOffset, _fadeOutTime, 1));
        }

        private IEnumerator Fade(float offset, float fadeTime, float reverse = 0)
        {
            yield return new WaitForSecondsRealtime(offset);
            for (float t = 0; t < fadeTime; t += Time.deltaTime) {
                float delta = Mathf.Abs(reverse - t / fadeTime);
                if (_imageToFade != null) {
                    Color col = _imageToFade.color;
                    col.a = _startImageAlpha * delta;
                    _imageToFade.color = col;
                }
                if (_textToFade != null) {
                    Color col = _textToFade.color;
                    col.a = _startTextAlpha * delta;
                    _textToFade.color = col;
                }
                yield return null;
            }
            if (_imageToFade != null) {
                _imageToFade.gameObject.SetActive(reverse == 0);
            }
        }
    }
}