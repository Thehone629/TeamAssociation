using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;


public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float bulletSpeed = 20f;//�ӵ����ٶ�

    [Networked]
    private TickTimer life { get; set; }

    

    public override void Spawned()
    {
        //�������ո�����ʱ�ͻ�ִ���������
        life=TickTimer.CreateFromSeconds(Runner,5.0f);//����5�뵹��ʱ
    }


    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);//5�뵹��ʱ���������ٵ�ǰ�������
        }
        else
        {
            transform.position += bulletSpeed * transform.forward * Runner.DeltaTime;//���ӵ���ǰ�� 
        }
        
    }
    //�ӵ�����������Ҿͼ�Ѫ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//���Ƿ���Player���Tag
        {
            var player = other.GetComponent<PlayerController>();
           
            player.TakeDamage(5);
        }
        Runner.Despawn(Object);
    }
}
