using UnityEngine;

namespace Mechanics.Boss
{
    [CreateAssetMenu]
    public class BossAiData : ScriptableObject
    {
        [Header("Idle Time")]
        public Vector2 IdleTimeMinMax = new Vector2(2f, 6f);
        public Vector2 OutsideArenaMinMax = new Vector2(2f, 6f);
        [Header("Move To Platform")]
        public int ChangeToCharge = 20;
        public Vector2 WhenToCharge = new Vector2(1f, 5f);
        [Header("Charge Attack")]
        public float TimeToRotate = 1;
        public float ChargeAcceleration = 1;
        public float RetreatSpeed = 0.5f;
        public float ImpactHoldTime = 0.25f;
        [Header("Debug")]
        public bool Debug = true;
    }
}