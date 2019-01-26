using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Collection of UI Functions not tied to any particular object
/// </summary>
public class UIFunctions : MonoBehaviour {
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
