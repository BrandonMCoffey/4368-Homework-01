using Assets.Scripts.Audio;
using Assets.Scripts.Utility.CustomFloats;
using Assets.Scripts.Utility.GameEvents.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Related Keys")]
        [SerializeField] private KeyCode _restartKey = KeyCode.Backspace;
        [SerializeField] private KeyCode _exitGameKey = KeyCode.Escape;
        [Header("Music")]
        [SerializeField] private SfxReference _musicToPlay = new SfxReference();
        [Header("Win Conditions")]
        [SerializeField] private FloatVariable _treasureCount = null;
        [SerializeField] private int _treasureToWin = 10;
        [SerializeField] private GameEvent _onWin = null;

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(_exitGameKey)) {
                Application.Quit();
            }
            if (_treasureCount.Value >= _treasureToWin) {
                Win();
            }
        }

        private void Win()
        {
            if (_onWin != null) {
                _onWin.Invoke();
            }
        }
    }
}