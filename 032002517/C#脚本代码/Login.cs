using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Login : MonoBehaviour
{
    public InputField acountInput;
    public InputField pwdInput;
    public Text tips;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //按下登录按钮就进行数据校验
    public void OnClickLogin()
    {
        string acount=acountInput.text;
        string password=pwdInput.text;
        if (acount == "1234567890" && password == "147")
        {
            //跳转页面
            SceneManager.LoadSceneAsync(3);
        }
        else
        {
            tips.text = "账号或密码错误";
        }
    }
    public void OnClickRegisterBtn()
    {
        //按下注册按钮就跳转到注册页面
        SceneManager.LoadSceneAsync(2);
    }
    public void OnClickBackBtn()
    {
        //按下返回按钮就返回主菜单
        SceneManager.LoadSceneAsync(0);
    }

}
