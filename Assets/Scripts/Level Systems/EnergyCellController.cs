using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Player_Systems;
using UnityEngine;

namespace Level_Systems
{
    public class EnergyCellController : MonoBehaviour
    {
        [SerializeField] private List<EnergyCell> _energyCells = new List<EnergyCell>();
        [SerializeField] private Transform _playerTank;
        [Header("Laser Attack")]
        [SerializeField] private float _chargeCellTime = 0.8f;
        [SerializeField] private float _laserTime = 0.8f;
        [SerializeField] private float _fadeCellTime = 0.4f;
        [SerializeField] private float _inBetweenTime = 1f;
        [Header("Platform Summoning")]
        [SerializeField] private float _chargeCellTime1 = 0.8f;
        [SerializeField] private float _summonTime = 2;
        [SerializeField] private float _waitTime = 0.4f;
        [SerializeField] private List<GameObject> _enemyToSpawn = new List<GameObject>();
        [SerializeField] private List<GameObject> _collectibleToSpawn = new List<GameObject>();

        private void Awake()
        {
            _energyCells = _energyCells.Where(item => item != null).ToList();
            if (_playerTank == null) {
                _playerTank = FindObjectOfType<PlayerTank>().transform;
            }
        }

        public float BeginLaserAttack(float startTime)
        {
            bool startFromLeft = _playerTank.position.x > 0;
            int energyCellCount = _energyCells.Count;
            for (var i = 0; i < energyCellCount; i++) {
                var cell = startFromLeft ? _energyCells[i] : _energyCells[energyCellCount - i - 1];
                startTime += _inBetweenTime;
                StartCoroutine(LaserAttack(cell, startTime));
            }
            return startTime + _chargeCellTime + _laserTime + _fadeCellTime;
        }

        private IEnumerator LaserAttack(EnergyCell cell, float startDelay)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            cell.StartLaserSequence();

            for (float t = 0; t < _chargeCellTime; t += Time.deltaTime) {
                float delta = t / _chargeCellTime;
                cell.Charge(delta);
                yield return null;
            }
            cell.ActivateLaser(_laserTime);
            yield return new WaitForSecondsRealtime(_laserTime);
            cell.DeactivateLaser();
            for (float t = 0; t < _fadeCellTime; t += Time.deltaTime) {
                float delta = t / _fadeCellTime;
                cell.DeCharge(delta);
                yield return null;
            }
            cell.FinishLaserSequence();
        }

        public float StartPlatformSummoning(float amount)
        {
            bool startFromLeft = _playerTank.position.x > 0;
            int energyCellCount = _energyCells.Count;
            for (var i = 0; i < energyCellCount; i++) {
                var cell = startFromLeft ? _energyCells[i] : _energyCells[energyCellCount - i - 1];
                cell.Respawn();
                StartCoroutine(PlatformSummoning(cell, (energyCellCount) * 10 - 10 * i, 40 + 10 * i, amount));
            }
            return _chargeCellTime1 + _summonTime + _waitTime;
        }

        private IEnumerator PlatformSummoning(EnergyCell cell, int good, int bad, float amount)
        {
            cell.StartSummonSequence();

            for (float t = 0; t < _chargeCellTime; t += Time.deltaTime) {
                float delta = t / _chargeCellTime;
                cell.ChargeSummon(delta);
                yield return null;
            }
            cell.Summon(_collectibleToSpawn, good, _enemyToSpawn, bad, amount);
        }
    }
}