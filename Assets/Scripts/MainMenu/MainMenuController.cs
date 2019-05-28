using Game;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    
    [SerializeField] Button NewGameButton;
    [SerializeField] Button LoadButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button OpenLinkButton;

    void Awake() {
        NewGameButton.onClick.AddListener(OnNewGame);
//        LoadButton.onClick.AddListener(OnLoad);
        SettingsButton.onClick.AddListener(OnSettings);
        QuitButton.onClick.AddListener(OnQuit);
        OpenLinkButton.onClick.AddListener(OnOpenLink);
    }

    private void OnNewGame() {
        GameManager.Get().LoadNextLevel();
    }

    private void OnLoad() {
        
    }

    private void OnSettings() {
        
    }

    private void OnQuit() {
        Application.Quit();
    }

    private void OnOpenLink() {
        Application.OpenURL("https://itch.io/jam/weekly-game-jam-98");
    }
}
