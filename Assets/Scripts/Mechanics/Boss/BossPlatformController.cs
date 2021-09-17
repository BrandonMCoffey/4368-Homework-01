using System.ComponentModel;
using Assets.Scripts.Level_Systems;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Boss
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

        public Transform GetNewDestination()
        {
            switch (_currentPlatformOption) {
                case PlatformOptions.Left:
                case PlatformOptions.Right:
                    return _centerBossPlatform.transform;
                case PlatformOptions.Center:
                    // TODO: Better way to do this?
                    int rand = Random.Range(0, 100);
                    return rand < 50 ? _leftBossPlatform.transform : _rightBossPlatform.transform;
                default:
                    return null;
            }
        }

        public void Lower(BossStateMachine boss, PlatformOptions option)
        {
            BossPlatform platform = GetPlatform(option);
            if (platform != null) platform.PrepareToLower(boss);
            _currentPlatformOption = PlatformOptions.Null;
        }

        public void Raise(BossStateMachine boss, PlatformOptions option)
        {
            if (_currentPlatformOption != PlatformOptions.Null) {
                return;
            }
            BossPlatform platform = GetPlatform(option);
            if (platform != null) platform.PrepareToRaise(boss);
            _currentPlatformOption = option;
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
    }
}