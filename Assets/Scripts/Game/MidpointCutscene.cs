using System.Collections;
using Mechanics.Boss;
using Mechanics.Player_Systems;
using UnityEngine;

namespace Game
{
    public class MidpointCutscene : MonoBehaviour
    {
        [SerializeField] private float _startPauseTime = 1;
        [SerializeField] private float _zoomInTime = 2.5f;
        [SerializeField] private float _screenShakeDuration = 0.5f;
        [SerializeField] private float _screenShakeIntensity = 0.5f;
        [SerializeField] private float _zoomOutTime = 2.5f;
        [SerializeField] private float _bossHealTime = 3f;
        [SerializeField] private float _endPauseTime = 0.2f;
        [SerializeField] private PlayerTank _player;
        [SerializeField] private Transform _tempPlayerArt = null;
        [SerializeField] private BossStateMachine _bossAi;
        [SerializeField] private BossTurret _turret;
        [SerializeField] private BossHealth _bossHealth;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleSystem _cutsceneParticles = null;

        public void Awake()
        {
            if (_player == null) {
                _player = FindObjectOfType<PlayerTank>();
            }
            if (_bossAi == null) {
                _bossAi = FindObjectOfType<BossStateMachine>();
            }
            if (_bossHealth == null) {
                _bossHealth = FindObjectOfType<BossHealth>();
            }
            if (_cameraController == null) {
                _cameraController = FindObjectOfType<CameraController>();
            }
        }

        public void StartCutscene()
        {
            _player.gameObject.SetActive(false);
            _tempPlayerArt.gameObject.SetActive(true);
            _bossAi.gameObject.SetActive(false);
            _turret.SetLockTurret(true);
            _tempPlayerArt.transform.SetPositionAndRotation(_player.transform.position, _player.transform.rotation);
            StartCoroutine(Cutscene());
        }

        private IEnumerator Cutscene()
        {
            yield return new WaitForSecondsRealtime(_startPauseTime);
            for (float t = 0; t < _zoomInTime; t += Time.deltaTime) {
                float delta = t / _zoomInTime;
                _cameraController.Zoom(delta);
                yield return null;
            }

            _cameraController.ShakeCamera(_screenShakeDuration, _screenShakeIntensity);
            if (_cutsceneParticles != null) {
                _cutsceneParticles.Play();
            }
            yield return new WaitForSecondsRealtime(_screenShakeDuration);

            for (float t = 0; t < _zoomOutTime; t += Time.deltaTime) {
                float delta = t / _zoomOutTime;
                _cameraController.Zoom(1 - delta);
                yield return null;
            }

            for (float t = 0; t < _bossHealTime; t += Time.deltaTime) {
                float delta = t / _bossHealTime;
                _bossHealth.HealEnrage(delta);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(_endPauseTime);
            EndCutscene();
        }

        private void EndCutscene()
        {
            _player.gameObject.SetActive(true);
            _tempPlayerArt.gameObject.SetActive(false);
            _turret.SetLockTurret(false);
            _bossAi.gameObject.SetActive(true);
            _bossAi.UpdateBossStage(BossStage.Enraged);
        }
    }
}