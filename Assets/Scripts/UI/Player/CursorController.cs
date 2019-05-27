using Items;
using UnityEngine;

namespace UI.Player {
    public class CursorController : MonoBehaviour {

        [SerializeField] private Texture2D Default;
        [SerializeField] private Texture2D Sword;
        [SerializeField] private Texture2D SwordInvalid;
        [SerializeField] private Texture2D Torch;
        [SerializeField] private Texture2D TorchInvalid;
        [SerializeField] private Texture2D Bow;
        [SerializeField] private Texture2D BowInvalid;
        [SerializeField] private Texture2D Rune;
        [SerializeField] private Texture2D RuneInvalid;
        [SerializeField] private Texture2D Shield;
        
        [SerializeField] private Vector2 HotSpot = Vector2.zero;
        
        private CursorMode _cursorMode = CursorMode.Auto;

        public void Awake() {
            Cursor.SetCursor(Default, HotSpot, _cursorMode);
        }
        
        public void UpdateCursor(BaseItem.Type type, bool hasValidTarget, bool shielding = false) {

            Texture2D cursorTexture;
            if (!shielding) {
                switch (type) {
                    case BaseItem.Type.Sword:
                        cursorTexture = hasValidTarget ? Sword : SwordInvalid;
                        break;
                    case BaseItem.Type.Torch:
                        cursorTexture = hasValidTarget ? Torch : TorchInvalid;
                        break;
                    case BaseItem.Type.Bow:
                        cursorTexture = hasValidTarget ? Bow : BowInvalid;
                        break;
                    case BaseItem.Type.Rune:
                        cursorTexture = hasValidTarget ? Rune : RuneInvalid;
                        break;
                    default:
                        cursorTexture = Default;
                        break;
                }
            } else {
                cursorTexture = Shield;
            }
            
            Cursor.SetCursor(cursorTexture, HotSpot, _cursorMode);
        }
    }
}
