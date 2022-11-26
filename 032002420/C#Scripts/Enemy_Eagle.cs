using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform uppoint,downpoint;
    public float upy,downy;
    private bool Faceup = true;
    public float Speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //删除子项目，不再继承父项目所有移动
        transform.DetachChildren();
        upy = uppoint.position.y;
        downy = downpoint.position.y;
        Destroy(uppoint.gameObject);
        Destroy(downpoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //如果面向向上
        if(Faceup)
        {
            //向上移动
            rb.velocity= new Vector2(rb.velocity.x,Speed);
            //超过预定位置，反向
            if(transform.position.y > upy)
            {
                // transform.localScale = new Vector3(1,1,1);
                Faceup = false;
            } 
        }
        else 
        {
            rb.velocity= new Vector2(rb.velocity.x,-Speed);
            if(transform.position.y < downy)
            {
                // transform.localScale = new Vector3(1,1,1);
                Faceup = true;
            } 
        }
    }
}
