using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform leftpoint,rightpoint;
    public float leftx,rightx;
    private bool Faceleft = true;
    public float Speed;
    private Animator Anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //删除子项目，不再继承父项目所有移动
        // transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //如果面向向左
        if(Faceleft)
        {
            //向左移动
            rb.velocity= new Vector2(-Speed,rb.velocity.y);
            //超过预定位置，反向
            if(transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1,1,1);
                Faceleft = false;
            } 
        }
        else 
        {
            rb.velocity= new Vector2(Speed,rb.velocity.y);
            if(transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1,1,1);
                Faceleft = true;
            } 
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    //public 让player调用
    public void JumpOn()
    {
        Anim.SetTrigger("death");
        
    }
}
