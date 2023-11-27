using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
}