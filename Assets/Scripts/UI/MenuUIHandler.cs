using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//make sure this scripts executes after all important data is setup
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void SelectLevel()
    {
        // 1 should be default for level scene
        SceneManager.LoadScene(1);
    }
}