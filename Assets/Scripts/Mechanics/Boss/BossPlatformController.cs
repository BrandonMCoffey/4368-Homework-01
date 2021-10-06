using System.ComponentModel;
using Level_Systems;
using UnityEngine;

namespace Mechanics.Boss
{
    public enum PlatformOptions
    {
        Null,
        Left,
        Center,
        Right
    }

    public class BossPlatformController : MonoBehaviour
    {
        [SerializeField] private BossPlatform _leftBossPlatform = null;
        [SerializeField] private BossPlatform _centerBossPlatform = null;
        [SerializeField] private BossPlatform _rightBossPlatform = null;

        private PlatformOptions _currentPlatformOption = PlatformOptions.Null;

        public bool IsOnPlatform => _currentPlatformOption != PlatformOptions.Null;

        public void SetPlatform(PlatformOptions currentPlatform)
        {
            _currentPlatformOption = currentPlatform;
        }

        public void Escalate()
        {
            _leftBossPlatform.Escalate();
            _centerBossPlatform.Escalate();
            _rightBossPlatform.Escalate();
        }

        public Transform GetNewDestination()
        {
            switch (_currentPlatformOption) {
                case PlatformOptions.Left:
                case PlatformOptions.Right:
                    return _centerBossPlatform.transform;
                case PlatformOptions.Center:
                    // TODO: Better way to do this?
                    int rand = Random.Range(0, 100);
                    if (rand < 50) {
                        _currentPlatformOption = PlatformOptions.Left;
                        return _leftBossPlatform.transform;
                    } else {
                        _currentPlatformOption = PlatformOptions.Right;
                        return _rightBossPlatform.transform;
                    }
                default:
                    return null;
            }
        }

        public float Lower(BossStateMachine boss)
        {
            BossPlatform platform = EnsureCurrentPlatform(boss);
            if (platform == null) return 0;

            platform.PrepareToLower(boss);
            _currentPlatformOption = PlatformOptions.Null;
            return platform.LowerTimer;
        }

        public float Raise(BossStateMachine boss)
        {
            int rand = Random.Range(0, 90);
            return rand < 30 ? Raise(boss, PlatformOptions.Left) : Raise(boss, rand < 60 ? PlatformOptions.Center : PlatformOptions.Right);
        }

        public float Raise(BossStateMachine boss, PlatformOptions option)
        {
            if (_currentPlatformOption != PlatformOptions.Null) {
                return 0;
            }
            BossPlatform platform = GetPlatform(option);
            if (platform == null) return 0;

            platform.PrepareToRaise(boss);
            _currentPlatformOption = option;
            return platform.TotalTime;
        }

        private BossPlatform GetPlatform(PlatformOptions option)
        {
            return option switch
            {
                PlatformOptions.Left   => _leftBossPlatform,
                PlatformOptions.Center => _centerBossPlatform,
                PlatformOptions.Right  => _rightBossPlatform,
                _                      => throw new InvalidEnumArgumentException(nameof(option) + " on " + gameObject)
            };
        }

        public BossPlatform EnsureCurrentPlatform(BossStateMachine boss)
        {
            BossPlatform current = GetPlatform(_currentPlatformOption);
            Vector3 bossPosition = boss.transform.position;
            if (Vector3.Distance(current.transform.position, bossPosition) > 4) {
                Debug.LogWarning("<color=white>Boss Platform has incorrect data. Correcting...</color>");
                // Test Left Platform
                if (_currentPlatformOption != PlatformOptions.Left) {
                    if (Vector3.Distance(_leftBossPlatform.transform.position, bossPosition) < 4) {
                        _currentPlatformOption = PlatformOptions.Left;
                        return _leftBossPlatform;
                    }
                }
                // Test Center Platform
                if (_currentPlatformOption != PlatformOptions.Center) {
                    if (Vector3.Distance(_centerBossPlatform.transform.position, bossPosition) < 4) {
                        _currentPlatformOption = PlatformOptions.Center;
                        return _centerBossPlatform;
                    }
                }
                // Test Right Platform
                if (_currentPlatformOption != PlatformOptions.Right) {
                    if (Vector3.Distance(_rightBossPlatform.transform.position, bossPosition) < 4) {
                        _currentPlatformOption = PlatformOptions.Right;
                        return _rightBossPlatform;
                    }
                }
            }
            return current;
        }
    }
}