using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    internal static Scene_Manager Instance;
    void Awake()
    {
        Instance = this;
    }

    internal enum SceneName
    {
        LoginScene,
        SampleScene,
        Main
    }

    internal void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
