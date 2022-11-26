using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

    //������Ҵ���������
    [SerializeField]
    private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField]
    private float moveSpeed = 15f;

    [SerializeField]
    private Bullet bulletPrefab;

    [Networked]
    public NetworkButtons ButtonsPrevious { get; set; }//��¼��ť��һ��״̬

    [SerializeField]
    private int maxHp = 100;//����һ�����Ѫ��

    [SerializeField]
    private Image hpBar = null;//��ʾѪ��

    //��������Կ��������������������ͬ��
    [Networked(OnChanged = nameof(OnHpChanged))]
    public int Hp { get; set; }//������¼Ѫ��

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    //��Ϸ����ʱ������������
    public override void Spawned()
    {
       

        if (Object.HasStateAuthority)//Ѫ��ֻ���ŷ����Ͻ��г�ʼ�����Ա�֤��Ϸ�Ĺ�ƽ��
        {
            Hp = maxHp;//��ʼ��Ѫ��
        }

    }
    public override void FixedUpdateNetwork()
    {
        //�������ϸ������ǵ�״̬
        if (GetInput(out NetworkInputData data))
        {
            //�ô�����������
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            Vector3 moveVector = data.movementInput.normalized;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            if (pressed.IsSet(InputButtons.JUM))
            {
                //�����˰�ť��ִ����Ծ�߼�
                networkCharacterController.Jump();
            }
            if (pressed.IsSet(InputButtons.FIRE))
            {
                //��������������ִ�з����ӵ�
                Runner.Spawn(bulletPrefab,
                    transform.position + transform.TransformDirection(Vector3.forward),
                    Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)),
                    Object.InputAuthority);
                //���һ��������ʾ�ӵ��ӵ�ǰ������Ϸ���󷢳�
            }
        }
        if (Hp <= 0 || networkCharacterController.transform.position.y <= -5f)
        {
            Respawn();//Ѫ��û���˾��������ߵ�����ͼ��ʱ������
        }
    }
    //��������
    private void Respawn()
    {
        networkCharacterController.transform.position = Vector3.up * 2;
        Hp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        if (Object.HasStateAuthority)
        {
            Hp -= damage;
        }
    }

    private static void OnHpChanged(Changed<PlayerController> changed)
    {
        //�����������Ϊstatic
        //����Ѫ���仯
        changed.Behaviour.hpBar.fillAmount = (float)changed.Behaviour.Hp / changed.Behaviour.maxHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {//������R�İ���
            ChangeColor_RPC(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {//������G�İ���
            ChangeColor_RPC(Color.green);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {//������B�İ���
            ChangeColor_RPC(Color.blue);
        }
    }

    //���ΪRPC������RPC��������ͬ����ɫ����ɫ
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void ChangeColor_RPC(Color newColor)
    {
        meshRenderer.material.color = newColor;
    }

   
}
