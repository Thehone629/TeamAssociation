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
        //���·��ؼ������ص�¼����
        SceneManager.LoadSceneAsync(1);
    }
    public void OnClickConfirmBtn()
    {
        //����ȷ�����ͽ���ע���ύ���ݵ���������Ȼ����ת����¼����
        Debug.Log("�����ȷ��ע�ᰴť");
    }
}
