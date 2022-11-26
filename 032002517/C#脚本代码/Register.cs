using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField acountInput;
    public InputField pwdInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBackBtn()
    {
        //按下返回键，返回登录界面
        SceneManager.LoadSceneAsync(1);
    }
    public void OnClickConfirmBtn()
    {
        //按下确定键就进行注册提交数据到服务器，然后跳转到登录界面
        Debug.Log("点击了确认注册按钮");
    }
}
