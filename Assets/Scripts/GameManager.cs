using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        [SerializeField] private KeyCode _restartKey = KeyCode.Backspace;

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}