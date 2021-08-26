using Assets.Scripts.Utility.CustomFloats;
using Assets.Scripts.Utility.GameEvents.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        [SerializeField] private KeyCode _restartKey = KeyCode.Backspace;
        [SerializeField] private FloatVariable _treasureCount = null;
        [SerializeField] private int _treasureToWin = 10;
        [SerializeField] private GameEvent _onWin = null;

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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