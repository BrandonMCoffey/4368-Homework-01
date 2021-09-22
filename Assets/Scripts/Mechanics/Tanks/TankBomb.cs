using UnityEngine;

namespace Mechanics.Tanks
{
    public class TankBomb : MonoBehaviour
    {
        [SerializeField] private GameObject _bombReference = null;

        public void DropBomb()
        {
            if (_bombReference == null) return;

            Instantiate(_bombReference, transform.position, Quaternion.identity);
        }
    }
}