using Items;

namespace Entities.Interactable.Interactions {
    public class HittableInteraction : BaseInteraction {

        private InteractableEntityController _entityController;

        void Awake() {
            _entityController = GetComponent<InteractableEntityController>();
        }

        public override void Interact(BaseItem item) {
            if (!_entityController)
                return;

            if (item.GetType() == typeof(SwordItem)) {
                SwordItem sword = (SwordItem) item;
                
            }
        }
    }
}