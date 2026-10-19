using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    
    private Button creditsButton;
    
    private VisualElement mainMenuRoot;
    
    public MainSettingsMenu settingsMenu;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }
    
    public void OnEnable()
    {
        inputSystem?.Enable();
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    public void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
        
        mainMenuRoot = null;
        
        inputSystem?.Disable();

        UnbindButtons();
    }

    private void OnDestroy()
    {
        inputSystem?.Disable();
        inputSystem?.Dispose();
        inputSystem = null;
    }
    
    void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        UnbindButtons();
        
        mainMenuRoot = rootElement;
        
        startButton = mainMenuRoot.Q<Button>("StartBtn");
        settingsButton = mainMenuRoot.Q<Button>("SettingsBtn");
        creditsButton = mainMenuRoot.Q<Button>("CreditsBtn");
        exitButton = mainMenuRoot.Q<Button>("ExitBtn");

        startButton.clicked += StartGameCallback;
        settingsButton.clicked += SettingsMenuCallback;
        creditsButton.clicked += CreditsCallback;
        exitButton.clicked += ExitGameCallback;
    }
    
    private void SettingsMenuCallback()
    {
        settingsMenu.ShowSettings();
        mainMenuRoot.style.display = DisplayStyle.None;
    }

    public void ShowMainMenu()
    {
        mainMenuRoot.style.display = DisplayStyle.Flex;
    }
    
    private void ExitGameCallback()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void StartGameCallback()
    {
        SceneManager.LoadScene("Scenes/Main", LoadSceneMode.Single);
    }
    
    private void CreditsCallback()
    {
        SceneManager.LoadScene("Scenes/Credits", LoadSceneMode.Single);
    }
    
    private void UnbindButtons()
    {
        if (startButton != null)
        {
            startButton.clicked -= StartGameCallback;
            startButton = null;
        }

        if (settingsButton != null)
        {
            settingsButton.clicked -= SettingsMenuCallback;
            settingsButton = null;
        }

        if (exitButton != null)
        {
            exitButton.clicked -= ExitGameCallback;
            exitButton = null;
        }
    }
}
