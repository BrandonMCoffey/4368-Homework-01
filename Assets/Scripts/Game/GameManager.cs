using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.CustomFloats;
using Utility.GameEvents.Logic;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Related Keys")]
        [SerializeField] private KeyCode _restartKey = KeyCode.Backspace;
        [SerializeField] private KeyCode _exitGameKey = KeyCode.Escape;
        [Header("Music")]
        [SerializeField] private SfxReference _musicToPlay = new SfxReference();
        [Header("Winning and Losing")]
        [SerializeField] private float _slowDownTime = 4;

        private void Start()
        {
            AudioManager.Instance.PlayMusic(_musicToPlay, 2);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(_exitGameKey)) {
                Application.Quit();
            }
        }

        public void Defeat()
        {
            StartCoroutine(SlowSceneToStop());
        }

        private IEnumerator SlowSceneToStop()
        {
            for (float t = 0; t < _slowDownTime; t += 0.01f) {
                float delta = t / _slowDownTime;
                Debug.Log(delta);
                Time.timeScale = 1 - delta;
                yield return null;
            }
            Time.timeScale = 0;
        }
    }
}