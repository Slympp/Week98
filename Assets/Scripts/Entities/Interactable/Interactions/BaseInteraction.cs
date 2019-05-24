using Items;
using UnityEngine;

namespace Entities.Interactable.Interactions {
    public abstract class BaseInteraction : MonoBehaviour {
        public abstract void Interact(BaseItem item);
    }
}