using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HostMainMenu : MonoBehaviour
{
    [SerializeField] Canvas _menu;
    [SerializeField] Canvas _credits;

    public void PlayButton()
    {
        SceneManager.LoadScene("HostMenu");
    }

    public void CreditButton()
    {
        _credits.enabled = true;
    }

    public void BackToMenu()
    {
        _credits.enabled = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.ExternalEval("window.close();");
#elif UNITY_STANDALONE
            Application.Quit();
#else
            Application.Quit();
#endif
    }
}
