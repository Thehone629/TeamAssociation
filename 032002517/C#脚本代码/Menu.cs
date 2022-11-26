using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLoginClick()
    {
        /*Debug.Log("登录游戏");*/
        SceneManager.LoadSceneAsync(1);

    }
    public void OnStartGameClick()
    {
       /* Debug.Log("开始游戏");*/
        SceneManager.LoadSceneAsync(3);
    }
    public void OnAboutUsClick()
    {
        Debug.Log("关于我们");
    }
    public void OnExitClick()
    {
        /*Debug.Log("退出游戏");*/
        Application.Quit();

    }
}
