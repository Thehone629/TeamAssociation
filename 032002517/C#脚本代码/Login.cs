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
    //���µ�¼��ť�ͽ�������У��
    public void OnClickLogin()
    {
        string acount=acountInput.text;
        string password=pwdInput.text;
        if (acount == "1234567890" && password == "147")
        {
            //��תҳ��
            SceneManager.LoadSceneAsync(3);
        }
        else
        {
            tips.text = "�˺Ż��������";
        }
    }
    public void OnClickRegisterBtn()
    {
        //����ע�ᰴť����ת��ע��ҳ��
        SceneManager.LoadSceneAsync(2);
    }
    public void OnClickBackBtn()
    {
        //���·��ذ�ť�ͷ������˵�
        SceneManager.LoadSceneAsync(0);
    }

}
