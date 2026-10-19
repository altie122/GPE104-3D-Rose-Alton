using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    
    private Button exitButton;
    
    private VisualElement creditsRoot;

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
        
        creditsRoot = null;
        
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
        
        creditsRoot = rootElement;
        
        exitButton = creditsRoot.Q<Button>("MainMenuBtn");

        exitButton.clicked += ExitScreenCallback;
    }
    
    private void ExitScreenCallback()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    
    private void UnbindButtons()
    {
        if (exitButton != null)
        {
            exitButton.clicked -= ExitScreenCallback;
            exitButton = null;
        }
    }
}
