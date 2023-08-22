using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    internal static Scene_Manager Instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadScene(SceneName.LoginScene);

    }


    internal void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName.ToString());
    }
}

internal enum SceneName
{
    Bootstrap,
    LoginScene,
    MainMenu,
    Lobby,
    Gameplay,
}