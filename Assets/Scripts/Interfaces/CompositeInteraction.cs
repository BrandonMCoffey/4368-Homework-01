using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Interfaces {
    public class CompositeInteraction : MonoBehaviour, IInteractable {
        [SerializeField] private List<GameObject> _interactableGameObjects = new List<GameObject>();

        public void Interact()
        {
            foreach (var interactable in _interactableGameObjects.Select(interactableGameObject => interactableGameObject.GetComponent<IInteractable>())) {
                interactable?.Interact();
            }
        }
    }
}