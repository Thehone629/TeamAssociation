using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;


public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float bulletSpeed = 20f;//子弹的速度

    [Networked]
    private TickTimer life { get; set; }

    

    public override void Spawned()
    {
        //网络对象刚刚生成时就会执行这个函数
        life=TickTimer.CreateFromSeconds(Runner,5.0f);//设置5秒倒计时
    }


    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);//5秒倒计时结束就销毁当前网络对象
        }
        else
        {
            transform.position += bulletSpeed * transform.forward * Runner.DeltaTime;//让子弹往前飞 
        }
        
    }
    //子弹碰到网络玩家就减血
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//看是否有Player这个Tag
        {
            var player = other.GetComponent<PlayerController>();
           
            player.TakeDamage(5);
        }
        Runner.Despawn(Object);
    }
}
