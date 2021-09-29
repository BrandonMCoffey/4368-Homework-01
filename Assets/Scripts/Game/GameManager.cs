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
        [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;
        [SerializeField] private KeyCode _restartKey = KeyCode.Backspace;
        [Header("References")]
        [SerializeField] private GameObject _pauseMenuPanel = null;
        [Header("Music")]
        [SerializeField] private SfxReference _musicToPlay = new SfxReference();
        [Header("Winning and Losing")]
        [SerializeField] private float _slowDownTime = 4;

        private bool _isPaused;
        private bool _gameOver;

        private void Start()
        {
            PauseGame(false);
            AudioManager.Instance.PlayMusic(_musicToPlay, 2);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey)) {
                RestartLevel();
            }
            if (Input.GetKeyDown(_pauseKey)) {
                PauseGame();
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PauseGame()
        {
            PauseGame(!_isPaused);
        }

        public void PauseGame(bool pause)
        {
            _isPaused = pause;
            if (!_gameOver) {
                Time.timeScale = pause ? 0 : 1;
            }
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(pause);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Defeat()
        {
            _gameOver = true;
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
            _isPaused = true;
        }
    }
}