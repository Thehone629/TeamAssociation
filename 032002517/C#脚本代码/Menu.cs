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
        /*Debug.Log("��¼��Ϸ");*/
        SceneManager.LoadSceneAsync(1);

    }
    public void OnStartGameClick()
    {
       /* Debug.Log("��ʼ��Ϸ");*/
        SceneManager.LoadSceneAsync(3);
    }
    public void OnAboutUsClick()
    {
        Debug.Log("��������");
    }
    public void OnExitClick()
    {
        /*Debug.Log("�˳���Ϸ");*/
        Application.Quit();

    }
}
