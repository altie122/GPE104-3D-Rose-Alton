using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class StartOnKeypress : MonoBehaviour
{
    private void Start()
    {
        // On any button pressed, load the main menu scene
        InputSystem.onAnyButtonPress.CallOnce(ctrl => SceneManager.LoadScene("Main Menu", LoadSceneMode.Single));
    }
}
