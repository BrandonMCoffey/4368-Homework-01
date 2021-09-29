using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class ForceEnabled : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objToEnable = new List<GameObject>();
        [SerializeField] private List<GameObject> _objToDisable = new List<GameObject>();

        private void Awake()
        {
            foreach (var obj in _objToEnable.Where(obj => obj != null)) {
                obj.SetActive(true);
            }
            foreach (var obj in _objToDisable.Where(obj => obj != null)) {
                obj.SetActive(false);
            }
        }
    }
}