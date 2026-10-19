using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private Button exitButton;
    private Button restartButton;
    
    private VisualElement pauseMenuRoot;
    
    public void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    public void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
        if (exitButton != null)
        {
            exitButton.clicked -= ExitGameCallback;
        }

        if (restartButton != null)
        {
            restartButton.clicked -= RestartGameCallback;
        }
        
        if (GameManager.instance)
        {
            GameManager.instance.endScreen = null;
        }
    }
    
    void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        if (GameManager.instance)
        {
            GameManager.instance.endScreen = this;
        }
        
        pauseMenuRoot = rootElement;
        
        exitButton = pauseMenuRoot.Q<Button>("ExitBtn");
        restartButton = pauseMenuRoot.Q<Button>("RestartBtn");
        
        pauseMenuRoot.style.display = DisplayStyle.None;
        
        exitButton.clicked += ExitGameCallback;
        restartButton.clicked += RestartGameCallback;
    }

    public void ShowEndScreen(string title, string subtitle)
    {
        pauseMenuRoot.Q<Label>("Title").text = title;
        pauseMenuRoot.Q<Label>("Subtitle").text = subtitle;
        pauseMenuRoot.style.display = DisplayStyle.Flex;
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
