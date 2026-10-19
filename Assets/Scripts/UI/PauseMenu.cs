using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    // public PauseStatus pauseStatus;
    
    private InputSystem_Actions inputSystem;
    
    private Button continueButton;
    private Button settingsButton;
    private Button exitButton;
    private Button restartButton;
    
    private VisualElement pauseMenuRoot;
    
    public SettingsMenu settingsMenu;
    
    public GameManager gameManager;
    
    public void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    public void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
        if (inputSystem != null)
        {
            inputSystem.Player.Pause.performed -= PauseGame;
            inputSystem.Disable();
            inputSystem.Dispose();
        }

        if (continueButton != null)
        {
            continueButton.clicked -= ResumeGameCallback;
        }

        if (settingsButton != null)
        {
            settingsButton.clicked -= SettingsMenuCallback;
        }

        if (exitButton != null)
        {
            exitButton.clicked -= ExitGameCallback;
        }

        if (restartButton != null)
        {
            restartButton.clicked -= RestartGameCallback;
        }
    }
    
    private void OnDestroy()
    {
        inputSystem?.Disable();
        inputSystem?.Dispose();
        inputSystem = null;
    }
    
    void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        inputSystem = new InputSystem_Actions();
        
        inputSystem.Enable();
        inputSystem.Player.Pause.performed += PauseGame;
        
        pauseMenuRoot = rootElement;
        
        continueButton = pauseMenuRoot.Q<Button>("ContinueBtn");
        settingsButton = pauseMenuRoot.Q<Button>("SettingsBtn");
        exitButton = pauseMenuRoot.Q<Button>("ExitBtn");
        restartButton = pauseMenuRoot.Q<Button>("RestartBtn");
        
        pauseMenuRoot.style.display = DisplayStyle.None;
        
        continueButton.clicked += ResumeGameCallback;
        settingsButton.clicked += SettingsMenuCallback;
        exitButton.clicked += ExitGameCallback;
        restartButton.clicked += RestartGameCallback;
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        if (!GameManager.instance.isPauseMenuEnabled)
        {
            return;
        }
        if (GameManager.instance.isPaused)
        {
            ResumeGameCallback();
        }
        else
        {
            Time.timeScale = 0f;
            GameManager.instance.isPaused = true;
            pauseMenuRoot.style.display = DisplayStyle.Flex;
        }
    }
    
    private void ResumeGameCallback()
    {
        Time.timeScale = 1f;
        GameManager.instance.isPaused = false;
        pauseMenuRoot.style.display = DisplayStyle.None;
    }

    private void SettingsMenuCallback()
    {
        settingsMenu.ShowSettings();
        pauseMenuRoot.style.display = DisplayStyle.None;
        inputSystem.Player.Pause.performed -= PauseGame;
    }

    public void ShowPauseMenu()
    {
        pauseMenuRoot.style.display = DisplayStyle.Flex;
        inputSystem.Player.Pause.performed += PauseGame;
    }

    private void ExitGameCallback()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void RestartGameCallback()
    {
        if (GameManager.instance)
        {
            GameManager.instance.ResetGameState();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
