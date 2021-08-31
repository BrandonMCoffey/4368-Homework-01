using UnityEngine;

namespace Assets.Scripts.Enemies {
    public enum EnemyTankType {
        Brown,
        Grey,
        Teal,
        Yellow,
        Red,
        Green,
        Purple,
        White,
        Black
    }

    [CreateAssetMenu]
    public class EnemyTankData : ScriptableObject {
        [SerializeField] private EnemyTankType _type = EnemyTankType.Brown;
        [SerializeField] private Material _material = null;

        public Color Color => GetColor(_type);
        public Material Material => _material;

        public static Color GetColor(EnemyTankType type)
        {
            return type switch
            {
                EnemyTankType.Brown  => new Color(161, 124, 63, 255) / 255,
                EnemyTankType.Grey   => new Color(119, 109, 99, 255) / 255,
                EnemyTankType.Teal   => new Color(66, 122, 117, 255) / 255,
                EnemyTankType.Yellow => new Color(131, 94, 132, 255) / 255,
                EnemyTankType.Red    => new Color(198, 91, 103, 255) / 255,
                EnemyTankType.Green  => new Color(85, 137, 71, 255) / 255,
                EnemyTankType.Purple => new Color(130, 94, 136, 255) / 255,
                EnemyTankType.White  => new Color(235, 220, 172, 255) / 255,
                EnemyTankType.Black  => new Color(56, 55, 52, 255) / 255,
                _                    => Color.magenta
            };
        }
    }
}