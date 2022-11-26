using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

    //接收玩家传来的数据
    [SerializeField]
    private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField]
    private float moveSpeed = 15f;

    [SerializeField]
    private Bullet bulletPrefab;

    [Networked]
    public NetworkButtons ButtonsPrevious { get; set; }//记录按钮是一个状态

    [SerializeField]
    private int maxHp = 100;//定义一个最大血量

    [SerializeField]
    private Image hpBar = null;//显示血条

    //加这个属性可以让这个变量在网络上同步
    [Networked(OnChanged = nameof(OnHpChanged))]
    public int Hp { get; set; }//用来记录血量

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    //游戏启动时会调用这个函数
    public override void Spawned()
    {
       

        if (Object.HasStateAuthority)//血量只在伺服器上进行初始化，以保证游戏的公平性
        {
            Hp = maxHp;//初始化血量
        }

    }
    public override void FixedUpdateNetwork()
    {
        //在网络上更新我们的状态
        if (GetInput(out NetworkInputData data))
        {
            //拿传过来的数据
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            Vector3 moveVector = data.movementInput.normalized;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            if (pressed.IsSet(InputButtons.JUM))
            {
                //按下了按钮，执行跳跃逻辑
                networkCharacterController.Jump();
            }
            if (pressed.IsSet(InputButtons.FIRE))
            {
                //按下了鼠标左键就执行发射子弹
                Runner.Spawn(bulletPrefab,
                    transform.position + transform.TransformDirection(Vector3.forward),
                    Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)),
                    Object.InputAuthority);
                //最后一个参数表示子弹从当前网络游戏对象发出
            }
        }
        if (Hp <= 0 || networkCharacterController.transform.position.y <= -5f)
        {
            Respawn();//血量没有了就重生或者掉到地图外时就重生
        }
    }
    //重生方法
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
        //这个方法必须为static
        //制作血条变化
        changed.Behaviour.hpBar.fillAmount = (float)changed.Behaviour.Hp / changed.Behaviour.maxHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {//检测键盘R的按下
            ChangeColor_RPC(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {//检测键盘G的按下
            ChangeColor_RPC(Color.green);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {//检测键盘B的按下
            ChangeColor_RPC(Color.blue);
        }
    }

    //标记为RPC，利于RPC在网络上同步角色的颜色
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void ChangeColor_RPC(Color newColor)
    {
        meshRenderer.material.color = newColor;
    }

   
}
